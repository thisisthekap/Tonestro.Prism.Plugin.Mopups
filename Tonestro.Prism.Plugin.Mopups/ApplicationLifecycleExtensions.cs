using Mopups.Interfaces;
using Prism;

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
    /// <param name="app">The <see cref="PrismApplication" /></param>
    public static void PopupPluginOnResume(this IWindowManager app)
    {
        InvokeLifecyleEvent(app, x => x.OnResume());
    }

    /// <summary>
    /// Adds support for <see cref="IApplicationLifecycleAware.OnSleep" />
    /// </summary>
    /// <remarks>
    /// Do not invoke <c>base.OnResume()</c>.
    /// </remarks>
    /// <param name="app">The <see cref="PrismApplication" /></param>
    public static void PopupPluginOnSleep(this PrismApplication app)
    {
        InvokeLifecyleEvent(app, x => x.OnSleep());
    }

    private static void InvokeLifecyleEvent(IWindowManager app, Action<IApplicationLifecycleAware> action)
    {
        if (app.Windows.Last().Page != null)
        {
            var popupNavigation = ContainerLocator.Container.Resolve<IPopupNavigation>();
            var appProvider = ContainerLocator.Container.Resolve<IWindowManager>();

            var page = PopupUtilities.TopPage(popupNavigation, appProvider);
            PageUtilities.InvokeViewAndViewModelAction(page, action);
        }
    }
}