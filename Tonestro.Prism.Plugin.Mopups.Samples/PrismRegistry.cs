namespace Tonestro.Prism.Plugin.Mopups.Samples;

public class PrismRegistry
{
    public static void Configure(PrismAppBuilder builder)
    {
        builder.RegisterTypes(Register)
            .OnInitialized(OnInitialized)
            .OnAppStart(nameof(LoadingPage));
    }

    private static void OnInitialized(IContainerProvider obj)
    {
    }

    private static void Register(IContainerRegistry containerRegistry)
    {
        RegisterPages(containerRegistry);
        RegisterServices(containerRegistry);
    }

    private static void RegisterPages(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<LoadingPage>()
            .RegisterForNavigation<MainPage>()
            .RegisterForNavigation<LoginPopup>();
    }

    private static void RegisterServices(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterPopupNavigationService();
    }
}