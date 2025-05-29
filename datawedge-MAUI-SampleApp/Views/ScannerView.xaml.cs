// Views/ScannerView.xaml.cs
using AppOne.Mobile.ViewModels;

namespace AppOne.Mobile.Views
{
    public partial class ScannerView : ContentPage
    {
        private readonly ScannerViewModel _viewModel;

        public ScannerView(ScannerViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel?.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel?.OnDisappearing();
        }
    }
}
