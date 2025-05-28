// Lokalizacja: datawedge_MAUI_SampleApp/Messaging/BarcodeScannedMessage.cs
using CommunityToolkit.Mvvm.Messaging.Messages; // Dla ValueChangedMessage

namespace datawedge_MAUI_SampleApp.Messaging
{
    // Wiadomość używana do przekazywania danych zeskanowanego kodu kreskowego.
    // Dziedziczy z ValueChangedMessage<string>, gdzie Value to dane kodu.
    public class BarcodeScannedMessage : ValueChangedMessage<string>
    {
        // Dodatkowe właściwości, jeśli potrzebne, np. typ kodu, czas skanowania
        public string? Symbology { get; }
        public System.DateTime Timestamp { get; }

        public BarcodeScannedMessage(string barcodeData, string? symbology = null) : base(barcodeData)
        {
            Symbology = symbology;
            Timestamp = System.DateTime.Now;
        }
    }
}
