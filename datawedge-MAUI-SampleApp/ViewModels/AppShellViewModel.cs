// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/ViewModels/AppShellViewModel.cs
using AppOne.Mobile.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using datawedge_MAUI_SampleApp.Interfaces;
using datawedge_MAUI_SampleApp.Views; // Potrzebne dla nameof(LoginView)
using System.Threading.Tasks;

namespace datawedge_MAUI_SampleApp.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly INotificationService _notificationService; // Dodano dla potwierdzenia wylogowania

        [ObservableProperty]
        bool isLoggedIn;

        public AppShellViewModel(IAuthenticationService authenticationService, INotificationService notificationService)
        {
            _authenticationService = authenticationService;
            _notificationService = notificationService; // Przypisanie serwisu

            // Sprawdź stan logowania asynchronicznie przy inicjalizacji
            Task.Run(async () => await CheckLoginState());
            // Subskrypcja na zmiany stanu uwierzytelnienia, jeśli AuthenticationService emituje zdarzenia
        }

        public async Task CheckLoginState()
        {
            IsLoggedIn = await _authenticationService.IsUserAuthenticatedAsync();
            if (!IsLoggedIn)
            {
                // Jeśli użytkownik nie jest zalogowany przy starcie, przejdź do LoginView
                // Upewnij się, że to nie powoduje pętli nawigacji lub problemów przy pierwszym uruchomieniu
                await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
            }
        }

        [RelayCommand]
        async Task Logout()
        {
            bool confirm = await _notificationService.ShowConfirmation("Wyloguj", "Czy na pewno chcesz się wylogować?", "Tak", "Nie");
            if (!confirm) return;

            _authenticationService.Logout();
            IsLoggedIn = false;
            // Poprawka CS0104: Użycie nameof(LoginView) powinno być jednoznaczne, jeśli LoginView jest poprawnie zdefiniowane
            await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
        }
    }
}
