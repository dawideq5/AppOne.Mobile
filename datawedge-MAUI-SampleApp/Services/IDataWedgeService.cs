// Lokalizacja: datawedge-MAUI-SampleApp/Services/IDataWedgeService.cs
#nullable enable

namespace datawedge_MAUI_SampleApp.Services
{
    /// <summary>
    /// Interfejs dla usługi DataWedge.
    /// </summary>
    public interface IDataWedgeService
    {
        /// <summary>
        /// Inicjalizuje usługę, np. tworząc profil DataWedge.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Włącza lub wyłącza skaner DataWedge.
        /// </summary>
        /// <param name="enable">True, aby włączyć, false, aby wyłączyć.</param>
        void EnableScanner(bool enable);

        /// <summary>
        /// Uruchamia programowy przycisk skanowania.
        /// </summary>
        /// <param name="start">True, aby rozpocząć skanowanie, false, aby zatrzymać.</param>
        void SoftScanTrigger(bool start);
    }
}