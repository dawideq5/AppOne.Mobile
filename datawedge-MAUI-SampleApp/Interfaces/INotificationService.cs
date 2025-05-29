// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Interfaces/INotificationService.cs
using System.Threading.Tasks;

namespace datawedge_MAUI_SampleApp.Interfaces
{
    public interface INotificationService
    {
        // Poprawka CS1061: Upewniono się, że metoda ShowNotification istnieje i ma prawidłową sygnaturę
        Task ShowNotification(string title, string message, string cancel);
        Task<bool> ShowConfirmation(string title, string message, string accept, string cancel);
    }
}
