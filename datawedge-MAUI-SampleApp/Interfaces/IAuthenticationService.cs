// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Interfaces/IAuthenticationService.cs
using System.Threading.Tasks;

namespace datawedge_MAUI_SampleApp.Interfaces
{
    public interface IAuthenticationService
    {
        // Corrected: Changed return type to Task<string?> to allow for null token on failure
        // This aligns with the warning CS8619 and typical authentication flows.
        Task<string?> LoginAsync(string username, string password);
        void Logout();
        Task<bool> IsUserAuthenticatedAsync();
        Task<string?> GetCurrentUserAsync(); // To get the logged-in username
        Task<string?> GetAuthTokenAsync(); // To get the auth token if needed elsewhere
    }
}
