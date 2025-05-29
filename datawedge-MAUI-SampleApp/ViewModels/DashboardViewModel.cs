// Path: dawideq5/appone.mobile/AppOne.Mobile-5f3e9cf781e1f9b6f8ff8363acc50291d9330492/datawedge-MAUI-SampleApp/ViewModels/DashboardViewModel.cs
using datawedge_MAUI_SampleApp.ViewModels; // Poprawiony using dla BaseViewModel
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using datawedge_MAUI_SampleApp.Interfaces;
using datawedge_MAUI_SampleApp.Views;
using System.Threading.Tasks;

namespace datawedge_MAUI_SampleApp.ViewModels
{
    public partial class DashboardViewModel : BaseViewModel // BaseViewModel jest teraz w datawedge_MAUI_SampleApp.ViewModels
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly Interfaces.IDataWedgeService _dataWedgeService;
        private readonly INotificationService _notificationService;

        [ObservableProperty]
        string? username;

        [ObservableProperty]
        string? welcomeMessage;

        public DashboardViewModel(
            IAuthenticationService authenticationService,
            Interfaces.IDataWedgeService dataWedgeService,
            INotificationService notificationService)
        {
            _authenticationService = authenticationService;
            _dataWedgeService = dataWedgeService;
            _notificationService = notificationService;
            Title = "Panel Główny";

            Task.Run(async () => await LoadUserData());
            _dataWedgeService.InitializeDataWedge();
        }

        private async Task LoadUserData()
        {
            IsBusy = true;
            Username = await _authenticationService.GetCurrentUserAsync();
            WelcomeMessage = string.IsNullOrWhiteSpace(Username) ? "Witaj Gościu!" : $"Witaj, {Username}!";
            IsBusy = false;
        }

        [RelayCommand]
        async Task GoToScannerAsync()
        {
            await Shell.Current.GoToAsync(nameof(ScannerView));
        }

        [RelayCommand]
        async Task LogoutAsync()
        {
            bool confirm = await _notificationService.ShowConfirmation("Wyloguj", "Czy na pewno chcesz się wylogować?", "Tak", "Nie");
            if (confirm)
            {
                _authenticationService.Logout();
                await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
            }
        }

        [RelayCommand]
        void EnableScanner()
        {
            _dataWedgeService.EnableScanner();
            _notificationService.ShowNotification("Skaner", "Skaner włączony (profil).", "OK");
        }

        [RelayCommand]
        void DisableScanner()
        {
            _dataWedgeService.DisableScanner();
            _notificationService.ShowNotification("Skaner", "Skaner wyłączony (profil).", "OK");
        }

        [RelayCommand]
        void StartSoftScan()
        {
            _dataWedgeService.StartSoftScan();
            _notificationService.ShowNotification("Skaner", "Rozpoczęto skanowanie (soft trigger).", "OK");
        }

        [RelayCommand]
        void StopSoftScan()
        {
            _dataWedgeService.StopSoftScan();
        }
    }
}