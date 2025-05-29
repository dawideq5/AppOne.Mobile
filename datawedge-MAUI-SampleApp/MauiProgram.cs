// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/MauiProgram.cs
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using datawedge_MAUI_SampleApp.Views;    // Poprawka CS0104: Upewnij się, że to jest jedyne źródło dla LoginView, DashboardView, ScannerView
using datawedge_MAUI_SampleApp.ViewModels;
using datawedge_MAUI_SampleApp.Services;
using datawedge_MAUI_SampleApp.Interfaces;
using System.Net.Http;
// Poprawka CS0246: Jeśli AppShell jest w głównej przestrzeni nazw
// using datawedge_MAUI_SampleApp; // Jeśli AppShell jest bezpośrednio w datawedge_MAUI_SampleApp

namespace datawedge_MAUI_SampleApp
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

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<HttpClient>();
            Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<IAuthenticationService, AuthenticationService>(builder.Services);
            Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<Interfaces.IDataWedgeService, DataWedgeService>(builder.Services);
            builder.Services.AddSingleton<INotificationService, NotificationService>();

#if DEBUG
            Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<Interfaces.IApiClient, MockApiClient>(builder.Services);
#else
            Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<Interfaces.IApiClient, ApiClient>(builder.Services);
#endif

            builder.Services.AddTransient<AppShellViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<ScannerViewModel>();

            // Poprawka CS0104: Upewnij się, że LoginView, DashboardView, ScannerView są jednoznacznie zdefiniowane
            // w datawedge_MAUI_SampleApp.Views i że ich x:Class w XAML jest poprawny.
            CommunityToolkit.Maui.ServiceCollectionExtensions.AddSingleton<LoginView, LoginViewModel>(builder.Services);
            CommunityToolkit.Maui.ServiceCollectionExtensions.AddSingleton<DashboardView, DashboardViewModel>(builder.Services);
            CommunityToolkit.Maui.ServiceCollectionExtensions.AddSingleton<ScannerView, ScannerViewModel>(builder.Services);

            // Poprawka CS0246: Upewnij się, że AppShell jest rozpoznawalny.
            // Jeśli AppShell.xaml.cs jest w głównej przestrzeni nazw:
            builder.Services.AddSingleton<AppShell>();
            // Jeśli AppShell.xaml.cs jest w datawedge_MAUI_SampleApp.Views:
            // builder.Services.AddSingleton<datawedge_MAUI_SampleApp.Views.AppShell>(); 
            // Pamiętaj, aby dostosować usingi na górze pliku, jeśli AppShell jest w Views.

            builder.Services.AddSingleton<App>();

            return builder.Build();
        }
    }
}
