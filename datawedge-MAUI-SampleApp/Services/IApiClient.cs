// Lokalizacja: datawedge_MAUI_SampleApp/Services/IApiClient.cs
using datawedge_MAUI_SampleApp.Models;
using System.Threading.Tasks;

namespace datawedge_MAUI_SampleApp.Services
{
    public interface IApiClient
    {
        Task<ValidationResponse> ValidateTicketAsync(string ticketCode);
        // W przyszłości można dodać:
        // Task<bool> LoginAsync(string username, string password);
    }
}
