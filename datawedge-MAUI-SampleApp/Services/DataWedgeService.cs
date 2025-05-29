// Services/DataWedgeService.cs
using AppOne.Mobile.Interfaces;
using AppOne.Mobile.Messaging; // Dla BarcodeScannedMessage
using CommunityToolkit.Mvvm.Messaging; // Dla WeakReferenceMessenger
using System;
using Android.OS;
using datawedge_MAUI_SampleApp.Platforms.Android;

using datawedge_MAUI_SampleApp.Messaging;


#if ANDROID
using Android.Content;
using Application = Android.App.Application;
#endif

namespace AppOne.Mobile.Services
{
    public class DataWedgeService : IDataWedgeService
    {
        // Zdefiniuj stałe dla akcji i extras DataWedge, jeśli są potrzebne globalnie
        public const string ActionDataWedgeFrom62 = "com.symbol.datawedge.api.ACTION_DATAWEDGE_FROM_6_2";
        public const string ExtraSwitchToProfile = "com.symbol.datawedge.api.SWITCH_TO_PROFILE";
        public const string ExtraScannerInputPlugin = "com.symbol.datawedge.api.SCANNER_INPUT_PLUGIN";
        public const string ExtraEnablePlugin = "ENABLE_PLUGIN";
        public const string ExtraDisablePlugin = "DISABLE_PLUGIN";
        public const string DataWedgeProfileName = "AppOneProfile"; // Ustaw nazwę swojego profilu DataWedge

        public DataWedgeService()
        {
            // Konstruktor może być pusty lub zawierać inicjalizację specyficzną dla platformy
        }

        public void Init()
        {
            // Tutaj można umieścić logikę inicjalizacji, np. tworzenie profilu DataWedge,
            // jeśli nie jest on tworzony ręcznie lub przez konfigurację.
            // Na ogół, profil jest konfigurowany w DataWedge, a aplikacja tylko się do niego przełącza
            // lub włącza/wyłącza skanowanie w domyślnym profilu.
            System.Diagnostics.Debug.WriteLine("DataWedgeService: Init called.");

            // Przykładowe tworzenie profilu (zaawansowane, zwykle niepotrzebne jeśli profil istnieje)
            // CreateDataWedgeProfile();
        }

        public void EnableScanning()
        {
#if ANDROID
            System.Diagnostics.Debug.WriteLine("DataWedgeService: Attempting to enable scanning.");
            try
            {
                Bundle b = new Bundle();
                b.PutString(ExtraSwitchToProfile, DataWedgeProfileName); // Przełącz na swój profil
                SendDataWedgeIntentWithExtra(ActionDataWedgeFrom62, b);

                // Alternatywnie, jeśli nie używasz profili, a chcesz włączyć skaner globalnie
                // Bundle bEnable = new Bundle();
                // bEnable.PutString(ExtraScannerInputPlugin, ExtraEnablePlugin);
                // SendDataWedgeIntentWithExtra(ActionDataWedgeFrom62, bEnable);

                System.Diagnostics.Debug.WriteLine($"DataWedgeService: Sent intent to switch to profile '{DataWedgeProfileName}' or enable plugin.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DataWedgeService: Error enabling scanning - {ex.Message}");
            }
#else
            System.Diagnostics.Debug.WriteLine("DataWedgeService: EnableScanning called on non-Android platform.");
#endif
        }

        public void DisableScanning()
        {
#if ANDROID
            System.Diagnostics.Debug.WriteLine("DataWedgeService: Attempting to disable scanning.");
            try
            {
                // Aby wyłączyć skanowanie, możesz przełączyć się na profil, który ma skaner wyłączony,
                // lub wysłać komendę wyłączenia pluginu skanera.
                // Poniżej przykład wyłączenia pluginu:
                Bundle bDisable = new Bundle();
                bDisable.PutString(ExtraScannerInputPlugin, ExtraDisablePlugin);
                SendDataWedgeIntentWithExtra(ActionDataWedgeFrom62, bDisable);
                System.Diagnostics.Debug.WriteLine("DataWedgeService: Sent intent to disable scanner plugin.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DataWedgeService: Error disabling scanning - {ex.Message}");
            }
#else
            System.Diagnostics.Debug.WriteLine("DataWedgeService: DisableScanning called on non-Android platform.");
#endif
        }

#if ANDROID
        private void SendDataWedgeIntentWithExtra(string action, Bundle extras)
        {
            var intent = new Intent();
            intent.SetAction(action);
            intent.PutExtras(extras);
            Application.Context.SendBroadcast(intent);
        }

        // Ta metoda jest wywoływana przez DWIntentReceiver
        public static void OnScanReceived(Context context, Intent intent)
        {
            string? action = intent.Action;
            if (action == DWIntentReceiver.IntentActionScan) // Upewnij się, że DWIntentReceiver.IntentActionScan jest poprawnie zdefiniowany
            {
                string? data = intent.GetStringExtra(DWIntentReceiver.IntentDataString);
                string? symbology = intent.GetStringExtra(DWIntentReceiver.IntentSymbology);
                DateTime scanTime = DateTime.Now;

                if (!string.IsNullOrEmpty(data))
                {
                    System.Diagnostics.Debug.WriteLine($"DataWedgeService: Scan Received - Data: {data}, Symbology: {symbology}");
                    // Wyślij wiadomość przez Messengera
                    WeakReferenceMessenger.Default.Send(new BarcodeScannedMessage(data, symbology ?? "Unknown", scanTime));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("DataWedgeService: Scan Received but data is null or empty.");
                }
            }
        }
#endif

        // Przykładowa metoda tworzenia profilu DataWedge (zaawansowane)
        // Zwykle profil konfiguruje się bezpośrednio w aplikacji DataWedge na urządzeniu.
        private void CreateDataWedgeProfile()
        {
#if ANDROID
            // Ta metoda jest bardziej złożona i wymaga ostrożności.
            // Zobacz dokumentację Zebra DataWedge dla szczegółów tworzenia profili programowo.
            // https://techdocs.zebra.com/datawedge/latest/guide/api/creatingprofiles/
            System.Diagnostics.Debug.WriteLine($"DataWedgeService: Attempting to create profile '{DataWedgeProfileName}'.");
            Bundle profileConfig = new Bundle();
            profileConfig.PutString("PROFILE_NAME", DataWedgeProfileName);
            profileConfig.PutString("PROFILE_ENABLED", "true"); // Włącz profil
            profileConfig.PutString("CONFIG_MODE", "CREATE_IF_NOT_EXIST"); // Utwórz, jeśli nie istnieje

            // Konfiguracja aplikacji powiązanych z profilem
            Bundle appConfig = new Bundle();
            appConfig.PutString("PACKAGE_NAME", Application.Context.PackageName); // Powiąż z tą aplikacją
            string[] appActivity = { "*" }; // Wszystkie aktywności
            appConfig.PutStringArray("ACTIVITY_LIST", appActivity);
            profileConfig.PutParcelableArray("APP_LIST", new Bundle[] { appConfig });

            // Konfiguracja pluginu skanera (Barcode input)
            Bundle barcodeConfig = new Bundle();
            barcodeConfig.PutString("PLUGIN_NAME", "BARCODE");
            barcodeConfig.PutString("RESET_CONFIG", "true"); // Zresetuj do domyślnych, a potem ustaw
            Bundle barcodeProps = new Bundle();
            barcodeProps.PutString("scanner_selection", "auto"); // Automatyczny wybór skanera
            barcodeProps.PutString("scanner_input_enabled", "true"); // Włącz skanowanie
            barcodeConfig.PutBundle("PARAM_LIST", barcodeProps);
            profileConfig.PutBundle("PLUGIN_CONFIG", barcodeConfig);


            // Konfiguracja pluginu Intent output (aby wysyłać dane do aplikacji)
            Bundle intentConfig = new Bundle();
            intentConfig.PutString("PLUGIN_NAME", "INTENT");
            intentConfig.PutString("RESET_CONFIG", "true");
            Bundle intentProps = new Bundle();
            intentProps.PutString("intent_output_enabled", "true");
            intentProps.PutString("intent_action", DWIntentReceiver.IntentActionScan); // Akcja, na którą nasłuchuje receiver
            intentProps.PutInt("intent_delivery", 2); // 2 for "Send via startActivity" (lub 0 dla broadcast, 1 dla startService)
            intentConfig.PutBundle("PARAM_LIST", intentProps);

            // Dodaj konfiguracje pluginów do profilu
            // Ważne: DataWedge od wersji 6.4 wymaga, aby konfiguracje pluginów były w tablicy.
            profileConfig.PutParcelableArray("PLUGIN_CONFIG", new Bundle[] { barcodeConfig, intentConfig });

            SendDataWedgeIntentWithExtra("com.symbol.datawedge.api.ACTION_DATAWEDGE_FROM_6_2", profileConfig); // Starsza akcja, może być potrzebna SetConfig
            // Dla nowszych wersji (6.3+) użyj SetConfig:
            // Bundle setConfigBundle = new Bundle();
            // setConfigBundle.PutBundle("com.symbol.datawedge.api.SET_CONFIG", profileConfig);
            // SendDataWedgeIntentWithExtra("com.symbol.datawedge.api.ACTION_DATAWEDGE_FROM_6_2", setConfigBundle); // To jest niepoprawne użycie dla SetConfig
            // Prawidłowe użycie SetConfig:
            // Intent i = new Intent();
            // i.SetAction("com.symbol.datawedge.api.ACTION");
            // i.PutExtra("com.symbol.datawedge.api.SET_CONFIG", profileConfig);
            // Application.Context.SendBroadcast(i);


            System.Diagnostics.Debug.WriteLine($"DataWedgeService: Intent to create profile '{DataWedgeProfileName}' sent.");
#endif
        }
    }
}
