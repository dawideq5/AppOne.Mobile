// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Platforms/Android/DWIntentReceiver.cs
using Android.App;
using Android.Content;
using Android.Util;
using CommunityToolkit.Mvvm.Messaging;
using datawedge_MAUI_SampleApp.Messaging; // Poprawka CS0234, CS0246: Upewnij się, że Messaging istnieje i zawiera BarcodeScannedMessage
using Microsoft.Maui.ApplicationModel;

namespace datawedge_MAUI_SampleApp.Platforms.Android
{
    [BroadcastReceiver(Exported = true, Enabled = true)]
    [IntentFilter(new[] { DWIntentReceiver.IntentActionScan }, Categories = new[] { Intent.CategoryDefault })]
    public class DWIntentReceiver : BroadcastReceiver
    {
        public const string IntentActionScan = "com.symbol.datawedge.api.ACTION";
        public const string IntentDataString = "com.symbol.datawedge.data_string";
        public const string IntentDataLabelType = "com.symbol.datawedge.label_type";

        public override void OnReceive(Context? context, Intent? intent)
        {
            Log.Debug("DWIntentReceiver", $"Received intent: {intent?.Action}");

            if (intent?.Action == IntentActionScan)
            {
                string? scanData = intent.GetStringExtra(IntentDataString);
                string? labelType = intent.GetStringExtra(IntentDataLabelType);

                if (!string.IsNullOrEmpty(scanData))
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        // Poprawka CS0246: Upewnij się, że BarcodeScannedMessage jest poprawnie zdefiniowane
                        WeakReferenceMessenger.Default.Send(new BarcodeScannedMessage(scanData, labelType ?? "Unknown"));
                    });
                    Log.Info("DWIntentReceiver", $"Scan data: {scanData}, Type: {labelType}");
                }
                else
                {
                    Log.Warn("DWIntentReceiver", "Received scan intent but data is empty.");
                }
            }
            else
            {
                Log.Warn("DWIntentReceiver", $"Received unexpected intent action: {intent?.Action}");
            }
        }
    }
}
