// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Views/LoginView.xaml.cs
using datawedge_MAUI_SampleApp.ViewModels;

namespace datawedge_MAUI_SampleApp.Views
{
    public partial class LoginView : ContentPage
    {
        public LoginView(LoginViewModel viewModel)
        {
            InitializeComponent(); // Poprawka CS0103: Upewnij siê, ¿e x:Class w LoginView.xaml to "datawedge_MAUI_SampleApp.Views.LoginView"
            BindingContext = viewModel;
        }
    }
}
