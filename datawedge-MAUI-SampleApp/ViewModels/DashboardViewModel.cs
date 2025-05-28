// Typ pliku: Plik C#
// Lokalizacja: datawedge_MAUI_SampleApp/ViewModels/DashboardViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using datawedge_MAUI_SampleApp.Views; // Potrzebne dla nameof(ScannerView)

namespace datawedge_MAUI_SampleApp.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        public DashboardViewModel()
        {
        }

        [RelayCommand]
        private async Task GoToScanner()
        {
            // Nawigacja do strony skanera
            await Shell.Current.GoToAsync(nameof(ScannerView));
        }
    }
}
