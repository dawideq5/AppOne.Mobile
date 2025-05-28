// Typ pliku: Plik C# (.cs)
// Lokalizacja: datawedge_MAUI_SampleApp/ViewModels/LoginViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using datawedge_MAUI_SampleApp.Views; // Dodajemy using dla DashboardView

namespace datawedge_MAUI_SampleApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isError = false;

        [RelayCommand]
        private async Task Login()
        {
            if (Username == "test" && Password == "test")
            {
                IsError = false;
                ErrorMessage = string.Empty;

                // Nawigacja do DashboardView po pomyślnym zalogowaniu
                // Używamy /// aby zresetować stos nawigacji
                await Shell.Current.GoToAsync($"//{nameof(DashboardView)}");
            }
            else
            {
                ErrorMessage = "Nieprawidłowy login lub hasło.";
                IsError = true;
            }
        }
    }
}
