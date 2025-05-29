// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI_SampleApp/Interfaces/IApiClient.cs
using AppOne.Mobile.Models; // Corrected namespace
using System.Threading.Tasks;

namespace datawedge_MAUI_SampleApp.Interfaces
{
    public interface IApiClient
    {
        Task<string> GetHelloAsync();
        Task<string> GetSecuredHelloAsync();
        Task<ValidationResponse> ValidateBarcodeAsync(string barcode);
    }
}