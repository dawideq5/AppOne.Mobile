// Path: dawideq5/appone.mobile/AppOne.Mobile-364202b6b5699d684b43b2b633ebce2e4ea9dbf7/datawedge-MAUI_SampleApp/Interfaces/IDataWedgeService.cs
namespace datawedge_MAUI_SampleApp.Interfaces // Upewnij się, że to jest JEDYNA definicja tego interfejsu
{
    public interface IDataWedgeService
    {
        void InitializeDataWedge();
        void CreateProfile();
        void SetDefaultProfile(string profileName);
        void EnableScanner();
        void DisableScanner();
        void StartSoftScan();
        void StopSoftScan();
    }
}
