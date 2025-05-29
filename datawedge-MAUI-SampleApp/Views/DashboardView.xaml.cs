// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Views/DashboardView.xaml.cs
using datawedge_MAUI_SampleApp.ViewModels;

namespace datawedge_MAUI_SampleApp.Views
{
    public partial class DashboardView : ContentPage
    {
        public DashboardView(DashboardViewModel viewModel)
        {
            InitializeComponent(); // Poprawka CS0103: Upewnij siê, ¿e x:Class w DashboardView.xaml to "datawedge_MAUI_SampleApp.Views.DashboardView"
            BindingContext = viewModel;
        }
    }
}