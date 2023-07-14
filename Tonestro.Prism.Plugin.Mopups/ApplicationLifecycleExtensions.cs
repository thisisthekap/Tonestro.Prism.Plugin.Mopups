using Mopups.Interfaces;
using Prism.Common;

namespace Tonestro.Prism.Plugin.Mopups;

/// <summary>
/// Provides support extensions for <see cref="IApplicationLifecycleAware" />
/// </summary>
public static class ApplicationLifecycleExtensions
{
    /// <summary>
    /// Adds support for <see cref="IApplicationLifecycleAware.OnResume" />
    /// </summary>
    /// <remarks>
    /// Do not invoke <c>base.OnResume()</c>.
    /// </remarks>
    /// <param name="app">The <see cref="IPageAccessor" /></param>
    public static void PopupPluginOnResume(this IPageAccessor app)
    {
        InvokeLifecyleEvent(app, x => x.OnResume());
    }

    /// <summary>
    /// Adds support for <see cref="IApplicationLifecycleAware.OnSleep" />
    /// </summary>
    /// <remarks>
    /// Do not invoke <c>base.OnResume()</c>.
    /// </remarks>
    /// <param name="pageAccessor">The <see cref="IPageAccessor" /></param>
    public static void PopupPluginOnSleep(this IPageAccessor pageAccessor)
    {
        InvokeLifecyleEvent(pageAccessor, x => x.OnSleep());
    }

    private static void InvokeLifecyleEvent(IPageAccessor pageAccessor, Action<IApplicationLifecycleAware> action)
    {
        if (pageAccessor.Page == null) return;
        var popupNavigation = ContainerLocator.Container.Resolve<IPopupNavigation>();

        var page = PopupUtilities.TopPage(popupNavigation, pageAccessor);
        PageUtilities.InvokeViewAndViewModelAction(page, action);
    }
}