using Mopups.Interfaces;
using Mopups.Pages;
using Prism.Behaviors;
using Prism.Common;
using Tonestro.Prism.Plugin.Mopups.Extensions;

namespace Tonestro.Prism.Plugin.Mopups;

public class BackgroundPopupDismissalBehavior : BehaviorBase<PopupPage>
{
    private IPopupNavigation PopupNavigation { get; }
    private IPageAccessor PageAccessor { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="BackgroundPopupDismissalBehavior"/>
    /// </summary>
    /// <param name="popupNavigation">The <see cref="IPopupNavigation"/> instance.</param>
    /// <param name="pageAccessor">The <see cref="IPageAccessor"/> instance.</param>
    public BackgroundPopupDismissalBehavior(IPopupNavigation popupNavigation, IPageAccessor pageAccessor)
    {
        PopupNavigation = popupNavigation;
        PageAccessor = pageAccessor;
    }

    /// <inheritdoc />
    protected override void OnAttachedTo(PopupPage bindable)
    {
        base.OnAttachedTo(bindable);
        bindable.BackgroundClicked += OnBackgroundClicked;
    }

    /// <inheritdoc />
    protected override void OnDetachingFrom(PopupPage bindable)
    {
        base.OnDetachingFrom(bindable);
        bindable.BackgroundClicked -= OnBackgroundClicked;
    }

    private void OnBackgroundClicked(object sender, EventArgs e)
    {
        // If the Popup Page is not going to dismiss we don't need to do anything
        if (!AssociatedObject.CloseWhenBackgroundIsClicked) return;

        var parameters = PopupUtilities.CreateBackNavigationParameters();

        InvokePageInterfaces(AssociatedObject, parameters, false);
        InvokePageInterfaces(TopPage(), parameters, true);
    }

    private void InvokePageInterfaces(Page page, INavigationParameters parameters, bool navigatedTo)
    {
        PageUtilities.InvokeViewAndViewModelAction<INavigatedAware>(page, (view) =>
        {
            if (navigatedTo)
                view.OnNavigatedTo(parameters);
            else
                view.OnNavigatedFrom(parameters);
        });
        PageUtilities.InvokeViewAndViewModelAction<IActiveAware>(page, (view) => view.IsActive = navigatedTo);

        if (!navigatedTo)
            PageUtilities.InvokeViewAndViewModelAction<IDestructible>(AssociatedObject, (view) => view.Destroy());
    }

    private Page TopPage()
    {
        Page page;
        if (PopupNavigation.PopupStack.Any(p => p != AssociatedObject))
            page = PopupNavigation.PopupStack.LastOrDefault(p => p != AssociatedObject);
        else if (PageAccessor.Page.Navigation.ModalStack.Count > 0)
            page = PageAccessor.Page.Navigation.ModalStack.LastOrDefault();
        else
            page = PageAccessor.Page.Navigation.NavigationStack.LastOrDefault();

        page ??= PageAccessor.Page;

        return page.GetDisplayedPage();
    }
}