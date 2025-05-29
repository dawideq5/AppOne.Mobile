// AppShell.xaml.cs
using AppOne.Mobile.ViewModels;
using AppOne.Mobile.Views; // Dla rejestracji tras

namespace AppOne.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(DashboardView), typeof(DashboardView));
            Routing.RegisterRoute(nameof(ScannerView), typeof(ScannerView));
        }
    }
}
