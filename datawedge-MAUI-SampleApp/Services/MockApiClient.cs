// Lokalizacja: datawedge_MAUI_SampleApp/Services/MockApiClient.cs
using datawedge_MAUI_SampleApp.Models;
using System.Threading.Tasks;
using System.Diagnostics;

namespace datawedge_MAUI_SampleApp.Services
{
    public class MockApiClient : IApiClient
    {
        public async Task<ValidationResponse> ValidateTicketAsync(string ticketCode)
        {
            Debug.WriteLine($"MockApiClient: Validating ticket '{ticketCode}'");
            // Symulacja opóźnienia sieciowego
            await Task.Delay(1000);

            if (string.IsNullOrWhiteSpace(ticketCode))
            {
                Debug.WriteLine("MockApiClient: Ticket code is empty.");
                return new ValidationResponse { IsSuccess = false, Message = "Pusty kod biletu." };
            }

            if (ticketCode.ToUpper().Contains("VALID"))
            {
                Debug.WriteLine($"MockApiClient: Ticket '{ticketCode}' is VALID.");
                return new ValidationResponse { IsSuccess = true, Message = $"Bilet '{ticketCode}' jest poprawny." };
            }
            else if (ticketCode.ToUpper().Contains("ERROR"))
            {
                Debug.WriteLine($"MockApiClient: Simulating error for ticket '{ticketCode}'.");
                throw new System.Net.Http.HttpRequestException("Symulowany błąd połączenia z serwerem.");
            }
            else
            {
                Debug.WriteLine($"MockApiClient: Ticket '{ticketCode}' is INVALID.");
                return new ValidationResponse { IsSuccess = false, Message = $"Bilet '{ticketCode}' jest niepoprawny." };
            }
        }
    }
}
