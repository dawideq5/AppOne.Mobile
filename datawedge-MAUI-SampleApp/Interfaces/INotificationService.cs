// Interfaces/INotificationService.cs
namespace AppOne.Mobile.Interfaces
{
    public interface INotificationService
    {
        Task PlayErrorSoundAsync();
        Task VibrateOnErrorAsync();
    }
}
