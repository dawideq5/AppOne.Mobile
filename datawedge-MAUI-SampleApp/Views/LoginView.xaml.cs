// Typ pliku: Plik C# (code-behind)
// Lokalizacja: datawedge_MAUI_SampleApp/Views/LoginView.xaml.cs
using datawedge_MAUI_SampleApp.ViewModels;

namespace datawedge_MAUI_SampleApp.Views;

public partial class LoginView : ContentPage
{
    public LoginView(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
