// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/ViewModels/DashboardViewModel.cs
using AppOne.Mobile.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using datawedge_MAUI_SampleApp.Interfaces;
using datawedge_MAUI_SampleApp.Views; // Poprawka CS0104: Upewnij się, że to jest jedyne źródło dla ScannerView i LoginView
using System.Threading.Tasks;
using static Android.Icu.Text.CaseMap;

namespace datawedge_MAUI_SampleApp.ViewModels
{
    public partial class DashboardViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly Interfaces.IDataWedgeService _dataWedgeService;
        private readonly INotificationService _notificationService;

        [ObservableProperty]
        string username = string.Empty;

        public DashboardViewModel(
            IAuthenticationService authenticationService,
            Interfaces.IDataWedgeService dataWedgeService,
            INotificationService notificationService)
        {
            _authenticationService = authenticationService;
            _dataWedgeService = dataWedgeService;
            _notificationService = notificationService;
            Title = "Dashboard";

            Task.Run(async () => Username = await _authenticationService.GetCurrentUserAsync() ?? "Gość");
            _dataWedgeService.InitializeDataWedge();
        }

        [RelayCommand]
        async Task GoToScannerAsync()
        {
            // Poprawka CS0104: Upewnij się, że ScannerView jest jednoznacznie zdefiniowane
            await Shell.Current.GoToAsync(nameof(ScannerView));
        }

        [RelayCommand]
        async Task LogoutAsync()
        {
            bool confirm = await _notificationService.ShowConfirmation("Wyloguj", "Czy na pewno chcesz się wylogować?", "Tak", "Nie");
            if (confirm)
            {
                _authenticationService.Logout();
                // Poprawka CS0104: Upewnij się, że LoginView jest jednoznacznie zdefiniowane
                await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
            }
        }

        [RelayCommand]
        void EnableScanner()
        {
            _dataWedgeService.EnableScanner();
            _notificationService.ShowNotification("Skaner", "Skaner włączony.", "OK");
        }

        [RelayCommand]
        void DisableScanner()
        {
            _dataWedgeService.DisableScanner();
            _notificationService.ShowNotification("Skaner", "Skaner wyłączony.", "OK");
        }

        [RelayCommand]
        void StartSoftScan()
        {
            _dataWedgeService.StartSoftScan();
        }

        [RelayCommand]
        void StopSoftScan()
        {
            _dataWedgeService.StopSoftScan();
        }
    }
}
