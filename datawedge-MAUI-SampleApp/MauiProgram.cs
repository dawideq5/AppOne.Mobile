// Lokalizacja: datawedge_MAUI_SampleApp/MauiProgram.cs
using Microsoft.Extensions.Logging;
using datawedge_MAUI_SampleApp.Services;
using datawedge_MAUI_SampleApp.ViewModels;
using datawedge_MAUI_SampleApp.Views;
using CommunityToolkit.Maui; // Dla UseMauiCommunityToolkit

namespace datawedge_MAUI_SampleApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit() // Inicjalizacja Community Toolkit
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Rejestracja usług
            builder.Services.AddSingleton<IApiClient, MockApiClient>(); // Używamy MockApiClient
            builder.Services.AddSingleton<IDataWedgeService, DataWedgeService>();


            // Rejestracja ViewModeli
            // Użyj AddTransient, jeśli ViewModel ma krótki cykl życia i nie przechowuje stanu między stronami,
            // lub AddSingleton, jeśli ma przechowywać stan lub być współdzielony.
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<ScannerViewModel>();

            // Rejestracja Widoków (stron)
            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<DashboardView>();
            builder.Services.AddTransient<ScannerView>();

            // Rejestracja głównej powłoki aplikacji
            builder.Services.AddSingleton<AppShell>();

            return builder.Build();
        }
    }
}
