// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Services/IApiClient.cs
// UWAGA: Ten plik (Services/IApiClient.cs) prawdopodobnie powinien zostać USUNIĘTY.
// Interfejs IApiClient powinien być zdefiniowany TYLKO w Interfaces/IApiClient.cs.
// Posiadanie dwóch definicji prowadzi do błędów niejednoznaczności (CS0104).
// Poniższy kod jest tu tylko dla kompletności, jeśli z jakiegoś powodu ten plik jest nadal potrzebny,
// ale zalecane jest jego usunięcie i używanie tylko Interfaces/IApiClient.cs.

using AppOne.Mobile.Models; // Corrected namespace
using System.Threading.Tasks;

namespace datawedge_MAUI_SampleApp.Services
{
    public interface IApiClient
    {
        Task<string> GetHelloAsync();
        Task<string> GetSecuredHelloAsync();
        Task<ValidationResponse> ValidateBarcodeAsync(string barcode);
    }
}