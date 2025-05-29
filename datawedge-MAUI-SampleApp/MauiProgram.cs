// MauiProgram.cs
using AppOne.Mobile.Interfaces;
using AppOne.Mobile.Services;
using AppOne.Mobile.ViewModels;
using AppOne.Mobile.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging; // Dla AddDebug
using Plugin.Maui.Audio;

namespace AppOne.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Rejestracja serwisów
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<IDataWedgeService, DataWedgeService>();
            // Wybierz ApiClient lub MockApiClient
            builder.Services.AddSingleton<IApiClient, ApiClient>(); // Rzeczywiste API
            // builder.Services.AddSingleton<IApiClient, MockApiClient>(); // Mock API do testów
            builder.Services.AddSingleton<INotificationService, NotificationService>();
            builder.Services.AddSingleton(AudioManager.Current);

            // Rejestracja ViewModeli
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<ScannerViewModel>();
            builder.Services.AddSingleton<AppShellViewModel>();

            // Rejestracja Widoków (Stron)
            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<DashboardView>();
            builder.Services.AddTransient<ScannerView>();

            // Rejestracja App i AppShell
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<App>(); // Dodaj rejestrację App

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
