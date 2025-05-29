// Interfaces/IApiClient.cs
using AppOne.Mobile.Models; // Upewnij się, że ValidationResponse jest tutaj
using datawedge_MAUI_SampleApp.Models;
using System.Threading.Tasks;

namespace AppOne.Mobile.Interfaces
{
    public interface IApiClient
    {
        Task<ValidationResponse> ValidateCodeAsync(string code);
        // Dodaj inne metody API, jeśli są potrzebne
        // Przykład: Task<User> GetUserAsync(string userId);
    }
}
