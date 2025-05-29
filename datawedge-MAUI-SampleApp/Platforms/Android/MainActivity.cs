// Platforms/Android/MainActivity.cs
using Android.App;
using Android.Content.PM;
using Android.OS;
// ScreenOffReceiver jest rejestrowany w Manifeście, więc nie trzeba go tutaj ręcznie rejestrować/wyrejestrowywać.

namespace AppOne.Mobile.Platforms.Android // Zmieniono namespace
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        // Nie ma potrzeby przechowywania referencji do ScreenOffReceiver, jeśli jest on zadeklarowany w Manifeście.
        // private ScreenOffReceiver? screenOffReceiver;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // screenOffReceiver = new ScreenOffReceiver(); // Niepotrzebne, jeśli w Manifeście
        }

        // OnResume i OnPause nie są potrzebne do zarządzania ScreenOffReceiver, jeśli jest on w Manifeście.
        /*
        protected override void OnResume()
        {
            base.OnResume();
            // RegisterReceiver(screenOffReceiver, new IntentFilter(Intent.ActionScreenOff)); // Niepotrzebne
        }

        protected override void OnPause()
        {
            base.OnPause();
            // UnregisterReceiver(screenOffReceiver); // Niepotrzebne
        }
        */
    }
}
