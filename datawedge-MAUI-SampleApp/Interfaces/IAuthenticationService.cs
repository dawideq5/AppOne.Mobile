// Interfaces/IAuthenticationService.cs
namespace AppOne.Mobile.Interfaces
{
    public interface IAuthenticationService
    {
        bool IsLoggedIn { get; }
        string? UserName { get; } // Może być null, jeśli użytkownik nie jest zalogowany

        event EventHandler? AuthenticationStateChanged;

        Task<bool> LoginAsync(string username, string password);
        void Logout();
        Task<string?> GetLastLoggedInUserAsync(); // Może zwrócić null
        Task SaveLastLoggedInUserAsync(string username);
        void SetUser(string username);
    }
}
