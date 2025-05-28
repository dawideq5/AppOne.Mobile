// Typ pliku: Plik C#
// Lokalizacja: datawedge_MAUI_SampleApp/ViewModels/ScannerViewModel.cs

#nullable enable // Włączenie kontekstu nullable

// POPRAWKI: Upewnij się, że te przestrzenie nazw są poprawne
using datawedge_MAUI_SampleApp.Services;
using datawedge_MAUI_SampleApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics; // Dla Color i Colors
using System.Diagnostics;
using datawedge_MAUI_SampleApp.Views;

namespace datawedge_MAUI_SampleApp.ViewModels
{
    public partial class ScannerViewModel : ObservableObject
    {
        // POPRAWKA: Upewnij się, że IApiClient jest zdefiniowany w datawedge_MAUI_SampleApp.Services
        private readonly IApiClient _apiClient;

        [ObservableProperty]
        private bool _isProcessing = false;

        [ObservableProperty]
        private string _validationMessage = "Skieruj skaner na kod lub użyj przycisku";

        [ObservableProperty]
        private Color _messageBackgroundColor = Colors.Gray; // Colors pochodzi z Microsoft.Maui.Graphics

        public ScannerViewModel(IApiClient apiClient) // Upewnij się, że IApiClient jest rozpoznawalne
        {
            _apiClient = apiClient;
        }

        [RelayCommand(CanExecute = nameof(CanProcess))]
        private async Task ProcessScannedCode(string? code) // Dodano ? dla nullable
        {
            if (string.IsNullOrWhiteSpace(code) || IsProcessing) return;

            IsProcessing = true;
            ValidationMessage = "Przetwarzanie...";
            MessageBackgroundColor = Colors.Orange;

            try
            {
                // POPRAWKA: Upewnij się, że ValidationResponse jest zdefiniowane w datawedge_MAUI_SampleApp.Models
                ValidationResponse response = await _apiClient.ValidateTicketAsync(code);
                ValidationMessage = response.Message;
                MessageBackgroundColor = response.IsSuccess ? Colors.Green : Colors.Red;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"[SCANNER ERROR] {ex.Message}");
                ValidationMessage = "Błąd połączenia z serwerem";
                MessageBackgroundColor = Colors.DarkRed;
            }
            finally
            {
                await Task.Delay(3000);
                ResetScannerState();
            }
        }

        private bool CanProcess() => !IsProcessing;

        private void ResetScannerState()
        {
            ValidationMessage = "Gotowy do skanowania";
            MessageBackgroundColor = Colors.Gray;
            IsProcessing = false;
        }

        public void Reset() => ResetScannerState();

        [RelayCommand]
        private async Task GoToDashboard()
        {
            await Shell.Current.GoToAsync($"//{nameof(DashboardView)}");
        }
    }
}
