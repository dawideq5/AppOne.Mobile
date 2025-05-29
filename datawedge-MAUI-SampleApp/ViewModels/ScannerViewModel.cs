// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/ViewModels/ScannerViewModel.cs
using AppOne.Mobile.Messaging;
using AppOne.Mobile.Models;
using AppOne.Mobile.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using datawedge_MAUI_SampleApp.Interfaces;
using datawedge_MAUI_SampleApp.Messaging; // Poprawka CS0234: Upewnij się, że Messaging istnieje i zawiera BarcodeScannedMessage
using datawedge_MAUI_SampleApp.Models;   // Poprawka CS0234: Upewnij się, że Models istnieje i zawiera ValidationResponse
using IntelliJ.Lang.Annotations;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static Android.Icu.Text.CaseMap;

namespace datawedge_MAUI_SampleApp.ViewModels
{
    public partial class ScannerViewModel : BaseViewModel, IRecipient<BarcodeScannedMessage>
    {
        private readonly Interfaces.IDataWedgeService _dataWedgeService;
        private readonly Interfaces.IApiClient _apiClient;
        private readonly INotificationService _notificationService;

        [ObservableProperty]
        string scannedData = "Oczekiwanie na skan...";

        [ObservableProperty]
        string lastValidationResult = "Brak walidacji.";

        public ObservableCollection<string> ScanHistory { get; } = new ObservableCollection<string>();

        public ScannerViewModel(
            Interfaces.IDataWedgeService dataWedgeService,
            Interfaces.IApiClient apiClient,
            INotificationService notificationService)
        {
            _dataWedgeService = dataWedgeService;
            _apiClient = apiClient;
            _notificationService = notificationService;
            Title = "Skaner";
            WeakReferenceMessenger.Default.Register(this);
        }

        public async void Receive(BarcodeScannedMessage message)
        {
            ScannedData = $"Dane: {message.Barcode}, Typ: {message.Symbology}";
            ScanHistory.Insert(0, ScannedData);
            if (ScanHistory.Count > 20)
            {
                ScanHistory.RemoveAt(ScanHistory.Count - 1);
            }
            await ValidateBarcodeAsync(message.Barcode);
        }

        [RelayCommand]
        async Task ValidateCurrentScanAsync()
        {
            if (ScannedData.StartsWith("Dane: "))
            {
                var parts = ScannedData.Split(new[] { ", Typ: " }, System.StringSplitOptions.None);
                var barcodePart = parts[0].Replace("Dane: ", "");
                if (!string.IsNullOrWhiteSpace(barcodePart) && barcodePart != "Oczekiwanie na skan...")
                {
                    await ValidateBarcodeAsync(barcodePart);
                }
                else
                {
                    await _notificationService.ShowNotification("Walidacja", "Brak danych do walidacji.", "OK");
                }
            }
            else
            {
                await _notificationService.ShowNotification("Walidacja", "Brak danych do walidacji (zeskanuj kod).", "OK");
            }
        }

        private async Task ValidateBarcodeAsync(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                LastValidationResult = "Błąd: Pusty kod kreskowy do walidacji.";
                await _notificationService.ShowNotification("Walidacja", LastValidationResult, "OK");
                return;
            }

            IsBusy = true;
            try
            {
                ValidationResponse? validationResponse = await _apiClient.ValidateBarcodeAsync(barcode);
                if (validationResponse != null)
                {
                    LastValidationResult = $"Walidacja '{barcode}': {(validationResponse.IsValid ? "PRAWIDŁOWY" : "NIEPRAWIDŁOWY")} - {validationResponse.Message}";
                    await _notificationService.ShowNotification("Wynik Walidacji", LastValidationResult, "OK");
                }
                else
                {
                    LastValidationResult = $"Walidacja '{barcode}': Nie udało się uzyskać odpowiedzi.";
                    await _notificationService.ShowNotification("Błąd Walidacji", LastValidationResult, "OK");
                }
            }
            catch (System.Exception ex)
            {
                LastValidationResult = $"Błąd walidacji '{barcode}': {ex.Message}";
                await _notificationService.ShowNotification("Krytyczny Błąd Walidacji", LastValidationResult, "OK");
                System.Diagnostics.Debug.WriteLine($"Validation Error: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        void StartScan()
        {
            _dataWedgeService.StartSoftScan();
        }

        [RelayCommand]
        void StopScan()
        {
            _dataWedgeService.StopSoftScan();
        }

        public void Cleanup()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }
    }
}
