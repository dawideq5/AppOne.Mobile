// Lokalizacja: datawedge_MAUI_SampleApp/Views/ScannerView.xaml.cs
using datawedge_MAUI_SampleApp.ViewModels;
// Usuniêto using datawedge_MAUI_SampleApp.Messaging i CommunityToolkit.Mvvm.Messaging,
// poniewa¿ ViewModel obs³uguje teraz odbieranie wiadomoœci.

namespace datawedge_MAUI_SampleApp.Views
{
    public partial class ScannerView : ContentPage
    {
        // ViewModel jest ju¿ wstrzykiwany przez DI i ustawiany w konstruktorze
        // private readonly ScannerViewModel _viewModel; 

        public ScannerView(ScannerViewModel viewModel)
        {
            InitializeComponent();
            // _viewModel = viewModel; // Niepotrzebne, jeœli BindingContext jest ustawiony
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // ViewModel jest ju¿ zarejestrowany na wiadomoœci w swoim konstruktorze.
            // Jeœli potrzebujesz specyficznej logiki UI przy pojawianiu siê, dodaj j¹ tutaj.
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Wywo³aj metodê Cleanup ViewModelu, aby siê wyrejestrowa³ z wiadomoœci
            if (BindingContext is ScannerViewModel vm)
            {
                vm.Cleanup();
            }
        }
    }
}
