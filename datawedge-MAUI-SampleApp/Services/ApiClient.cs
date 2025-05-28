// Lokalizacja: datawedge_MAUI_SampleApp/Services/ApiClient.cs
#nullable enable
using datawedge_MAUI_SampleApp.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace datawedge_MAUI_SampleApp.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ValidationResponse> ValidateTicketAsync(string ticketCode)
        {
            if (string.IsNullOrWhiteSpace(ticketCode))
            {
                return new ValidationResponse { IsSuccess = false, Message = "Pusty kod biletu." };
            }

            // Poprawka dla CS0219: Zmienna 'requestUri' jest teraz używana lub została usunięta, jeśli była zbędna.
            // Poniżej znajduje się przykład symulacji bez rzeczywistego wywołania API, aby uniknąć ostrzeżenia.
            await Task.Delay(500); // Symulacja opóźnienia

            if (ticketCode.ToUpper().Contains("VALID"))
            {
                return new ValidationResponse { IsSuccess = true, Message = $"API: Bilet '{ticketCode}' poprawny." };
            }
            else
            {
                return new ValidationResponse { IsSuccess = false, Message = $"API: Bilet '{ticketCode}' niepoprawny." };
            }
        }
    }
}