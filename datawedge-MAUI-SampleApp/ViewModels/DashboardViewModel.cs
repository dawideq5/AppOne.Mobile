// ViewModels/DashboardViewModel.cs
using AppOne.Mobile.Interfaces; // Dla IAuthenticationService
using AppOne.Mobile.Views;    // Dla nameof(ScannerView), nameof(LoginView)
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using datawedge_MAUI_SampleApp.Views;
using Microsoft.Maui.Controls; // Dla Shell
using System; // Dla EventArgs
using System.Threading.Tasks; // Dla Task

namespace AppOne.Mobile.ViewModels
{
    public partial class DashboardViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;

        [ObservableProperty]
        string welcomeMessage = string.Empty;

        public DashboardViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            Title = "Panel Główny";
            UpdateWelcomeMessage();
            _authenticationService.AuthenticationStateChanged += OnAuthenticationStateChanged;
        }

        private void OnAuthenticationStateChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UpdateWelcomeMessage();
                if (!_authenticationService.IsLoggedIn && Shell.Current.CurrentPage is DashboardView)
                {
                    // Jeśli użytkownik jest na dashboardzie i został wylogowany, przekieruj
                    Shell.Current.GoToAsync($"//{nameof(LoginView)}");
                }
            });
        }

        private void UpdateWelcomeMessage()
        {
            if (_authenticationService.IsLoggedIn)
            {
                WelcomeMessage = $"Witaj, {_authenticationService.UserName}!";
            }
            else
            {
                WelcomeMessage = "Witaj! Zaloguj się, aby kontynuować.";
            }
        }

        [RelayCommand]
        async Task GoToScanner()
        {
            // Nawigacja do strony skanera
            await Shell.Current.GoToAsync(nameof(ScannerView));
        }

        [RelayCommand]
        async Task Logout()
        {
            _authenticationService.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
        }

        public void OnAppearing()
        {
            UpdateWelcomeMessage(); // Upewnij się, że wiadomość jest aktualna przy wejściu
        }
    }
}
