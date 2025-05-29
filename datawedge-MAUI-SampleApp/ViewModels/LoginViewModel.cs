// ViewModels/LoginViewModel.cs
using AppOne.Mobile.Interfaces;
using AppOne.Mobile.Views; // Dla nameof(DashboardView)
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using datawedge_MAUI_SampleApp.Views;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace AppOne.Mobile.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IApiClient _apiClient; // Dodajemy IApiClient, jeśli walidacja jest tu

        [ObservableProperty]
        string username = string.Empty;

        [ObservableProperty]
        string password = string.Empty;

        public LoginViewModel(IAuthenticationService authenticationService, IApiClient apiClient)
        {
            _authenticationService = authenticationService;
            _apiClient = apiClient; // Przypisz IApiClient
            Title = "Logowanie";
            LoadLastUserCommand = new AsyncRelayCommand(LoadLastUserAsync);
        }

        public IAsyncRelayCommand LoadLastUserCommand { get; }

        private async Task LoadLastUserAsync()
        {
            Username = await _authenticationService.GetLastLoggedInUserAsync() ?? string.Empty;
        }

        [RelayCommand]
        async Task Login()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    await Shell.Current.DisplayAlert("Błąd logowania", "Nazwa użytkownika i hasło nie mogą być puste.", "OK");
                    return;
                }

                // Użyj AuthenticationService do logowania
                bool success = await _authenticationService.LoginAsync(Username, Password);

                if (success)
                {
                    // Nawigacja do DashboardView
                    await Shell.Current.GoToAsync($"//{nameof(DashboardView)}");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Błąd logowania", "Nieprawidłowa nazwa użytkownika lub hasło.", "OK");
                }
            }
            catch (System.Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", $"Wystąpił błąd podczas logowania: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Login failed: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task OnAppearing()
        {
            await LoadLastUserAsync();
            // Resetuj hasło przy każdym pojawieniu się widoku
            Password = string.Empty;
        }
    }
}
