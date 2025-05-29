// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Services/AuthenticationService.cs
using datawedge_MAUI_SampleApp.Interfaces;
using Microsoft.Maui.Storage; // For SecureStorage
using System.Threading.Tasks;

namespace datawedge_MAUI_SampleApp.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string AuthTokenKey = "AuthToken";
        private const string UsernameKey = "Username"; // Key to store username

        public async Task<string?> GetCurrentUserAsync()
        {
            return await SecureStorage.GetAsync(UsernameKey);
        }

        // Corrected: Return type changed to Task<string?> to match potential interface and allow null
        public async Task<string?> LoginAsync(string username, string password)
        {
            // Simulate API call or actual authentication logic
            // In a real app, you would call your backend API here
            if (username == "test" && password == "password") // Example credentials
            {
                var token = $"dummy-token-for-{username}-{System.DateTime.UtcNow.Ticks}";
                await SecureStorage.SetAsync(AuthTokenKey, token);
                await SecureStorage.SetAsync(UsernameKey, username); // Store username
                return token;
            }
            return null; // Return null if login fails
        }

        public void Logout()
        {
            SecureStorage.Remove(AuthTokenKey);
            SecureStorage.Remove(UsernameKey); // Remove username on logout
        }

        public async Task<bool> IsUserAuthenticatedAsync()
        {
            var token = await SecureStorage.GetAsync(AuthTokenKey);
            return !string.IsNullOrEmpty(token);
        }

        public async Task<string?> GetAuthTokenAsync()
        {
            return await SecureStorage.GetAsync(AuthTokenKey);
        }
    }
}
