// ViewModels/ScannerViewModel.cs
using AppOne.Mobile.Interfaces;
using AppOne.Mobile.Messaging;
using AppOne.Mobile.Models;
using AppOne.Mobile.Views; // Dla nameof(LoginView)
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics; // Dla Colors
using System;
using System.Threading.Tasks;

namespace AppOne.Mobile.ViewModels
{
    public partial class ScannerViewModel : BaseViewModel, IRecipient<BarcodeScannedMessage>
    {
        private readonly IApiClient _apiClient;
        private readonly IDataWedgeService _dataWedgeService;
        private readonly INotificationService _notificationService;
        private readonly IAuthenticationService _authenticationService;

        [ObservableProperty]
        string scanStatusMessage = "Gotowy do skanowania";

        [ObservableProperty]
        Color scanStatusColor = Colors.DodgerBlue;

        [ObservableProperty]
        string lastScannedCode = string.Empty;

        [ObservableProperty]
        string loggedInUserDisplayName = string.Empty;

        public ScannerViewModel(
            IApiClient apiClient,
            IDataWedgeService dataWedgeService,
            INotificationService notificationService,
            IAuthenticationService authenticationService)
        {
            _apiClient = apiClient;
            _dataWedgeService = dataWedgeService;
            _notificationService = notificationService;
            _authenticationService = authenticationService;

            Title = "Skaner Biletów";
            // ResetScannerStatus(); // Już zainicjowane przez ObservableProperty
            UpdateLoggedInUserDisplay();

            _authenticationService.AuthenticationStateChanged += OnAuthenticationStateChanged;
        }

        private void OnAuthenticationStateChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UpdateLoggedInUserDisplay();
                if (!_authenticationService.IsLoggedIn && Shell.Current.CurrentPage is ScannerView)
                {
                    // Jeśli użytkownik jest na stronie skanera i został wylogowany, przekieruj
                    Shell.Current.GoToAsync($"//{nameof(LoginView)}");
                }
            });
        }

        private void UpdateLoggedInUserDisplay()
        {
            LoggedInUserDisplayName = _authenticationService.IsLoggedIn ? $"Zalogowano jako: {_authenticationService.UserName}" : "Niezalogowany";
        }

        [RelayCommand]
        async Task Logout()
        {
            _authenticationService.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
        }

        public void Receive(BarcodeScannedMessage message)
        {
            MainThread.BeginInvokeOnMainThread(async () => await ProcessScannedCode(message.Barcode));
        }

        private async Task ProcessScannedCode(string barcodeData)
        {
            if (IsBusy || string.IsNullOrWhiteSpace(barcodeData))
                return;

            IsBusy = true;
            LastScannedCode = $"Zeskanowano: {barcodeData}";

            try
            {
                ValidationResponse validationResponse = await _apiClient.ValidateCodeAsync(barcodeData);

                if (validationResponse.IsValid)
                {
                    ScanStatusMessage = validationResponse.Message ?? "Kod PRAWIDŁOWY";
                    ScanStatusColor = Colors.Green;
                }
                else
                {
                    ScanStatusMessage = validationResponse.Message ?? "Kod NIEPRAWIDŁOWY";
                    ScanStatusColor = Colors.Red;
                    await _notificationService.PlayErrorSoundAsync();
                    await _notificationService.VibrateOnErrorAsync();
                }
            }
            catch (Exception ex)
            {
                ScanStatusMessage = "Błąd walidacji";
                ScanStatusColor = Colors.OrangeRed;
                LastScannedCode = $"Błąd: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Validation error: {ex}");
                await _notificationService.PlayErrorSoundAsync();
                await _notificationService.VibrateOnErrorAsync();
            }
            finally
            {
                IsBusy = false;
                // Opcjonalnie: zresetuj status po kilku sekundach
                // await Task.Delay(5000);
                // ResetScannerStatus();
            }
        }

        private void ResetScannerStatus()
        {
            ScanStatusMessage = "Gotowy do skanowania";
            ScanStatusColor = Colors.DodgerBlue;
            LastScannedCode = string.Empty;
        }

        public void OnAppearing()
        {
            WeakReferenceMessenger.Default.Register<BarcodeScannedMessage>(this);
            _dataWedgeService?.EnableScanning();
            ResetScannerStatus(); // Zresetuj status przy każdym wejściu
            UpdateLoggedInUserDisplay();
        }

        public void OnDisappearing()
        {
            _dataWedgeService?.DisableScanning();
            WeakReferenceMessenger.Default.Unregister<BarcodeScannedMessage>(this);
        }
    }
}
