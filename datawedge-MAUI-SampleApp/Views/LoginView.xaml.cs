// Views/LoginView.xaml.cs
using AppOne.Mobile.ViewModels;

namespace AppOne.Mobile.Views
{
    public partial class LoginView : ContentPage
    {
        private readonly LoginViewModel _viewModel;
        public LoginView(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel != null)
            {
                await _viewModel.OnAppearing();
            }
        }
    }
}
