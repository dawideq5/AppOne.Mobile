// Lokalizacja: datawedge_MAUI_SampleApp/Platforms/Android/MainActivity.cs
#nullable enable
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace datawedge_MAUI_SampleApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        // Poprawka dla CS8765: Dodano '?' do typu Bundle, aby dopasować go do przesłanianej metody.
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}