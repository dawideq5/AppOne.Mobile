// Lokalizacja: datawedge_MAUI_SampleApp/ViewModels/DashboardViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using datawedge_MAUI_SampleApp.Views; // Dla nameof(ScannerView) i nameof(LoginView)

namespace datawedge_MAUI_SampleApp.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        public DashboardViewModel()
        {
            // Inicjalizacja, jeśli potrzebna
        }

        [RelayCommand]
        private async Task GoToScanner()
        {
            // Nawigacja do strony skanera
            await Shell.Current.GoToAsync(nameof(ScannerView));
        }

        [RelayCommand]
        private async Task Logout()
        {
            // Logika wylogowania (np. czyszczenie tokenów, stanu użytkownika)
            // ...

            // Nawigacja z powrotem do strony logowania
            // Użycie "//" resetuje stos nawigacji
            await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
        }
    }
}
