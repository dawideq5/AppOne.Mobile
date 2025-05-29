// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/AppShell.xaml.cs
using datawedge_MAUI_SampleApp.ViewModels;
using datawedge_MAUI_SampleApp.Views;
using datawedge_MAUI_SampleApp.Interfaces; // Dodano dla IAuthenticationService, jeśli jest tu potrzebne

namespace datawedge_MAUI_SampleApp // Lub datawedge_MAUI_SampleApp.Views jeśli plik tam jest
{
    public partial class AppShell : Shell
    {
        // Ostrzeżenie CS8618: Jeśli pole _authenticationService jest tu używane, zainicjuj je.
        // Zazwyczaj ViewModel (AppShellViewModel) zajmuje się logiką, w tym serwisami.
        // private readonly IAuthenticationService _authenticationService; 

        public AppShell(AppShellViewModel viewModel) // Poprawka CS0246 dla AppShellViewModel
        {
            InitializeComponent(); // Poprawka CS0103: Upewnij się, że AppShell.xaml jest poprawny i x:Class jest zgodny
            BindingContext = viewModel;

            // Rejestracja tras dla nawigacji
            // Upewnij się, że nazwy widoków (LoginView, DashboardView, ScannerView) są poprawne
            // i że odpowiadające im klasy istnieją w przestrzeni nazw datawedge_MAUI_SampleApp.Views
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(DashboardView), typeof(DashboardView));
            Routing.RegisterRoute(nameof(ScannerView), typeof(ScannerView));
        }
    }
}
