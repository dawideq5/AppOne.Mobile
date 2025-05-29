// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI-SampleApp/Services/DataWedgeService.cs
#if ANDROID
using Android.Content;
using Android.OS;
#endif
using System;
using datawedge_MAUI_SampleApp.Interfaces;
using datawedge_MAUI_SampleApp.Platforms.Android;
using datawedge_MAUI_SampleApp.Messaging;      // Poprawka CS0234: Upewnij się, że Messaging istnieje i zawiera BarcodeScannedMessage
using Microsoft.Maui.ApplicationModel;
using CommunityToolkit.Mvvm.Messaging;
using AppOne.Mobile.Messaging;

namespace datawedge_MAUI_SampleApp.Services
{
    public class DataWedgeService : Interfaces.IDataWedgeService
    {
        private const string ActionDatawedge = "com.symbol.datawedge.api.ACTION";
        private const string ExtraSetDefaultProfile = "com.symbol.datawedge.api.SET_DEFAULT_PROFILE";
        private const string ExtraCreateProfile = "com.symbol.datawedge.api.CREATE_PROFILE";
        private const string ExtraSoftScanTrigger = "com.symbol.datawedge.api.SOFT_SCAN_TRIGGER";
        private const string ExtraScannerInputPlugin = "com.symbol.datawedge.api.SCANNER_INPUT_PLUGIN";
        private const string ProfileName = "AppOneMAUIProfile";

        public DataWedgeService()
        {
            WeakReferenceMessenger.Default.Register<BarcodeScannedMessage>(this, (r, m) =>
            {
                System.Diagnostics.Debug.WriteLine($"DataWedgeService received scan: {m.Barcode} ({m.Symbology})");
            });
        }

        public void InitializeDataWedge()
        {
#if ANDROID
            CreateProfile();
#endif
        }

        private void SendDataWedgeIntent(Bundle extras)
        {
#if ANDROID
            var context = Platform.AppContext;
            if (context != null)
            {
                Intent intent = new Intent();
                intent.SetAction(ActionDatawedge);
                intent.PutExtras(extras);
                context.SendBroadcast(intent);
            }
#endif
        }

        public void CreateProfile()
        {
#if ANDROID
            System.Diagnostics.Debug.WriteLine($"Attempting to create DataWedge profile: {ProfileName}");
            Bundle profileConfig = new Bundle();
            profileConfig.PutString("PROFILE_NAME", ProfileName);
            profileConfig.PutString("PROFILE_ENABLED", "true");
            profileConfig.PutString("CONFIG_MODE", "CREATE_IF_NOT_EXIST");

            Bundle appConfig = new Bundle();
            appConfig.PutString("PACKAGE_NAME", AppInfo.PackageName);
            appConfig.PutStringArray("ACTIVITY_LIST", new string[] { "*" });
            profileConfig.PutParcelableArray("APP_LIST", new Bundle[] { appConfig });

            Bundle intentConfig = new Bundle();
            intentConfig.PutString("PLUGIN_NAME", "INTENT");
            intentConfig.PutString("RESET_CONFIG", "true");

            Bundle intentProps = new Bundle();
            intentProps.PutString("intent_output_enabled", "true");
            intentProps.PutString("intent_action", DWIntentReceiver.IntentActionScan);
            intentProps.PutInt("intent_delivery", 2);
            intentConfig.PutBundle("PARAM_LIST", intentProps);

            Bundle barcodeConfig = new Bundle();
            barcodeConfig.PutString("PLUGIN_NAME", "BARCODE");
            barcodeConfig.PutString("RESET_CONFIG", "true");

            Bundle barcodeProps = new Bundle();
            barcodeProps.PutString("scanner_selection", "auto");
            barcodeProps.PutString("decoder_ean13", "true");
            barcodeProps.PutString("decoder_code128", "true");
            barcodeConfig.PutBundle("PARAM_LIST", barcodeProps);

            profileConfig.PutParcelableArray("PLUGIN_CONFIG", new Bundle[] { intentConfig, barcodeConfig });

            SendDataWedgeIntent(profileConfig);
            System.Diagnostics.Debug.WriteLine($"DataWedge profile '{ProfileName}' creation/update intent sent.");
#endif
        }

        public void SetDefaultProfile(string profileName)
        {
#if ANDROID
            Bundle extras = new Bundle();
            extras.PutString(ExtraSetDefaultProfile, profileName);
            SendDataWedgeIntent(extras);
            System.Diagnostics.Debug.WriteLine($"DataWedge set default profile intent sent for: {profileName}");
#endif
        }

        public void EnableScanner()
        {
#if ANDROID
            Bundle extras = new Bundle();
            extras.PutString("PROFILE_NAME", ProfileName);
            Bundle pluginConfig = new Bundle();
            pluginConfig.PutString("PLUGIN_NAME", ExtraScannerInputPlugin);
            pluginConfig.PutString("RESET_CONFIG", "false");
            Bundle paramsBundle = new Bundle();
            paramsBundle.PutString("scanner_input_enabled", "true");
            pluginConfig.PutBundle("PARAM_LIST", paramsBundle);
            extras.PutBundle("PLUGIN_CONFIG", pluginConfig);
            SendDataWedgeIntent(extras);
            System.Diagnostics.Debug.WriteLine("DataWedge enable scanner intent sent.");
#endif
        }

        public void DisableScanner()
        {
#if ANDROID
            Bundle extras = new Bundle();
            extras.PutString("PROFILE_NAME", ProfileName);
            Bundle pluginConfig = new Bundle();
            pluginConfig.PutString("PLUGIN_NAME", ExtraScannerInputPlugin);
            pluginConfig.PutString("RESET_CONFIG", "false");
            Bundle paramsBundle = new Bundle();
            paramsBundle.PutString("scanner_input_enabled", "false");
            pluginConfig.PutBundle("PARAM_LIST", paramsBundle);
            extras.PutBundle("PLUGIN_CONFIG", pluginConfig);
            SendDataWedgeIntent(extras);
            System.Diagnostics.Debug.WriteLine("DataWedge disable scanner intent sent.");
#endif
        }

        public void StartSoftScan()
        {
#if ANDROID
            Bundle extras = new Bundle();
            extras.PutString(ExtraSoftScanTrigger, "START_SCANNING");
            SendDataWedgeIntent(extras);
            System.Diagnostics.Debug.WriteLine("DataWedge start soft scan intent sent.");
#endif
        }

        public void StopSoftScan()
        {
#if ANDROID
            Bundle extras = new Bundle();
            extras.PutString(ExtraSoftScanTrigger, "STOP_SCANNING");
            SendDataWedgeIntent(extras);
            System.Diagnostics.Debug.WriteLine("DataWedge stop soft scan intent sent.");
#endif
        }
    }
}
