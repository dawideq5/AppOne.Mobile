// Lokalizacja: datawedge_MAUI_SampleApp/Platforms/Android/DWIntentReceiver.cs
#nullable enable
using Android.App;
using Android.Content;
using Android.Util;
using datawedge_MAUI_SampleApp.Messaging;
using CommunityToolkit.Mvvm.Messaging;
using System; // Dla DateTime

namespace datawedge_MAUI_SampleApp.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = true)] // Exported = true, jeśli DataWedge wysyła broadcast do innych aplikacji
                                                         // lub jeśli profil jest skonfigurowany do wysyłania do konkretnej akcji bez komponentu.
                                                         // Jeśli profil DataWedge jest skonfigurowany do wysyłania do konkretnego komponentu
                                                         // (np. .DWIntentReceiver), Exported może być false.
                                                         // Dla bezpieczeństwa, jeśli nie jest to absolutnie konieczne, preferuj Exported = false.
                                                         // Jednakże, aby DataWedge mógł wysłać broadcast, często Exported=true jest wymagane.
    [IntentFilter(new[] { DWIntentReceiver.IntentAction }, Categories = new[] { Intent.CategoryDefault })]
    public class DWIntentReceiver : BroadcastReceiver
    {
        // Akcja, na którą nasłuchuje BroadcastReceiver - MUSI być zgodna z konfiguracją profilu DataWedge
        public const string IntentAction = "com.appone.BARCODE_ACTION_MAUI"; // Użyj tej samej akcji, co w DataWedgeService

        // Klucze używane przez DataWedge (sprawdź dokumentację Zebry)
        public const string DataStringTag = "com.symbol.datawedge.data_string";
        public const string SymbologyTag = "com.symbol.datawedge.label_type";
        public const string SourceTag = "com.symbol.datawedge.source";
        public const string DecodedTimeTag = "com.symbol.datawedge.decoded_time";


        public override void OnReceive(Context? context, Intent? intent)
        {
            if (intent == null)
            {
                Log.Warn("DWIntentReceiver", "Received null intent.");
                return;
            }

            string action = intent.Action ?? "Unknown action";
            Log.Info("DWIntentReceiver", $"Received intent with action: {action}");

            if (action == IntentAction)
            {
                string? scanData = intent.GetStringExtra(DataStringTag);
                string? symbology = intent.GetStringExtra(SymbologyTag);
                // string source = intent.GetStringExtra(SourceTag); // Można odkomentować, jeśli potrzebne

                if (!string.IsNullOrEmpty(scanData))
                {
                    Log.Info("DWIntentReceiver", $"ScanData: {scanData}, Symbology: {symbology ?? "N/A"}");
                    // Wysyłanie wiadomości z danymi kodu i symboliką
                    WeakReferenceMessenger.Default.Send(new BarcodeScannedMessage(scanData, symbology));
                }
                else
                {
                    Log.Warn("DWIntentReceiver", "Scan intent received, but scanData is empty.");
                }
            }
        }
    }
}
