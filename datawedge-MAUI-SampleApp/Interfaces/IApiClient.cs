// Plik: datawedge-MAUI-SampleApp/Interfaces/IApiClient.cs
namespace datawedge_MAUI_SampleApp.Interfaces
{
    /// <summary>
    /// Interfejs dla klienta API do wysyłania danych.
    /// </summary>
    public interface IApiClient
    {
        /// <summary>
        /// Wysyła kod kreskowy do API.
        /// </summary>
        /// <param name="barcode">Skanowany kod kreskowy.</param>
        /// <returns>Odpowiedź z API.</returns>
        Task<string> SendBarcodeAsync(string barcode);
        // Tutaj można dodać inne metody API w przyszłości.
    }
}
