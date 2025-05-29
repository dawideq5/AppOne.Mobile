// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/App.xaml.cs
using datawedge_MAUI_SampleApp.Interfaces;
// Poprawka CS0246: Dodano using dla AppShell. Zakładam, że AppShell.xaml.cs jest w głównej przestrzeni nazw projektu lub w Views.
// Jeśli AppShell jest w Views, użyj: using datawedge_MAUI_SampleApp.Views;
// Jeśli AppShell jest w głównej przestrzeni nazw, ten using może nie być potrzebny, jeśli App.xaml.cs i AppShell.xaml.cs są w tej samej przestrzeni.
// Dla bezpieczeństwa, dodajmy using dla głównej przestrzeni, gdzie AppShell prawdopodobnie się znajduje.
// using datawedge_MAUI_SampleApp; // Jeśli AppShell jest bezpośrednio w datawedge_MAUI_SampleApp
// lub
using datawedge_MAUI_SampleApp.Views; // Jeśli AppShell jest w datawedge_MAUI_SampleApp.Views

namespace datawedge_MAUI_SampleApp
{
    public partial class App : Application
    {
        // Poprawka CS0102: Usunięto zduplikowane pole _authenticationService, jeśli AppShellViewModel nim zarządza.
        // Jeśli to pole jest potrzebne tutaj, upewnij się, że jest tylko jedna definicja.
        // Na razie zakładam, że AppShell (przez swój ViewModel) zarządza logiką uwierzytelniania.

        // Poprawka CS0111 i CS0246: Upewnij się, że jest tylko jeden konstruktor App
        // i że typ AppShell jest rozpoznawany.
        public App(AppShell appShell) // Wstrzykujemy gotowy AppShell
        {
            InitializeComponent(); // Poprawka CS0103: Upewnij się, że App.xaml jest poprawny i x:Class="datawedge_MAUI_SampleApp.App"
            MainPage = appShell;
        }

        // Jeśli potrzebujesz _authenticationService w App.xaml.cs, możesz je wstrzyknąć tutaj:
        // private readonly IAuthenticationService _authenticationService;
        // public App(AppShell appShell, IAuthenticationService authenticationService)
        // {
        //     InitializeComponent();
        //     _authenticationService = authenticationService; // Poprawka CS8618 - inicjalizacja
        //     MainPage = appShell;
        // }

        // Ostrzeżenie CS8618: Jeśli pole _authenticationService jest zadeklarowane, musi być zainicjowane.
        // Jeśli nie jest używane bezpośrednio w tej klasie (np. logika OnStart została przeniesiona), można je usunąć.
    }
}