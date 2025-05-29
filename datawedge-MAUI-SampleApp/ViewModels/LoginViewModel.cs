// Path: dawideq5/appone.mobile/AppOne.Mobile-5f3e9cf781e1f9b6f8ff8363acc50291d9330492/datawedge-MAUI-SampleApp/ViewModels/LoginViewModel.cs
using datawedge_MAUI_SampleApp.ViewModels; // Upewnij się, że BaseViewModel jest tutaj
using datawedge_MAUI_SampleApp.Interfaces;
using datawedge_MAUI_SampleApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.ComponentModel; // Potrzebne dla PropertyChangedEventArgs

namespace datawedge_MAUI_SampleApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly INotificationService _notificationService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsLoginCommandEnabled))]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        string username = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsLoginCommandEnabled))]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        string password = string.Empty;

        public bool IsLoginCommandEnabled => CanLogin();

        public LoginViewModel(IAuthenticationService authenticationService, INotificationService notificationService)
        {
            _authenticationService = authenticationService;
            _notificationService = notificationService;
            Title = "Logowanie";

            // Subskrybuj zmianę właściwości w klasie bazowej (IsBusy)
            // Należy pamiętać o odsubskrybowaniu, aby uniknąć wycieków pamięci,
            // jeśli ViewModel może żyć dłużej niż jego kontekst. W MAUI dla stron to zazwyczaj nie jest problem.
            this.PropertyChanged += LoginViewModel_PropertyChanged;
        }

        private void LoginViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsBusy))
            {
                // IsBusy się zmieniło, zaktualizuj zależne stany
                OnPropertyChanged(nameof(IsLoginCommandEnabled));
                LoginCommand?.NotifyCanExecuteChanged();
            }
        }

        private bool CanLogin()
        {
            bool canLogin = !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && !IsBusy;
            return canLogin;
        }

        // Usunięto: partial void OnIsBusyChanged(bool value) 
        // Zamiast tego używamy subskrypcji PropertyChanged

        [RelayCommand(CanExecute = nameof(CanLogin))]
        async Task LoginAsync()
        {
            IsBusy = true;

            var token = await _authenticationService.LoginAsync(Username, Password);

            if (!string.IsNullOrEmpty(token))
            {
                await Shell.Current.GoToAsync($"//{nameof(DashboardView)}");
            }
            else
            {
                await _notificationService.ShowNotification("Błąd logowania", "Nieprawidłowa nazwa użytkownika lub hasło.", "OK");
                Password = string.Empty;
            }
            IsBusy = false;
        }

        // Opcjonalnie: jeśli planujesz usuwać ten ViewModel w trakcie życia aplikacji i chcesz być bardzo ostrożny
        // public void Cleanup()
        // {
        //     this.PropertyChanged -= LoginViewModel_PropertyChanged;
        // }
    }
}