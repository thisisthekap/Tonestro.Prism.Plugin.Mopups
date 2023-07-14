namespace Tonestro.Prism.Plugin.Mopups.Samples;

public class LoadingPageViewModel : BaseViewModel, IInitialize
{
    public LoadingPageViewModel(INavigationService navigationService) : base(navigationService)
    {
    }

    public async void Initialize(INavigationParameters parameters)
    {
        await Task.Delay(TimeSpan.FromSeconds(3)).ConfigureAwait(true);
        await NavigationService.NavigateAsync(nameof(MainPage)).ConfigureAwait(true);
    }
}