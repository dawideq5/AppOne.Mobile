// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Views/ScannerView.xaml.cs
using datawedge_MAUI_SampleApp.ViewModels;

namespace datawedge_MAUI_SampleApp.Views
{
    public partial class ScannerView : ContentPage
    {
        public ScannerView(ScannerViewModel viewModel)
        {
            InitializeComponent(); // Poprawka CS0103: Upewnij siê, ¿e x:Class w ScannerView.xaml to "datawedge_MAUI_SampleApp.Views.ScannerView"
            BindingContext = viewModel;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is ScannerViewModel vm)
            {
                vm.Cleanup();
            }
        }
    }
}