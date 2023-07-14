using Mopups.Interfaces;
using Mopups.Pages;
using Prism.Common;
using NavigationMode = Prism.Navigation.NavigationMode;

namespace Tonestro.Prism.Plugin.Mopups;

/// <summary>
/// Implements the <see cref="INavigationService" /> for working with <see cref="PopupPage" />
/// </summary>
public class PopupPageNavigationService : PageNavigationService
{
    private readonly IPageAccessor _pageAccessor;

    /// <summary>
    /// Gets the <see cref="IPopupNavigation" /> service.
    /// </summary>
    protected IPopupNavigation PopupNavigation { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="PopupPageNavigationService" />
    /// </summary>
    /// <param name="popupNavigation"></param>
    /// <param name="container"></param>
    /// <param name="windowManager"></param>
    /// <param name="eventAggregator"></param>
    /// <param name="pageAccessor"></param>
    public PopupPageNavigationService(IPopupNavigation popupNavigation, IContainerProvider container,
        IWindowManager windowManager, IEventAggregator eventAggregator, IPageAccessor pageAccessor)
        : base(container, windowManager, eventAggregator, pageAccessor)
    {
        PopupNavigation = popupNavigation;
        // _page = windowManager.Windows[^1].Page;
        _pageAccessor = pageAccessor;
    }

    /// <inheritdoc />
    public override async Task<INavigationResult> GoBackAsync(INavigationParameters parameters)
    {
        INavigationResult result;
        parameters.TryGetValue(KnownNavigationParameters.Animated, out bool? animated);
        try
        {
            switch (PopupUtilities.TopPage(PopupNavigation, _windowManager))
            {
                case PopupPage popupPage:
                    var segmentParameters = UriParsingHelper.GetSegmentParameters(null, parameters);
                    ((INavigationParametersInternal)segmentParameters).Add("__NavigationMode", NavigationMode.Back);
                    var previousPage = PopupUtilities.GetOnNavigatedToTarget(PopupNavigation, _windowManager);

                    await DoPop(popupPage.Navigation, false, animated ?? false);

                    PageUtilities.InvokeViewAndViewModelAction<IActiveAware>(popupPage, a => a.IsActive = false);
                    PageUtilities.OnNavigatedFrom(popupPage, segmentParameters);
                    PageUtilities.OnNavigatedTo(previousPage, segmentParameters);
                    PageUtilities.InvokeViewAndViewModelAction<IActiveAware>(previousPage, a => a.IsActive = true);
                    PageUtilities.DestroyPage(popupPage);
                    result = new NavigationResult();
                    break;

                default:
                    result = await base.GoBackAsync(parameters);
                    break;
            }
        }
        catch (Exception e)
        {
#if DEBUG
            System.Diagnostics.Debugger.Break();
#endif
            result = new NavigationResult { Exception = e };
        }

        return result;
    }

    /// <inheritdoc />
    protected override async Task<Page> DoPop(INavigation navigation, bool useModalNavigation, bool animated)
    {
        var page = _pageAccessor.Page;
        if (PopupNavigation.PopupStack.Count > 0 || page is PopupPage)
        {
            await PopupNavigation.PopAsync(animated);
            return null;
        }

        return await base.DoPop(navigation, useModalNavigation, animated);
    }

    /// <inheritdoc />
    protected override async Task DoPush(Page currentPage, Page page, bool? useModalNavigation, bool animated,
        bool insertBeforeLast = false, int navigationOffset = 0)
    {
        switch (page)
        {
            case PopupPage popup:
                if (_windowManager.Windows[^1].Page is null)
                {
                    throw new PopupNavigationException(popup);
                }

                await PopupNavigation.PushAsync(popup, animated);
                break;
            default:
                if (PopupNavigation.PopupStack.Any())
                {
                    foreach (var pageToPop in PopupNavigation.PopupStack)
                        PageUtilities.DestroyPage(pageToPop);

                    await PopupNavigation.PopAllAsync(animated);
                }

                if (currentPage is PopupPage)
                {
                    currentPage = PageUtilities.GetCurrentPage(_windowManager.Windows[^1].Page);
                }

                await base.DoPush(currentPage, page, useModalNavigation, animated, insertBeforeLast, navigationOffset);
                break;
        }
    }

    /// <inheritdoc />
    protected override Page GetCurrentPage()
    {
        return PopupNavigation.PopupStack.Any() ? PopupNavigation.PopupStack.LastOrDefault() : base.GetCurrentPage();
    }
}