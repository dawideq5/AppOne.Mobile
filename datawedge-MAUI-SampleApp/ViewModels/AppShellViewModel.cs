// ViewModels/AppShellViewModel.cs
using AppOne.Mobile.Interfaces;
using AppOne.Mobile.Views; // Dla nameof(LoginView)
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace AppOne.Mobile.ViewModels
{
    public partial class AppShellViewModel : ObservableObject
    {
        private readonly IAuthenticationService _authenticationService;

        [ObservableProperty]
        string loggedInUserDisplayName = string.Empty;

        [ObservableProperty]
        bool isLoggedIn;

        public AppShellViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _authenticationService.AuthenticationStateChanged += OnAuthenticationStateChanged;
            UpdateAuthenticationState();
        }

        private void OnAuthenticationStateChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(UpdateAuthenticationState);
        }

        private void UpdateAuthenticationState()
        {
            IsLoggedIn = _authenticationService.IsLoggedIn;
            LoggedInUserDisplayName = _authenticationService.IsLoggedIn && !string.IsNullOrEmpty(_authenticationService.UserName)
                ? $"Zalogowano jako: {_authenticationService.UserName}"
                : string.Empty;

            // Jeśli użytkownik nie jest zalogowany, a nie jest na stronie logowania, przekieruj
            // Sprawdź, czy Shell.Current i CurrentState są dostępne
            if (!IsLoggedIn &&
                Shell.Current != null &&
                Shell.Current.CurrentState != null &&
                !Shell.Current.CurrentState.Location.OriginalString.Contains(nameof(LoginView), StringComparison.OrdinalIgnoreCase))
            {
                // Shell.Current.GoToAsync($"//{nameof(LoginView)}"); // Rozważ, czy to jest zawsze pożądane
            }
        }

        [RelayCommand]
        async Task Logout()
        {
            _authenticationService.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
        }
    }
}
