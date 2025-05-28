// Typ pliku: Plik C# (code-behind)
// Lokalizacja: datawedge_MAUI_SampleApp/App.xaml.cs

// Upewnij się, że przestrzeń nazw jest poprawna
namespace datawedge_MAUI_SampleApp;

public partial class App : Application
{
    public App(AppShell appShell) // Wstrzykujemy AppShell
    {
        InitializeComponent();

        MainPage = appShell; // Ustawiamy AppShell jako główną stronę
    }
}
