using Mopups.Interfaces;
using Mopups.Pages;
using NavigationMode = Prism.Navigation.NavigationMode;

namespace Tonestro.Prism.Plugin.Mopups;

/// <summary>
/// Extensions to better handle Popup Navigation.
/// </summary>
public static partial class PopupExtensions
{
    private static IPopupNavigation SPopupNavigation => ContainerLocator.Container.Resolve<IPopupNavigation>();

    private static IReadOnlyList<PopupPage> SPopupStack => SPopupNavigation.PopupStack;

    /// <summary>
    /// Clears the Popup Navigation Stack.
    /// </summary>
    /// <param name="navigationService">The <see cref="INavigationService" />.</param>
    /// <param name="key">A single NavigationParameter key.</param>
    /// <param name="param">A single NavigationParameter value.</param>
    /// <param name="animated">A flag to indicate whether the Navigation should be animated.</param>
    /// <returns>The <see cref="INavigationResult" />.</returns>
    public static Task<INavigationResult> ClearPopupStackAsync(this INavigationService navigationService, string key,
        object param, bool animated = true) =>
        navigationService.ClearPopupStackAsync(GetNavigationParameters(key, param, NavigationMode.Back), animated);

    /// <summary>
    /// Clears the Popup Navigation Stack.
    /// </summary>
    /// <param name="navigationService">The <see cref="INavigationService" />.</param>
    /// <param name="parameters">The <see cref="INavigationParameters" />.</param>
    /// <param name="animated">A flag to indicate whether the Navigation should be animated.</param>
    /// <returns>The <see cref="INavigationResult" />.</returns>
    public static async Task<INavigationResult> ClearPopupStackAsync(this INavigationService navigationService,
        INavigationParameters parameters = null, bool animated = true)
    {
        parameters ??= new NavigationParameters();
        parameters.Add(KnownNavigationParameters.Animated, animated);
        while (SPopupStack.Any())
        {
            var result = await navigationService.GoBackAsync(parameters);
            if (result.Exception != null)
                return result;
        }

        return new NavigationResult();
    }

    private static INavigationParameters GetNavigationParameters(string key, object param, NavigationMode mode) =>
        new NavigationParameters()
        {
            { key, param }
        }.AddNavigationMode(mode);
}