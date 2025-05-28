// Lokalizacja: datawedge_MAUI_SampleApp/Views/DashboardView.xaml.cs
using datawedge_MAUI_SampleApp.ViewModels;
using System.Diagnostics;

namespace datawedge_MAUI_SampleApp.Views
{
    public partial class DashboardView : ContentPage
    {
        public DashboardView(DashboardViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            Debug.WriteLine("DashboardView: Initialized.");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Debug.WriteLine("DashboardView: OnAppearing called.");
            // Usuni�to logik� czyszczenia stosu nawigacji.
            // Nawigacja absolutna z LoginViewModel powinna to obs�u�y�.
        }
        protected override bool OnBackButtonPressed()
        {
            // Zablokuj powr�t przyciskiem systemowym z DashboardView (np. do LoginView)
            // U�ytkownik powinien u�y� opcji Wyloguj.
            Debug.WriteLine("DashboardView: Back button pressed, preventing navigation.");
            return true; // True oznacza, �e obs�u�yli�my naci�ni�cie i system nie powinien nic robi�
        }
    }
}
