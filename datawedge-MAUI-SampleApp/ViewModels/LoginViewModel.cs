// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/ViewModels/LoginViewModel.cs
using AppOne.Mobile.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using datawedge_MAUI_SampleApp.Interfaces;
using datawedge_MAUI_SampleApp.Views;
using IntelliJ.Lang.Annotations;
using System.Threading.Tasks;
using static Android.Icu.Text.CaseMap;

namespace datawedge_MAUI_SampleApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly INotificationService _notificationService;

        // Poprawka CS8618: Inicjalizacja pól lub oznaczenie jako nullable
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        string username = string.Empty; // Inicjalizacja

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        string password = string.Empty; // Inicjalizacja

        public LoginViewModel(IAuthenticationService authenticationService, INotificationService notificationService)
        {
            _authenticationService = authenticationService;
            _notificationService = notificationService;
            Title = "Login";
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && !IsBusy;
        }

        [RelayCommand(CanExecute = nameof(CanLogin))]
        async Task LoginAsync()
        {
            IsBusy = true;
            // Aktualizacja stanu CanExecute dla komendy
            LoginCommand.NotifyCanExecuteChanged();

            var token = await _authenticationService.LoginAsync(Username, Password);

            if (!string.IsNullOrEmpty(token))
            {
                await Shell.Current.GoToAsync($"//{nameof(DashboardView)}");
            }
            else
            {
                // Poprawka CS1061: Upewniono się, że INotificationService ma metodę ShowNotification
                await _notificationService.ShowNotification("Błąd logowania", "Nieprawidłowa nazwa użytkownika lub hasło.", "OK");
                Password = string.Empty; // Wyczyść hasło po nieudanym logowaniu
            }
            IsBusy = false;
            // Aktualizacja stanu CanExecute dla komendy
            LoginCommand.NotifyCanExecuteChanged();
        }
    }
}
