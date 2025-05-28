// Typ pliku: Plik C# (code-behind)
// Lokalizacja: datawedge_MAUI_SampleApp/Views/DashboardView.xaml.cs
using datawedge_MAUI_SampleApp.ViewModels; // Za�o�enie, �e stworzymy DashboardViewModel

namespace datawedge_MAUI_SampleApp.Views;

public partial class DashboardView : ContentPage
{
    public DashboardView(DashboardViewModel viewModel) // Za�o�enie, �e stworzymy DashboardViewModel
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
