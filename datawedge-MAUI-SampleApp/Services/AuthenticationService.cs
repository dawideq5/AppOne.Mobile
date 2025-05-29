// Services/AuthenticationService.cs
using AppOne.Mobile.Interfaces;
using Microsoft.Maui.Storage;
using System;
using System.Threading.Tasks;

namespace AppOne.Mobile.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string AuthKey = "auth_user";
        private const string LastUserKey = "last_user_login";

        public bool IsLoggedIn => !string.IsNullOrEmpty(Preferences.Get(AuthKey, string.Empty));
        public string? UserName => Preferences.Get(AuthKey, string.Empty);

        public event EventHandler? AuthenticationStateChanged;

        public async Task<bool> LoginAsync(string username, string password)
        {
            // TODO: Zastąp to rzeczywistą logiką walidacji (np. wywołanie API)
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            // Symulacja udanego logowania
            SetUser(username);
            await SaveLastLoggedInUserAsync(username);
            return true;
        }

        public void SetUser(string username)
        {
            Preferences.Set(AuthKey, username);
            AuthenticationStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Logout()
        {
            Preferences.Remove(AuthKey);
            AuthenticationStateChanged?.Invoke(this, EventArgs.Empty);
            Console.WriteLine("User logged out.");
        }

        public Task<string?> GetLastLoggedInUserAsync()
        {
            return Task.FromResult(Preferences.Get(LastUserKey, string.Empty));
        }

        public Task SaveLastLoggedInUserAsync(string username)
        {
            Preferences.Set(LastUserKey, username);
            return Task.CompletedTask;
        }
    }
}
