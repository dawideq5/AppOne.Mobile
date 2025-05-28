// Typ pliku: Plik C# (code-behind)
// Lokalizacja: datawedge_MAUI_SampleApp/AppShell.xaml.cs
using datawedge_MAUI_SampleApp.Views;

namespace datawedge_MAUI_SampleApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Rejestracja tras dla nawigacji
        Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
        Routing.RegisterRoute(nameof(DashboardView), typeof(DashboardView));
        Routing.RegisterRoute(nameof(ScannerView), typeof(ScannerView));
    }
}
