// Views/DashboardView.xaml.cs
using AppOne.Mobile.ViewModels;

namespace AppOne.Mobile.Views
{
    public partial class DashboardView : ContentPage
    {
        private readonly DashboardViewModel _viewModel;
        public DashboardView(DashboardViewModel viewModel)
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
    }
}
