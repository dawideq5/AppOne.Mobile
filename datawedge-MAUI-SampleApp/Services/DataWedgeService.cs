// Lokalizacja: datawedge-MAUI-SampleApp/Services/DataWedgeService.cs
#nullable enable
using System.Diagnostics;

namespace datawedge_MAUI_SampleApp.Services
{
    // Ta klasa implementuje interfejs IDataWedgeService
    public class DataWedgeService : IDataWedgeService
    {
        private const string ProfileName = "AppOneMAUIProfile";
        private const string ActionDatawedgeApi = "com.symbol.datawedge.api.ACTION";
        private const string ExtraSetCommand = "com.symbol.datawedge.api.SET_CONFIG";
        private const string ExtraCreateProfile = "com.symbol.datawedge.api.CREATE_PROFILE";
        private const string ExtraSoftScanTrigger = "com.symbol.datawedge.api.SOFT_SCAN_TRIGGER";
        private const string ExtraEnablePlugin = "com.symbol.datawedge.api.ENABLE_PLUGIN";
        private const string ExtraDisablePlugin = "com.symbol.datawedge.api.DISABLE_PLUGIN";

        // Akcja dla Intentu wyjściowego - MUSI być taka sama jak w DWIntentReceiver
        private const string OutputIntentAction = "com.appone.BARCODE_ACTION_MAUI";

        public void Initialize()
        {
            Debug.WriteLine("DataWedgeService: Initialize called.");
            CreateProfile();
        }

        private void CreateProfile()
        {
#if ANDROID
            try
            {
                var context = Android.App.Application.Context;

                // Utworzenie profilu
                var createProfileIntent = new Android.Content.Intent();
                createProfileIntent.SetAction(ActionDatawedgeApi);
                createProfileIntent.PutExtra(ExtraCreateProfile, ProfileName);
                context.SendBroadcast(createProfileIntent);
                Debug.WriteLine($"DataWedgeService: Sent CREATE_PROFILE command for '{ProfileName}'.");

                // Konfiguracja profilu
                var profileConfig = new Android.OS.Bundle();
                profileConfig.PutString("PROFILE_NAME", ProfileName);
                profileConfig.PutString("PROFILE_ENABLED", "true");
                profileConfig.PutString("CONFIG_MODE", "UPDATE");

                // Powiązanie aplikacji
                var appConfig = new Android.OS.Bundle();
                appConfig.PutString("PACKAGE_NAME", context.PackageName);
                appConfig.PutStringArray("ACTIVITY_LIST", new string[] { "*" });
                profileConfig.PutParcelableArray("APP_LIST", new Android.OS.Bundle[] { appConfig });

                // Konfiguracja wyjścia Intent
                var intentConfig = new Android.OS.Bundle();
                intentConfig.PutString("PLUGIN_NAME", "INTENT");
                intentConfig.PutString("RESET_CONFIG", "true");
                var intentProps = new Android.OS.Bundle();
                intentProps.PutString("intent_output_enabled", "true");
                intentProps.PutString("intent_action", OutputIntentAction);
                intentProps.PutString("intent_delivery", "2"); // 2 for Broadcast
                intentConfig.PutBundle("PARAM_LIST", intentProps);

                // Konfiguracja wejścia Barcode
                var barcodeConfig = new Android.OS.Bundle();
                barcodeConfig.PutString("PLUGIN_NAME", "BARCODE");
                barcodeConfig.PutString("RESET_CONFIG", "true");
                var barcodeProps = new Android.OS.Bundle();
                barcodeProps.PutString("scanner_selection", "auto");
                barcodeProps.PutString("scanner_input_enabled", "true");
                barcodeConfig.PutBundle("PARAM_LIST", barcodeProps);

                // Dodanie konfiguracji pluginów
                profileConfig.PutParcelableArray("PLUGIN_CONFIG", new Android.OS.Bundle[] { intentConfig, barcodeConfig });

                // Wysłanie konfiguracji
                var setConfigIntent = new Android.Content.Intent();
                setConfigIntent.SetAction(ActionDatawedgeApi);
                setConfigIntent.PutExtra(ExtraSetCommand, profileConfig);
                context.SendBroadcast(setConfigIntent);
                Debug.WriteLine($"DataWedgeService: Sent SET_CONFIG command for '{ProfileName}'.");
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"DataWedgeService: Error creating/updating profile: {ex.Message}");
            }
#else
            Debug.WriteLine("DataWedgeService: Skipping profile creation (not on Android).");
#endif
        }

        private void SendCommand(string command, string parameter)
        {
#if ANDROID
            var context = Android.App.Application.Context;
            var intent = new Android.Content.Intent();
            intent.SetAction(ActionDatawedgeApi);
            intent.PutExtra(command, parameter);
            context.SendBroadcast(intent);
#endif
        }

        public void EnableScanner(bool enable)
        {
            SendCommand(enable ? ExtraEnablePlugin : ExtraDisablePlugin, "BARCODE");
        }

        public void SoftScanTrigger(bool start)
        {
            SendCommand(ExtraSoftScanTrigger, start ? "START_SCANNING" : "STOP_SCANNING");
        }
    }
}