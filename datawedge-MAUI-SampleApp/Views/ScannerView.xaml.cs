// Lokalizacja: datawedge_MAUI_SampleApp/Views/ScannerView.xaml.cs
using datawedge_MAUI_SampleApp.ViewModels;
// Usuni�to using datawedge_MAUI_SampleApp.Messaging i CommunityToolkit.Mvvm.Messaging,
// poniewa� ViewModel obs�uguje teraz odbieranie wiadomo�ci.

namespace datawedge_MAUI_SampleApp.Views
{
    public partial class ScannerView : ContentPage
    {
        // ViewModel jest ju� wstrzykiwany przez DI i ustawiany w konstruktorze
        // private readonly ScannerViewModel _viewModel; 

        public ScannerView(ScannerViewModel viewModel)
        {
            InitializeComponent();
            // _viewModel = viewModel; // Niepotrzebne, je�li BindingContext jest ustawiony
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // ViewModel jest ju� zarejestrowany na wiadomo�ci w swoim konstruktorze.
            // Je�li potrzebujesz specyficznej logiki UI przy pojawianiu si�, dodaj j� tutaj.
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Wywo�aj metod� Cleanup ViewModelu, aby si� wyrejestrowa� z wiadomo�ci
            if (BindingContext is ScannerViewModel vm)
            {
                vm.Cleanup();
            }
        }
    }
}
