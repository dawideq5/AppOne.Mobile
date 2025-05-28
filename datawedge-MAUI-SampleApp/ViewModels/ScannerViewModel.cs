// Lokalizacja: datawedge_MAUI_SampleApp/ViewModels/ScannerViewModel.cs
#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using datawedge_MAUI_SampleApp.Services;
using datawedge_MAUI_SampleApp.Models;
using datawedge_MAUI_SampleApp.Messaging;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using datawedge_MAUI_SampleApp.Views; // Dla nameof(DashboardView)

namespace datawedge_MAUI_SampleApp.ViewModels
{
    public partial class ScannerViewModel : ObservableObject, IRecipient<BarcodeScannedMessage>
    {
        private readonly IApiClient _apiClient;
        private readonly IDataWedgeService _dataWedgeService;

        [ObservableProperty]
        private string _scannedDataInfo = "Oczekiwanie na skan z urządzenia...";

        [ObservableProperty]
        private ObservableCollection<string> _scanHistory;

        [ObservableProperty]
        private bool _isProcessing = false;

        [ObservableProperty]
        private string _validationMessage = "Naciśnij przycisk skanowania na urządzeniu.";

        [ObservableProperty]
        private Color _messageBackgroundColor = Colors.Gray;

        // Ta właściwość kontroluje, czy DataWedge plugin jest aktywny.
        // Można by dodać przycisk w UI do jej przełączania, jeśli potrzebne.
        [ObservableProperty]
        private bool _isScannerIntegrationActive = true;

        public ScannerViewModel(IApiClient apiClient, IDataWedgeService dataWedgeService)
        {
            _apiClient = apiClient;
            _dataWedgeService = dataWedgeService;
            ScanHistory = new ObservableCollection<string>();
            WeakReferenceMessenger.Default.Register<BarcodeScannedMessage>(this);

            // Inicjalizacja i upewnienie się, że skaner (plugin DataWedge) jest aktywny na starcie.
            _dataWedgeService.Initialize();
            _dataWedgeService.EnableScanner(_isScannerIntegrationActive);
            Debug.WriteLine("ScannerViewModel: Initialized. DataWedge integration active: " + _isScannerIntegrationActive);
        }

        // Metoda wywoływana, gdy DWIntentReceiver otrzyma dane ze skanera fizycznego
        public void Receive(BarcodeScannedMessage message)
        {
            if (IsProcessing)
            {
                Debug.WriteLine("ScannerViewModel: Already processing a scan, ignoring new barcode message.");
                return; // Ignoruj nowe skany, jeśli poprzedni jest jeszcze przetwarzany
            }

            string barcodeData = message.Value;
            string? symbology = message.Symbology;
            Debug.WriteLine($"ScannerViewModel: Hardware scan received - Data: {barcodeData}, Symbology: {symbology ?? "N/A"}");

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                // Ustaw komunikat "Skanowanie..." lub "Przetwarzanie..." po otrzymaniu danych
                ValidationMessage = "Przetwarzanie skanu..."; // Zmieniono z "Skanowanie..."
                MessageBackgroundColor = Colors.CornflowerBlue;
                IsProcessing = true; // Zablokuj dalsze przetwarzanie do zakończenia obecnego
                // OnPropertyChanged(nameof(CanTriggerScan)); // Nie ma już przycisku CanTriggerScan

                ScannedDataInfo = $"Dane: {barcodeData}, Typ: {symbology ?? "N/A"}, Czas: {message.Timestamp:HH:mm:ss}";
                if (ScanHistory.Count > 20)
                {
                    ScanHistory.RemoveAt(ScanHistory.Count - 1);
                }
                ScanHistory.Insert(0, ScannedDataInfo);

                await ProcessScannedCodeInternal(barcodeData);
            });
        }

        private async Task ProcessScannedCodeInternal(string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                ValidationMessage = "Otrzymano pusty kod.";
                MessageBackgroundColor = Colors.OrangeRed;
                IsProcessing = false;
                return;
            }
            // Komunikat "Przetwarzanie skanu..." został już ustawiony w Receive()

            try
            {
                ValidationResponse response = await _apiClient.ValidateTicketAsync(code);
                ValidationMessage = response.Message;
                MessageBackgroundColor = response.IsSuccess ? Colors.Green : Colors.Red;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"[SCANNER ERROR] API call failed: {ex.Message}");
                ValidationMessage = "Błąd połączenia z serwerem.";
                MessageBackgroundColor = Colors.DarkRed;
            }
            finally
            {
                await Task.Delay(3000); // Czas na odczytanie komunikatu przez użytkownika
                ResetScannerStateAfterProcessing();
            }
        }

        // Usunięto TriggerScanCommand i CanTriggerScan, ponieważ przycisk został usunięty.
        // Jeśli potrzebujesz programowego wyzwalania skanu do testów, można je przywrócić.

        // Ta komenda może być nadal użyteczna, jeśli chcesz dać użytkownikowi
        // możliwość włączania/wyłączania integracji ze skanerem DataWedge w ustawieniach aplikacji.
        [RelayCommand]
        private void ToggleScannerIntegration()
        {
            IsScannerIntegrationActive = !IsScannerIntegrationActive;
            _dataWedgeService.EnableScanner(IsScannerIntegrationActive);
            ScannedDataInfo = IsScannerIntegrationActive ? "Oczekiwanie na skan z urządzenia..." : "Integracja ze skanerem wyłączona.";
            if (!IsScannerIntegrationActive)
            {
                ValidationMessage = "Skaner (integracja) wyłączony.";
                MessageBackgroundColor = Colors.DarkGray;
            }
            else
            {
                ValidationMessage = "Naciśnij przycisk skanowania na urządzeniu.";
                MessageBackgroundColor = Colors.Gray;
            }
            Debug.WriteLine($"ScannerViewModel: DataWedge integration toggled. Active: {IsScannerIntegrationActive}");
        }

        private void ResetScannerStateAfterProcessing()
        {
            ValidationMessage = IsScannerIntegrationActive ? "Naciśnij przycisk skanowania na urządzeniu." : "Skaner (integracja) wyłączony.";
            MessageBackgroundColor = Colors.Gray;
            IsProcessing = false;
        }

        [RelayCommand]
        private async Task GoToDashboard()
        {
            // Nawigacja wstecz do DashboardView
            if (Shell.Current.Navigation.NavigationStack.Count > 1)
            {
                Debug.WriteLine("ScannerViewModel: Navigating back to Dashboard (GoToAsync \"..\")");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                Debug.WriteLine("ScannerViewModel: Navigation stack is too shallow to go back. Navigating to DashboardView directly (fallback).");
                await Shell.Current.GoToAsync($"//{nameof(DashboardView)}");
            }
        }

        public void Cleanup()
        {
            WeakReferenceMessenger.Default.Unregister<BarcodeScannedMessage>(this);
            Debug.WriteLine("ScannerViewModel: Unregistered from BarcodeScannedMessage.");
        }
    }
}
