using System.Windows.Input;

namespace Tonestro.Prism.Plugin.Mopups.Samples;

public class LoginPopupViewModel : BaseViewModel
{
    private string _username;

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    private string _password;

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public ICommand LoginCommand { get; }
    public ICommand CloseCommand { get; }

    public LoginPopupViewModel(INavigationService navigationService) : base(navigationService)
    {
        LoginCommand = new DelegateCommand(() => NavigationService.GoBackAsync(new NavigationParameters
        {
            {
                MainPageViewModel.TextResultKeyParam,
                $"You have been successfully login with username: {_username} and Password: {_password}."
            }
        }));
        CloseCommand = new DelegateCommand(() => NavigationService.GoBackAsync(new NavigationParameters
        {
            { MainPageViewModel.TextResultKeyParam, "You are closing the popup pages." }
        }));
    }
}