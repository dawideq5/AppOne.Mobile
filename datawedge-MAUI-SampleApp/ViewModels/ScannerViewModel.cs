// Path: dawideq5/appone.mobile/AppOne.Mobile-5f3e9cf781e1f9b6f8ff8363acc50291d9330492/datawedge-MAUI-SampleApp/ViewModels/ScannerViewModel.cs
using AppOne.Mobile.Messaging;
using AppOne.Mobile.Models;
// Poprawiony using dla BaseViewModel
using datawedge_MAUI_SampleApp.ViewModels; // Jeśli BaseViewModel jest teraz tutaj
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using datawedge_MAUI_SampleApp.Interfaces;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using Microsoft.Maui.Controls; // Potrzebne dla Colors, jeśli nie są globalnie dostępne
using Microsoft.Maui.ApplicationModel; // Potrzebne dla MainThread

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

        [ObservableProperty]
        string? lastScannedCode;

        [ObservableProperty]
        string scanStatusMessage = "Gotowy do skanowania";

        [ObservableProperty]
        Color scanStatusColor = Colors.Gray;

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
            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        // Poprawiona sygnatura metody - usunięto 'async'
        public void Receive(BarcodeScannedMessage message)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                ScannedData = $"Dane: {message.Barcode}, Typ: {message.Symbology}, Czas: {message.ScanTime.ToLocalTime()}";
                LastScannedCode = $"Zeskanowano: {message.Barcode}";
                ScanStatusMessage = "Przetwarzanie...";
                ScanStatusColor = Colors.Orange;

                ScanHistory.Insert(0, ScannedData);
                if (ScanHistory.Count > 20)
                {
                    ScanHistory.RemoveAt(ScanHistory.Count - 1);
                }
                await ValidateBarcodeAsync(message.Barcode); // await jest tutaj, wewnątrz lambdy
            });
        }

        [RelayCommand]
        async Task ValidateCurrentScanAsync()
        {
            if (ScannedData.StartsWith("Dane: "))
            {
                var parts = ScannedData.Split(new[] { ", Typ: " }, StringSplitOptions.None);
                var barcodePart = parts[0].Replace("Dane: ", "").Trim();
                if (!string.IsNullOrWhiteSpace(barcodePart) && barcodePart != "Oczekiwanie na skan...")
                {
                    ScanStatusMessage = "Walidacja...";
                    ScanStatusColor = Colors.Blue;
                    await ValidateBarcodeAsync(barcodePart);
                }
                else
                {
                    await _notificationService.ShowNotification("Walidacja", "Brak danych do walidacji.", "OK");
                    ScanStatusMessage = "Brak danych do walidacji";
                    ScanStatusColor = Colors.Red;
                }
            }
            else
            {
                await _notificationService.ShowNotification("Walidacja", "Brak danych do walidacji (zeskanuj kod).", "OK");
                ScanStatusMessage = "Zeskanuj kod";
                ScanStatusColor = Colors.Red;
            }
        }

        private async Task ValidateBarcodeAsync(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                LastValidationResult = "Błąd: Pusty kod kreskowy do walidacji.";
                ScanStatusMessage = "Pusty kod kreskowy";
                ScanStatusColor = Colors.Red;
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
                    if (validationResponse.IsValid)
                    {
                        ScanStatusMessage = "Kod PRAWIDŁOWY";
                        ScanStatusColor = Colors.Green;
                    }
                    else
                    {
                        ScanStatusMessage = "Kod NIEPRAWIDŁOWY";
                        ScanStatusColor = Colors.Red;
                    }
                    await _notificationService.ShowNotification("Wynik Walidacji", LastValidationResult, "OK");
                }
                else
                {
                    LastValidationResult = $"Walidacja '{barcode}': Nie udało się uzyskać odpowiedzi.";
                    ScanStatusMessage = "Błąd odpowiedzi API";
                    ScanStatusColor = Colors.DarkRed;
                    await _notificationService.ShowNotification("Błąd Walidacji", LastValidationResult, "OK");
                }
            }
            catch (Exception ex)
            {
                LastValidationResult = $"Błąd walidacji '{barcode}': {ex.Message}";
                ScanStatusMessage = "Krytyczny błąd walidacji";
                ScanStatusColor = Colors.DarkMagenta;
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
            ScanStatusMessage = "Skanowanie aktywne...";
            ScanStatusColor = Colors.CornflowerBlue;
            LastScannedCode = string.Empty;
        }

        [RelayCommand]
        void StopScan()
        {
            _dataWedgeService.StopSoftScan();
            ScanStatusMessage = "Skanowanie zatrzymane";
            ScanStatusColor = Colors.Gray;
        }

        public void Cleanup()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
            System.Diagnostics.Debug.WriteLine("ScannerViewModel cleaned up and unregistered from WeakReferenceMessenger.");
        }
    }
}