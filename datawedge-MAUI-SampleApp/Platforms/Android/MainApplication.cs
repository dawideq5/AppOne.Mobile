// Platforms/Android/MainApplication.cs
using Android.App;
using Android.Runtime;
using System;

namespace AppOne.Mobile.Platforms.Android // Zmieniono namespace
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
