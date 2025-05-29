// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Platforms/Android/MainApplication.cs
using Android.App;
using Android.Runtime;

namespace datawedge_MAUI_SampleApp.Platforms.Android // Upewnij się, że przestrzeń nazw jest spójna
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => datawedge_MAUI_SampleApp.MauiProgram.CreateMauiApp(); // Poprawka CS0103: Pełna kwalifikacja do MauiProgram
    }
}
