// Lokalizacja: datawedge_MAUI_SampleApp/ViewModels/LoginViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using datawedge_MAUI_SampleApp.Views; // Dla nameof(DashboardView)
using System.Diagnostics;

namespace datawedge_MAUI_SampleApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        [ObservableProperty]
        private bool _isError = false;

        public LoginViewModel()
        {
            Debug.WriteLine("LoginViewModel initialized.");
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Nazwa użytkownika i hasło są wymagane.";
                IsError = true;
                Debug.WriteLine("Login attempt with empty fields.");
                return;
            }

            if (Username.Trim().ToLower() == "admin" && Password.Trim() == "admin")
            {
                IsError = false;
                ErrorMessage = string.Empty;
                Debug.WriteLine("Admin login successful. Navigating to DashboardView with absolute route.");

                // POPRAWKA: Użycie nawigacji absolutnej, aby DashboardView stał się nowym korzeniem stosu nawigacji.
                // To powinno wyczyścić poprzedni stos (z LoginView).
                await Shell.Current.GoToAsync($"//{nameof(DashboardView)}");
            }
            else
            {
                ErrorMessage = "Nieprawidłowy login lub hasło.";
                IsError = true;
                Debug.WriteLine($"Login failed for user: {Username}");
            }
        }
    }
}
