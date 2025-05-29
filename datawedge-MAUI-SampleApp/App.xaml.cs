// App.xaml.cs
using AppOne.Mobile.Interfaces; // Dla IAuthenticationService
using AppOne.Mobile.Views;    // Dla LoginView

namespace AppOne.Mobile
{
    public partial class App : Application
    {
        public App(IAuthenticationService authenticationService, AppShell appShell) // Wstrzyknij AppShell
        {
            InitializeComponent();

            // Ustaw MainPage na AppShell
            MainPage = appShell;

            // Sprawdź stan logowania przy starcie
            // Jeśli użytkownik nie jest zalogowany, a AppShell domyślnie nie przekierowuje,
            // możesz to zrobić tutaj. Jednak AppShellViewModel powinien to obsłużyć.
            if (!authenticationService.IsLoggedIn)
            {
                // Shell.Current.GoToAsync($"//{nameof(LoginView)}"); // Powinno być obsługiwane przez AppShellViewModel
            }
        }
    }
}
