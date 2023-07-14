using System.Windows.Input;

namespace Tonestro.Prism.Plugin.Mopups.Samples;

public class MainPageViewModel : BaseViewModel, INavigationAware
{
    public const string TextResultKeyParam = "TextResultKeyParam";

    private string _text;

    public string Text
    {
        get => _text;
        private set => SetProperty(ref _text, value);
    }

    public ICommand ClickCommand { get; }

    public MainPageViewModel(INavigationService navigationService) : base(navigationService)
    {
        ClickCommand = new DelegateCommand(() => NavigationService.NavigateAsync(nameof(LoginPopup)));
        Text = "Welcome to MAUI";
    }

    public void OnNavigatedFrom(INavigationParameters parameters)
    {
    }

    public void OnNavigatedTo(INavigationParameters parameters)
    {
        if (parameters.TryGetValue(TextResultKeyParam, out string text))
        {
            Text = text;
        }
    }
}