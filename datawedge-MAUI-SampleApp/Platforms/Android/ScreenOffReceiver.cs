// Platforms/Android/ScreenOffReceiver.cs
using Android.App;
using Android.Content;
using AppOne.Mobile.Interfaces;
using AppOne.Mobile.Views; // Dla nameof(LoginView)
using Microsoft.Maui.ApplicationModel;

namespace AppOne.Mobile.Platforms.Android // Zmieniono namespace
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    // IntentFilter jest już zdefiniowany w AndroidManifest.xml, więc tutaj nie jest konieczny,
    // ale dla jasności można go zostawić. Jeśli jest w Manifeście, ten atrybut może być zbędny.
    // [IntentFilter(new[] { Intent.ActionScreenOff })] 
    public class ScreenOffReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            if (intent?.Action == Intent.ActionScreenOff)
            {
                System.Diagnostics.Debug.WriteLine("Screen OFF detected on Android.");

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        var authService = IPlatformApplication.Current?.Services?.GetService<IAuthenticationService>();
                        if (authService != null && authService.IsLoggedIn)
                        {
                            authService.Logout();
                            System.Diagnostics.Debug.WriteLine("User logged out due to screen off.");

                            if (Shell.Current != null &&
                                Shell.Current.CurrentState != null &&
                                !Shell.Current.CurrentState.Location.OriginalString.Contains(nameof(LoginView)))
                            {
                                await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error during screen off logout: {ex.Message}");
                    }
                });
            }
        }
    }
}
