// Interfaces/IDataWedgeService.cs
namespace AppOne.Mobile.Interfaces // Zmieniono namespace na Interfaces dla spójności
{
    public interface IDataWedgeService
    {
        void Init();
        void EnableScanning();
        void DisableScanning();
        // Możesz dodać zdarzenie lub metodę zwrotną do przekazywania zeskanowanych danych
        // event EventHandler<string> BarcodeScanned; // Przykład
    }
}
