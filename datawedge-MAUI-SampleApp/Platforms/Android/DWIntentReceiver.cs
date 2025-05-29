// Platforms/Android/DWIntentReceiver.cs
using Android.App;
using Android.Content;
using AppOne.Mobile.Services; // Dla DataWedgeService.OnScanReceived

namespace AppOne.Mobile.Platforms.Android // Zmieniono namespace
{
    // Upewnij się, że akcja w IntentFilter zgadza się z konfiguracją DataWedge
    // oraz z tym, co jest używane w DataWedgeService do wysyłania wiadomości.
    // Ta akcja powinna być unikalna dla Twojej aplikacji.
    public const string IntentActionScan = "com.appone.mobile.SCAN"; // Przykładowa akcja, dostosuj!
    public const string IntentDataString = "com.symbol.datawedge.data_string";
    public const string IntentSymbology = "com.symbol.datawedge.label_type";


    // Wyłącz Exported = true, jeśli odbiorca nie musi być wywoływany przez inne aplikacje
    // Jeśli DataWedge wysyła broadcast, Exported może być potrzebne, ale zwykle DataWedge
    // jest skonfigurowany do wysyłania do konkretnej aktywności lub przez explicit intent.
    // Jeśli używasz profilu DataWedge skonfigurowanego do wysyłania broadcastu na tę akcję,
    // to Exported = true jest poprawne.
    [BroadcastReceiver(Enabled = true, Exported = true)] // Ustaw Exported odpowiednio
    [IntentFilter(new[] { IntentActionScan })]
    public class DWIntentReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            if (intent != null && context != null)
            {
                DataWedgeService.OnScanReceived(context, intent);
            }
        }
    }
}
