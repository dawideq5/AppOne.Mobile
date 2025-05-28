// Typ pliku: Plik C# (.cs)
// Lokalizacja: datawedge-MAUI-SampleApp/MauiProgram.cs

using Microsoft.Extensions.Logging; // POPRAWKA: Dodano dla AddDebug()
using datawedge_MAUI_SampleApp.Services; // Upewnij się, że ta przestrzeń nazw istnieje i zawiera IApiClient, MockApiClient
using datawedge_MAUI_SampleApp.ViewModels;
using datawedge_MAUI_SampleApp.Views;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;

namespace datawedge_MAUI_SampleApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            // POPRAWKA: Metoda AddDebug() jest metodą rozszerzenia dla ILoggingBuilder
            builder.Logging.AddDebug();
#endif

            // Rejestracja Twoich usług i API
            // Upewnij się, że IApiClient i MockApiClient są w przestrzeni nazw datawedge_MAUI_SampleApp.Services
            builder.Services.AddSingleton<IApiClient, MockApiClient>();

            // Rejestracja Twoich ViewModeli
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<DashboardViewModel>();
            builder.Services.AddSingleton<ScannerViewModel>();

            // Rejestracja Twoich Widoków (stron)
            builder.Services.AddSingleton<LoginView>();
            builder.Services.AddSingleton<DashboardView>();
            builder.Services.AddSingleton<ScannerView>();

            // Rejestracja głównej powłoki aplikacji
            builder.Services.AddSingleton<AppShell>();

            return builder.Build();
        }
    }
}
