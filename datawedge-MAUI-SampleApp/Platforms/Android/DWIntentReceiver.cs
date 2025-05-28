// Typ pliku: Plik C#
// Lokalizacja: datawedge_MAUI_SampleApp/Platforms/Android/DWIntentReceiver.cs

// Włączenie kontekstu adnotacji nullable dla lepszego bezpieczeństwa typów
#nullable enable

using Android.App; // POPRAWKA: Dodano using dla [BroadcastReceiver] i [IntentFilter]
using Android.Content;
using Android.Util;
using datawedge_MAUI_SampleApp.Messaging; // Upewnij się, że przestrzeń nazw i klasa BarcodeScannedMessage są poprawnie zdefiniowane w Twoim projekcie
using CommunityToolkit.Mvvm.Messaging;    // Dodajemy using dla Messengera

namespace datawedge_MAUI_SampleApp.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = false)] // Exported = false dla bezpieczeństwa, jeśli nie jest to wymagane przez DataWedge
    // IntentFilter musi pasować do tego, co skonfigurowałeś w profilu DataWedge
    [IntentFilter(new[] { DWIntentReceiver.IntentAction })] // Użycie stałej dla spójności
    public class DWIntentReceiver : BroadcastReceiver
    {
        // Akcja, na którą nasłuchuje BroadcastReceiver - powinna być zgodna z konfiguracją DataWedge
        public const string IntentAction = "com.zebra.dwintents.ACTION"; // Upewnij się, że to jest akcja, na którą nasłuchujesz
        // public const string IntentCategory = "android.intent.category.DEFAULT"; // Ta stała nie jest używana w atrybucie IntentFilter, można ją usunąć, jeśli nie jest potrzebna gdzie indziej

        // Klucze używane przez DataWedge do przesyłania danych (sprawdź dokumentację Zebry dla pewności)
        public const string DataString = "com.symbol.datawedge.data_string";
        public const string LabelType = "com.symbol.datawedge.label_type";
        // Inne potencjalne klucze, które mogą być przydatne
        // public const string Source = "com.symbol.datawedge.source";
        // public const string DecodeData = "com.symbol.datawedge.decode_data";


        public override void OnReceive(Context? context, Intent? intent)
        {
            // Sprawdzenie, czy intent i jego akcja nie są null
            if (intent?.Action == null)
            {
                Log.Warn("DWIntentReceiver", "Received null intent or action.");
                return;
            }

            Log.Info("DWIntentReceiver", "Received intent with action: " + intent.Action);

            if (intent.Action.Equals(IntentAction))
            {
                // Odczytaj dane z intentu
                string? decodedData = intent.GetStringExtra(DataString);
                string? decodedLabelType = intent.GetStringExtra(LabelType); // Może być null, jeśli nie jest wysyłany

                if (!string.IsNullOrEmpty(decodedData))
                {
                    Log.Info("DWIntentReceiver", "Barcode scanned: " + decodedData + " | Type: " + (decodedLabelType ?? "N/A"));

                    // Wyślij wiadomość z zeskanowanym kodem
                    // ScannerView nasłuchuje na tę wiadomość
                    // Upewnij się, że klasa BarcodeScannedMessage jest poprawnie zdefiniowana
                    // i znajduje się w przestrzeni nazw datawedge_MAUI_SampleApp.Messaging
                    WeakReferenceMessenger.Default.Send(new BarcodeScannedMessage(decodedData));
                }
                else
                {
                    Log.Warn("DWIntentReceiver", "Intent received for action " + IntentAction + ", but no barcode data (DataString) found in extras.");
                }
            }
            else
            {
                Log.Warn("DWIntentReceiver", "Received intent with unhandled action: " + intent.Action);
            }
        }
    }
}
