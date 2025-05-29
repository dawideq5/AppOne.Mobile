// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Services/DataWedgeService.cs
#if ANDROID
using Android.Content;
using Android.OS;
#endif
using System;
using datawedge_MAUI_SampleApp.Interfaces; // This should be the correct IDataWedgeService from Interfaces folder
// using datawedge_MAUI_SampleApp.Platforms.Android; // DWIntentReceiver is in this namespace. Used for IntentActionScan.
using AppOne.Mobile.Messaging;      // Corrected: This is where BarcodeScannedMessage is.
using Microsoft.Maui.ApplicationModel;
using CommunityToolkit.Mvvm.Messaging;
// Potentially remove datawedge_MAUI_SampleApp.Messaging if BarcodeScannedMessage comes only from AppOne.Mobile.Messaging
// using datawedge_MAUI_SampleApp.Messaging;

namespace datawedge_MAUI_SampleApp.Services
{
    // Ensure this implements the IDataWedgeService from datawedge_MAUI_SampleApp.Interfaces
    public class DataWedgeService : Interfaces.IDataWedgeService
    {
        private const string ActionDatawedgeApi = "com.symbol.datawedge.api.ACTION";
        private const string ExtraSetDefaultProfile = "com.symbol.datawedge.api.SET_DEFAULT_PROFILE";
        private const string ExtraCreateProfile = "com.symbol.datawedge.api.CREATE_PROFILE";
        private const string ExtraSoftScanTrigger = "com.symbol.datawedge.api.SOFT_SCAN_TRIGGER";
        private const string ExtraConfigMode = "CONFIG_MODE";
        private const string ExtraProfileName = "PROFILE_NAME";
        private const string ExtraProfileEnabled = "PROFILE_ENABLED";
        private const string ExtraAppList = "APP_LIST";
        private const string ExtraPluginConfig = "PLUGIN_CONFIG";
        private const string ExtraScannerInputPlugin = "com.symbol.datawedge.api.SCANNER_INPUT_PLUGIN"; // For enabling/disabling

        // Profile name used by this application
        private const string AppOneProfileName = "AppOneMAUIProfile";
        // Action for the intent output from DataWedge, should match DWIntentReceiver
        private const string IntentActionScanOutput = datawedge_MAUI_SampleApp.Platforms.Android.DWIntentReceiver.IntentActionScan;


        public DataWedgeService()
        {
            // Register to receive messages of type AppOne.Mobile.Messaging.BarcodeScannedMessage
            WeakReferenceMessenger.Default.Register<AppOne.Mobile.Messaging.BarcodeScannedMessage>(this, (r, m) =>
            {
                System.Diagnostics.Debug.WriteLine($"DataWedgeService (via WeakReferenceMessenger) received scan: {m.Barcode} ({m.Symbology}) at {m.ScanTime}");
                // This handler is for messages sent via WeakReferenceMessenger, typically from DWIntentReceiver
            });
        }

        public void InitializeDataWedge()
        {
#if ANDROID
            System.Diagnostics.Debug.WriteLine("DataWedgeService: InitializeDataWedge called.");
            CreateProfile(); // Create the profile on initialization
#endif
        }

        private void SendDataWedgeIntentWithExtra(string action, string extraKey, Bundle extraValue)
        {
#if ANDROID
            var context = Platform.AppContext;
            Intent intent = new Intent();
            intent.SetAction(action);
            intent.PutExtra(extraKey, extraValue);
            context.SendBroadcast(intent);
#endif
        }

        private void SendDataWedgeIntentWithExtra(string action, string extraKey, string extraValue)
        {
#if ANDROID
            var context = Platform.AppContext;
            Intent intent = new Intent();
            intent.SetAction(action);
            intent.PutExtra(extraKey, extraValue);
            context.SendBroadcast(intent);
#endif
        }
        private void SendDataWedgeCommand(string command, string parameter)
        {
#if ANDROID
            var context = Platform.AppContext;
            if (context == null) return;

            Intent intent = new Intent();
            intent.SetAction(ActionDatawedgeApi);
            intent.PutExtra(command, parameter);
            context.SendBroadcast(intent);
#endif
        }


        public void CreateProfile()
        {
#if ANDROID
            System.Diagnostics.Debug.WriteLine($"DataWedgeService: Attempting to create DataWedge profile: {AppOneProfileName}");
            var context = Platform.AppContext;
            if (context == null)
            {
                System.Diagnostics.Debug.WriteLine("DataWedgeService: Android context is null, cannot create profile.");
                return;
            }

            Bundle profileConfig = new Bundle();
            profileConfig.PutString(ExtraProfileName, AppOneProfileName);
            profileConfig.PutString(ExtraProfileEnabled, "true"); // Enable the profile
            profileConfig.PutString(ExtraConfigMode, "CREATE_IF_NOT_EXIST"); // Create if not existing, update if it does

            // Specify the application association for the profile
            Bundle appConfig = new Bundle();
            appConfig.PutString("PACKAGE_NAME", AppInfo.PackageName); // Associates with this app's package name
            appConfig.PutStringArray("ACTIVITY_LIST", new string[] { "*" }); // All activities in the app
            profileConfig.PutParcelableArray(ExtraAppList, new Bundle[] { appConfig });

            // Configure BARCODE plugin
            Bundle barcodeConfig = new Bundle();
            barcodeConfig.PutString("PLUGIN_NAME", "BARCODE");
            barcodeConfig.PutString("RESET_CONFIG", "true"); // Overwrite existing settings

            Bundle barcodeProps = new Bundle();
            barcodeProps.PutString("scanner_selection", "auto"); // Auto-select scanner
            barcodeProps.PutString("scanner_input_enabled", "true"); // Enable scanning
            // Enable specific decoders (example)
            barcodeProps.PutString("decoder_ean13", "true");
            barcodeProps.PutString("decoder_code128", "true");
            barcodeProps.PutString("decoder_qr", "true"); // Example: enabling QR code scanning
                                                          // Add other decoders as needed: e.g., decoder_datamatrix, decoder_code39 etc.

            barcodeConfig.PutBundle("PARAM_LIST", barcodeProps);

            // Configure INTENT plugin for output
            Bundle intentConfig = new Bundle();
            intentConfig.PutString("PLUGIN_NAME", "INTENT");
            intentConfig.PutString("RESET_CONFIG", "true");

            Bundle intentProps = new Bundle();
            intentProps.PutString("intent_output_enabled", "true");
            intentProps.PutString("intent_action", IntentActionScanOutput); // Custom action for DWIntentReceiver
            intentProps.PutInt("intent_delivery", 2); // "Send via startActivity" (Broadcast intent is 0, StartActivity is 1, StartService is 2 - check DW docs, usually 0 for broadcast)
                                                      // For BroadcastReceiver, intent_delivery should be 0 (Broadcast Intent)
            intentProps.PutString("intent_delivery", "0"); // 0 for Broadcast, 1 for StartActivity, 2 for StartService.
            intentConfig.PutBundle("PARAM_LIST", intentProps);


            // Add plugin configurations to the profile
            profileConfig.PutParcelableArray(ExtraPluginConfig, new Bundle[] { barcodeConfig, intentConfig });

            // Send the intent to create/update the profile
            SendDataWedgeIntentWithExtra(ActionDatawedgeApi, ExtraCreateProfile, profileConfig);

            // Optionally, set this profile as the default
            // SetDefaultProfile(AppOneProfileName); // Consider if this is always desired

            System.Diagnostics.Debug.WriteLine($"DataWedgeService: Profile '{AppOneProfileName}' creation/update intent sent.");
#endif
        }


        public void SetDefaultProfile(string profileName)
        {
#if ANDROID
            System.Diagnostics.Debug.WriteLine($"DataWedgeService: Setting default profile to: {profileName}");
            SendDataWedgeIntentWithExtra(ActionDatawedgeApi, ExtraSetDefaultProfile, profileName);
            System.Diagnostics.Debug.WriteLine($"DataWedgeService: Set default profile intent sent for '{profileName}'.");
#endif
        }

        public void EnableScanner()
        {
#if ANDROID
            System.Diagnostics.Debug.WriteLine("DataWedgeService: Enabling scanner.");
            // This command enables/disables scanning for the currently active profile.
            // Ensure AppOneProfileName is active if you want to control it specifically.
            // Alternatively, modify the profile's 'scanner_input_enabled' parameter.
            SendDataWedgeIntentWithExtra(ActionDatawedgeApi, ExtraScannerInputPlugin, "ENABLE_PLUGIN"); // This might be incorrect.
                                                                                                        // The correct way is usually to set PARAM_LIST for SCANNER_INPUT_PLUGIN

            Bundle scannerPluginConfig = new Bundle();
            scannerPluginConfig.PutString("PLUGIN_NAME", "BARCODE"); // Target the BARCODE plugin

            Bundle paramsBundle = new Bundle();
            paramsBundle.PutString("scanner_input_enabled", "true");
            scannerPluginConfig.PutBundle("PARAM_LIST", paramsBundle);

            Bundle profileModification = new Bundle();
            profileModification.PutString(ExtraProfileName, AppOneProfileName); // Specify the profile to modify
            profileModification.PutString(ExtraConfigMode, "UPDATE");      // Ensure we are updating
            profileModification.PutParcelableArray(ExtraPluginConfig, new Bundle[] { scannerPluginConfig });


            Intent intent = new Intent();
            intent.SetAction(ActionDatawedgeApi);
            // This should be a "SET_CONFIG" or similar action to modify a profile.
            // For simple enable/disable, often SOFT_SCAN_TRIGGER or modifying the profile is used.
            // Let's use SET_CONFIG to update the profile.
            intent.PutExtra("com.symbol.datawedge.api.SET_CONFIG", profileModification);

            Platform.AppContext.SendBroadcast(intent);
            System.Diagnostics.Debug.WriteLine("DataWedgeService: Enable scanner intent (via SET_CONFIG) sent.");
#endif
        }

        public void DisableScanner()
        {
#if ANDROID
            System.Diagnostics.Debug.WriteLine("DataWedgeService: Disabling scanner.");

            Bundle scannerPluginConfig = new Bundle();
            scannerPluginConfig.PutString("PLUGIN_NAME", "BARCODE");

            Bundle paramsBundle = new Bundle();
            paramsBundle.PutString("scanner_input_enabled", "false");
            scannerPluginConfig.PutBundle("PARAM_LIST", paramsBundle);

            Bundle profileModification = new Bundle();
            profileModification.PutString(ExtraProfileName, AppOneProfileName);
            profileModification.PutString(ExtraConfigMode, "UPDATE");
            profileModification.PutParcelableArray(ExtraPluginConfig, new Bundle[] { scannerPluginConfig });

            Intent intent = new Intent();
            intent.SetAction(ActionDatawedgeApi);
            intent.PutExtra("com.symbol.datawedge.api.SET_CONFIG", profileModification);

            Platform.AppContext.SendBroadcast(intent);
            System.Diagnostics.Debug.WriteLine("DataWedgeService: Disable scanner intent (via SET_CONFIG) sent.");
#endif
        }


        public void StartSoftScan()
        {
#if ANDROID
            System.Diagnostics.Debug.WriteLine("DataWedgeService: Starting soft scan.");
            SendDataWedgeCommand(ExtraSoftScanTrigger, "START_SCANNING");
            System.Diagnostics.Debug.WriteLine("DataWedgeService: Start soft scan intent sent.");
#endif
        }

        public void StopSoftScan()
        {
#if ANDROID
            System.Diagnostics.Debug.WriteLine("DataWedgeService: Stopping soft scan.");
            SendDataWedgeCommand(ExtraSoftScanTrigger, "STOP_SCANNING");
            System.Diagnostics.Debug.WriteLine("DataWedgeService: Stop soft scan intent sent.");
#endif
        }
    }
}