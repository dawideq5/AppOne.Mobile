// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Platforms/Android/ScreenOffReceiver.cs
using Android.App;
using Android.Content;
// using Android.OS; // Nieużywane?
using Android.Util;
using datawedge_MAUI_SampleApp.Interfaces;
using Microsoft.Maui.Controls.PlatformConfiguration; // Dla IPlatformApplication
using Microsoft.Maui.Platform; // Dla GetMauiContext


namespace datawedge_MAUI_SampleApp.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = false)] // Exported = false dla bezpieczeństwa, jeśli nie jest potrzebny z zewnątrz
    [IntentFilter(new[] { Intent.ActionScreenOff })]
    public class ScreenOffReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            if (intent?.Action == Intent.ActionScreenOff)
            {
                Log.Info("ScreenOffReceiver", "Screen off event detected.");

                // Poprawka CS0618: Użycie IPlatformApplication.Current.Services
                var authService = GetAuthenticationService(context);

                if (authService != null)
                {
                    // Uruchomienie logiki w tle, aby uniknąć blokowania OnReceive
                    Task.Run(async () =>
                    {
                        if (await authService.IsUserAuthenticatedAsync())
                        {
                            Log.Info("ScreenOffReceiver", "User is authenticated. Performing screen off actions.");
                            // authService.Logout(); // Rozważ, czy to jest pożądane zachowanie
                            // Log.Info("ScreenOffReceiver", "User logged out due to screen off.");
                        }
                        else
                        {
                            Log.Info("ScreenOffReceiver", "User is not authenticated.");
                        }
                    });
                }
                else
                {
                    Log.Warn("ScreenOffReceiver", "AuthenticationService not available.");
                }
            }
        }

        private IAuthenticationService? GetAuthenticationService(Context? context)
        {
            try
            {
                // Próba uzyskania dostępu do serwisów MAUI. Może być zawodne w BroadcastReceiver.
                // Alternatywnie, można przekazać zależności w inny sposób lub użyć statycznego dostępu,
                // jeśli jest to absolutnie konieczne i bezpieczne.
                if (IPlatformApplication.Current?.Services != null)
                {
                    return IPlatformApplication.Current.Services.GetService<IAuthenticationService>();
                }

                // Jeśli powyższe zawiedzie, a context jest dostępny (np. z Activity), można spróbować:
                // var mauiContext = context?.GetMauiContext();
                // return mauiContext?.Services.GetService<IAuthenticationService>();

                Log.Warn("ScreenOffReceiver", "Could not access MAUI services via IPlatformApplication.Current.Services.");
            }
            catch (Exception ex)
            {
                Log.Error("ScreenOffReceiver", $"Error getting AuthenticationService: {ex.Message}");
            }
            return null;
        }
    }
}
