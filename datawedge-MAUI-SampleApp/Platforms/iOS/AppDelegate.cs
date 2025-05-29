// Platforms/iOS/AppDelegate.cs
using Foundation;
using UIKit;
using AppOne.Mobile.Interfaces;
using AppOne.Mobile.Views; // Dla nameof(LoginView)
using Microsoft.Maui.ApplicationModel;

namespace AppOne.Mobile.Platforms.iOS // Zmieniono namespace
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        private NSObject? protectedDataWillBecomeUnavailableObserver;
        private NSObject? appWillResignActiveObserver; // Dla iOS 13+ (blokada ekranu)
        private NSObject? appDidEnterBackgroundObserver; // Ogólne przejście w tło

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var result = base.FinishedLaunching(application, launchOptions);

            // Dla starszych wersji iOS lub jako fallback
            protectedDataWillBecomeUnavailableObserver = NSNotificationCenter.DefaultCenter.AddObserver(
                UIApplication.ProtectedDataWillBecomeUnavailableNotification,
                HandleScreenLockEvent);

            // Dla iOS 13+ bardziej niezawodne może być UIApplicationWillResignActiveNotification
            // gdy aplikacja traci fokus (co obejmuje blokadę ekranu)
            appWillResignActiveObserver = NSNotificationCenter.DefaultCenter.AddObserver(
                UIApplication.WillResignActiveNotification,
                HandleScreenLockEvent);

            // Można też rozważyć DidEnterBackgroundNotification, jeśli wylogowanie ma nastąpić
            // także przy przejściu aplikacji w tło z innych powodów niż blokada ekranu.
            appDidEnterBackgroundObserver = NSNotificationCenter.DefaultCenter.AddObserver(
                UIApplication.DidEnterBackgroundNotification,
                HandleScreenLockEvent);


            return result;
        }

        private void HandleScreenLockEvent(NSNotification notification)
        {
            System.Diagnostics.Debug.WriteLine($"iOS App event: {notification.Name}. Performing logout if needed.");

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var authService = IPlatformApplication.Current?.Services?.GetService<IAuthenticationService>();
                    if (authService != null && authService.IsLoggedIn)
                    {
                        authService.Logout();
                        System.Diagnostics.Debug.WriteLine("User logged out due to iOS app state change (e.g., screen lock/background).");

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
                    System.Diagnostics.Debug.WriteLine($"Error during iOS app state change logout: {ex.Message}");
                }
            });
        }

        public override void WillTerminate(UIApplication application)
        {
            if (protectedDataWillBecomeUnavailableObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(protectedDataWillBecomeUnavailableObserver);
                protectedDataWillBecomeUnavailableObserver = null;
            }
            if (appWillResignActiveObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(appWillResignActiveObserver);
                appWillResignActiveObserver = null;
            }
            if (appDidEnterBackgroundObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(appDidEnterBackgroundObserver);
                appDidEnterBackgroundObserver = null;
            }
            base.WillTerminate(application);
        }
    }
}
