// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Services/ApiClient.cs
using AppOne.Mobile.Models;
using datawedge_MAUI_SampleApp.Interfaces;
using datawedge_MAUI_SampleApp.Models;   // Poprawka CS0234: Upewnij się, że Models istnieje i zawiera ValidationResponse
using Microsoft.Maui.Storage;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace datawedge_MAUI_SampleApp.Services
{
    public class ApiClient : Interfaces.IApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://yourapi.azurewebsites.net";

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task PrepareHttpClientAuth()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<string> GetHelloAsync()
        {
            await PrepareHttpClientAuth();
            var response = await _httpClient.GetAsync($"{BaseUrl}/api/hello");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetSecuredHelloAsync()
        {
            await PrepareHttpClientAuth();
            var response = await _httpClient.GetAsync($"{BaseUrl}/api/secured/hello");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<ValidationResponse> ValidateBarcodeAsync(string barcode)
        {
            await PrepareHttpClientAuth();
            var requestContent = new StringContent(JsonSerializer.Serialize(new { barcodeData = barcode }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{BaseUrl}/api/validateBarcode", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                try
                {
                    var validationResponse = JsonSerializer.Deserialize<ValidationResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return validationResponse ?? new ValidationResponse { IsValid = false, Message = "Failed to deserialize API response." };
                }
                catch (JsonException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"JSON Deserialization Error: {ex}");
                    return new ValidationResponse { IsValid = false, Message = $"Error deserializing response: {ex.Message}" };
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                return new ValidationResponse { IsValid = false, Message = $"API request failed: {response.ReasonPhrase} - {errorContent}" };
            }
        }
    }
}
