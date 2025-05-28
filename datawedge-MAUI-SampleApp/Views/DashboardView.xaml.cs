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
            // Usuniêto logikê czyszczenia stosu nawigacji.
            // Nawigacja absolutna z LoginViewModel powinna to obs³u¿yæ.
        }
        protected override bool OnBackButtonPressed()
        {
            // Zablokuj powrót przyciskiem systemowym z DashboardView (np. do LoginView)
            // U¿ytkownik powinien u¿yæ opcji Wyloguj.
            Debug.WriteLine("DashboardView: Back button pressed, preventing navigation.");
            return true; // True oznacza, ¿e obs³u¿yliœmy naciœniêcie i system nie powinien nic robiæ
        }
    }
}
