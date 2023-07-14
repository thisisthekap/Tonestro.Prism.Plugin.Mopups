namespace Tonestro.Prism.Plugin.Mopups.Samples;

public class BaseViewModel : BindableBase
{
    protected INavigationService NavigationService { get; }

    public BaseViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
    }
}