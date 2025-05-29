// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Platforms/Android/DWIntentReceiver.cs
using Android.App;
using Android.Content;
using Android.Util;
using CommunityToolkit.Mvvm.Messaging;
using AppOne.Mobile.Messaging; // Poprawiono: Użycie prawidłowej przestrzeni nazw dla BarcodeScannedMessage
using Microsoft.Maui.ApplicationModel;
// Usunięto: using datawedge_MAUI_SampleApp.Messaging; // To powodowało błąd, jeśli BarcodeScannedMessage jest w AppOne.Mobile.Messaging

namespace datawedge_MAUI_SampleApp.Platforms.Android
{
    [BroadcastReceiver(Exported = true, Enabled = true)]
    // Upewnij się, że akcja w IntentFilter jest zgodna z konfiguracją profilu DataWedge
    // Common DataWedge action: "com.symbol.datawedge.api.ACTION"
    // Action from AndroidManifest.xml: "com.appone.mobile.SCAN"
    // Action from DataWedgeService: datawedge_MAUI_SampleApp.Platforms.Android.DWIntentReceiver.IntentActionScan (która może być "com.symbol.datawedge.api.ACTION")
    // Wybierz JEDNĄ spójną akcję. Jeśli w Manifeście jest "com.appone.mobile.SCAN" i DataWedge jest tak skonfigurowany, użyj tej.
    // Poniżej używam stałej zdefiniowanej w tej klasie dla spójności wewnątrz tego pliku.
    [IntentFilter(new[] { DWIntentReceiver.IntentActionScan }, Categories = new[] { Intent.CategoryDefault })]
    public class DWIntentReceiver : BroadcastReceiver
    {
        // Ta akcja powinna być tą, którą DataWedge jest skonfigurowany wysyłać.
        // Może to być domyślna akcja DataWedge lub niestandardowa zdefiniowana w profilu.
        // W AndroidManifest.xml masz <action android:name="com.appone.mobile.SCAN" />
        // Jeśli DataWedge jest skonfigurowany do wysyłania tej akcji, to pole powinno mieć tę wartość.
        public const string IntentActionScan = "com.appone.mobile.SCAN"; // Zgodnie z AndroidManifest.xml lub konfiguracją profilu DataWedge
        public const string IntentDataString = "com.symbol.datawedge.data_string"; // Standardowy klucz DataWedge
        public const string IntentDataLabelType = "com.symbol.datawedge.label_type"; // Standardowy klucz DataWedge

        // Alternatywne klucze, które mogą być używane przez DataWedge w zależności od konfiguracji
        public const string ResultScanString = "com.symbol.datawedge.api.RESULT_SCAN_STRING";
        public const string ResultLabelType = "com.symbol.datawedge.api.RESULT_LABEL_TYPE";
        public const string SourceScanner = "com.symbol.datawedge.source"; // np. "scanner", "msr", "simulscan"
        public const string ActionDataWedgeFromApi = "com.symbol.datawedge.api.ACTION"; // Używane do komend API
        public const string ResultAction = "com.symbol.datawedge.api.RESULT_ACTION"; // Używane dla wyników komend API


        public override void OnReceive(Context? context, Intent? intent)
        {
            Log.Debug("DWIntentReceiver", $"Odebrano intent: {intent?.Action}");

            if (intent == null)
            {
                Log.Warn("DWIntentReceiver", "Odebrano pusty intent.");
                return;
            }

            // Sprawdź, czy akcja intencji to ta, na którą nasłuchujemy dla skanów
            if (intent.Action == IntentActionScan)
            {
                string? scanData = intent.GetStringExtra(IntentDataString);
                string? labelType = intent.GetStringExtra(IntentDataLabelType);

                // Czasami DataWedge może wysyłać dane pod innymi kluczami, szczególnie jeśli jest to wynik komendy API
                if (string.IsNullOrEmpty(scanData))
                {
                    scanData = intent.GetStringExtra(ResultScanString);
                }
                if (string.IsNullOrEmpty(labelType))
                {
                    labelType = intent.GetStringExtra(ResultLabelType);
                }

                if (!string.IsNullOrEmpty(scanData))
                {
                    Log.Info("DWIntentReceiver", $"Zeskanowane dane: {scanData}, Typ: {labelType ?? "N/A"}");
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        // Używamy BarcodeScannedMessage z przestrzeni nazw AppOne.Mobile.Messaging
                        WeakReferenceMessenger.Default.Send(new BarcodeScannedMessage(scanData, labelType ?? "Unknown", DateTime.Now));
                    });
                }
                else
                {
                    Log.Warn("DWIntentReceiver", "Odebrano intent skanowania, ale dane są puste lub pod nieoczekiwanymi kluczami.");
                    // Możesz tutaj zalogować wszystkie extrasy, aby zobaczyć, co faktycznie przyszło
                    // if (intent.Extras != null)
                    // {
                    //     foreach (var key in intent.Extras.KeySet() ?? new List<string>())
                    //     {
                    //         Log.Debug("DWIntentReceiver", $"Extra: {key} = {intent.Extras.Get(key)}");
                    //     }
                    // }
                }
            }
            else
            {
                Log.Warn("DWIntentReceiver", $"Odebrano nieoczekiwaną akcję intencji: {intent.Action}");
            }
        }
    }
}