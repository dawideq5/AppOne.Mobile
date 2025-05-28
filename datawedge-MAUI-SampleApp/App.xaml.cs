// Lokalizacja: datawedge_MAUI_SampleApp/App.xaml.cs
namespace datawedge_MAUI_SampleApp
{
    public partial class App : Application
    {
        public App(AppShell appShell) // Wstrzykujemy AppShell
        {
            InitializeComponent();

            MainPage = appShell; // Ustawiamy AppShell jako główną stronę
        }
    }
}
