// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Services/NotificationService.cs
using datawedge_MAUI_SampleApp.Interfaces;
using System.Threading.Tasks; // Dodano dla Task

namespace datawedge_MAUI_SampleApp.Services
{
    public class NotificationService : INotificationService
    {
        public Task ShowNotification(string title, string message, string cancel)
        {
            if (Application.Current?.MainPage == null)
            {
                return Task.CompletedTask;
            }
            // Użyj DisplayAlert z MainPage aplikacji
            return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public Task<bool> ShowConfirmation(string title, string message, string accept, string cancel)
        {
            if (Application.Current?.MainPage == null)
            {
                return Task.FromResult(false);
            }
            // Użyj DisplayAlert z MainPage aplikacji
            return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
