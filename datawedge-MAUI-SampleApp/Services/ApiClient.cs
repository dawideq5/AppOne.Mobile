// Services/ApiClient.cs
using AppOne.Mobile.Interfaces;
using AppOne.Mobile.Models;
using datawedge_MAUI_SampleApp.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppOne.Mobile.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://your-api-base-url.com/api/"; // ZMIEŃ NA SWÓJ ADRES API

        public ApiClient()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
            // Możesz tutaj dodać domyślne nagłówki, np. Authorization, jeśli używasz tokenów
        }

        public async Task<ValidationResponse> ValidateCodeAsync(string code)
        {
            try
            {
                // Przykładowe żądanie GET. Dostosuj do swojego API.
                // HttpResponseMessage response = await _httpClient.GetAsync($"validate?code={code}");
                // response.EnsureSuccessStatusCode();
                // string content = await response.Content.ReadAsStringAsync();
                // return JsonSerializer.Deserialize<ValidationResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                //       ?? new ValidationResponse { IsValid = false, Message = "Błąd deserializacji odpowiedzi API." };

                // --- SYMULACJA ---
                // Zastąp to rzeczywistym wywołaniem API
                await Task.Delay(500); // Symulacja opóźnienia sieciowego
                if (string.IsNullOrWhiteSpace(code))
                {
                    return new ValidationResponse { IsValid = false, Message = "Kod nie może być pusty." };
                }
                if (code.StartsWith("INVALID"))
                {
                    return new ValidationResponse { IsValid = false, Message = "Kod nieprawidłowy (symulacja z API)." };
                }
                if (code == "TIMEOUT") // Symulacja błędu
                {
                    throw new TimeoutException("API call timed out.");
                }
                return new ValidationResponse { IsValid = true, Message = "Kod prawidłowy (symulacja z API)." };
                // --- KONIEC SYMULACJI ---
            }
            catch (HttpRequestException ex)
            {
                // Błąd połączenia lub błąd HTTP
                Console.WriteLine($"API request error: {ex.Message}");
                return new ValidationResponse { IsValid = false, Message = $"Błąd połączenia z API: {ex.Message}" };
            }
            catch (JsonException ex)
            {
                // Błąd deserializacji
                Console.WriteLine($"API deserialization error: {ex.Message}");
                return new ValidationResponse { IsValid = false, Message = "Błąd przetwarzania odpowiedzi z API." };
            }
            catch (Exception ex)
            {
                // Inne błędy
                Console.WriteLine($"API general error: {ex.Message}");
                return new ValidationResponse { IsValid = false, Message = $"Wystąpił nieoczekiwany błąd: {ex.Message}" };
            }
        }
    }
}
