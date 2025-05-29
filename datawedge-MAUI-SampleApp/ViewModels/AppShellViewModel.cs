// Path: dawideq5/appone.mobile/AppOne.Mobile-5f3e9cf781e1f9b6f8ff8363acc50291d9330492/datawedge-MAUI-SampleApp/ViewModels/AppShellViewModel.cs
using datawedge_MAUI_SampleApp.ViewModels; // Poprawiony using dla BaseViewModel
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using datawedge_MAUI_SampleApp.Interfaces;
using datawedge_MAUI_SampleApp.Views;
using System.Threading.Tasks;

namespace datawedge_MAUI_SampleApp.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel // BaseViewModel jest teraz w datawedge_MAUI_SampleApp.ViewModels
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly INotificationService _notificationService;

        [ObservableProperty]
        bool isLoggedIn;

        [ObservableProperty]
        string? loggedInUserDisplayName;

        public AppShellViewModel(IAuthenticationService authenticationService, INotificationService notificationService)
        {
            _authenticationService = authenticationService;
            _notificationService = notificationService;
            IsBusy = true;
            Task.Run(async () => await CheckLoginState());
        }

        public async Task CheckLoginState()
        {
            IsLoggedIn = await _authenticationService.IsUserAuthenticatedAsync();
            if (IsLoggedIn)
            {
                LoggedInUserDisplayName = await _authenticationService.GetCurrentUserAsync();
            }
            else
            {
                LoggedInUserDisplayName = null;
                await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
            }
            IsBusy = false;
        }

        [RelayCommand]
        async Task Logout()
        {
            bool confirm = await _notificationService.ShowConfirmation("Wyloguj", "Czy na pewno chcesz się wylogować?", "Tak", "Nie");
            if (!confirm) return;

            _authenticationService.Logout();
            IsLoggedIn = false;
            LoggedInUserDisplayName = null;
            await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
        }
    }
}