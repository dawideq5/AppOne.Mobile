// Messaging/BarcodeScannedMessage.cs
using System;

namespace AppOne.Mobile.Messaging
{
    public class BarcodeScannedMessage
    {
        public string Barcode { get; }
        public string Symbology { get; }
        public DateTime ScanTime { get; }

        public BarcodeScannedMessage(string barcode, string symbology, DateTime scanTime)
        {
            Barcode = barcode;
            Symbology = symbology;
            ScanTime = scanTime;
        }
    }
}
