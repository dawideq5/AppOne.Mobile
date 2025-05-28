// Typ pliku: Plik C# (code-behind)
// Lokalizacja: datawedge_MAUI_SampleApp/Views/DashboardView.xaml.cs
using datawedge_MAUI_SampleApp.ViewModels; // Za³o¿enie, ¿e stworzymy DashboardViewModel

namespace datawedge_MAUI_SampleApp.Views;

public partial class DashboardView : ContentPage
{
    public DashboardView(DashboardViewModel viewModel) // Za³o¿enie, ¿e stworzymy DashboardViewModel
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
