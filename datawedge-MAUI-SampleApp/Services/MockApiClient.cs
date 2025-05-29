// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Services/MockApiClient.cs
using AppOne.Mobile.Models;
using datawedge_MAUI_SampleApp.Interfaces;
using datawedge_MAUI_SampleApp.Models;   // Poprawka CS0234: Upewnij się, że Models istnieje i zawiera ValidationResponse
using System.Threading.Tasks;

namespace datawedge_MAUI_SampleApp.Services
{
    public class MockApiClient : Interfaces.IApiClient
    {
        public Task<string> GetHelloAsync()
        {
            return Task.FromResult("Hello from Mock API!");
        }

        public Task<string> GetSecuredHelloAsync()
        {
            return Task.FromResult("Hello from Secured Mock API!");
        }

        public Task<ValidationResponse> ValidateBarcodeAsync(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return Task.FromResult(new ValidationResponse { IsValid = false, Message = "Kod kreskowy nie może być pusty." });
            }

            if (barcode == "12345VALID")
            {
                return Task.FromResult(new ValidationResponse { IsValid = true, Message = "Kod kreskowy jest prawidłowy (Mock)." });
            }
            else if (barcode == "98765INVALID")
            {
                return Task.FromResult(new ValidationResponse { IsValid = false, Message = "Kod kreskowy jest nieprawidłowy (Mock)." });
            }
            else
            {
                return Task.FromResult(new ValidationResponse { IsValid = false, Message = "Kod kreskowy nierozpoznany przez Mock API." });
            }
        }
    }
}
