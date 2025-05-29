// Services/NotificationService.cs
using AppOne.Mobile.Interfaces;
using Microsoft.Maui.Devices;
using Plugin.Maui.Audio;
using System;
using System.Threading.Tasks;

namespace AppOne.Mobile.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IAudioManager _audioManager;
        private const string ErrorSoundFileName = "error_sound.mp3"; // Upewnij się, że plik istnieje w Resources/Raw

        public NotificationService(IAudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        public async Task PlayErrorSoundAsync()
        {
            try
            {
                if (_audioManager == null)
                {
                    Console.WriteLine("Error: AudioManager is not initialized.");
                    return;
                }

                // Upewnij się, że plik error_sound.mp3 istnieje w Resources/Raw i ma Build Action = MauiAsset
                var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(ErrorSoundFileName));
                if (player != null)
                {
                    player.Play();
                }
                else
                {
                    Console.WriteLine($"Error: Could not create audio player for {ErrorSoundFileName}. Check if file exists and path is correct.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing sound: {ex.Message}");
            }
        }

        public async Task VibrateOnErrorAsync()
        {
            try
            {
                if (Vibration.Default.IsSupported)
                {
                    Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100));
                    await Task.Delay(150);
                    Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100));
                    await Task.Delay(150);
                    Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100));
                }
                else
                {
                    Console.WriteLine("Vibration not supported on this device.");
                }
            }
            catch (FeatureNotSupportedException ex)
            {
                Console.WriteLine($"Vibration not supported: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during vibration: {ex.Message}");
            }
        }
    }
}
