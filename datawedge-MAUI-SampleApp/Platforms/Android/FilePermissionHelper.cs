// Platforms/Android/FilePermissionHelper.cs
// Ten plik wydaje się być specyficzny dla Twojego projektu,
// jeśli nie jest używany, można go pominąć.
// Upewnij się, że przestrzeń nazw jest poprawna.
namespace AppOne.Mobile.Platforms.Android // Zmieniono namespace
{
    public static class FilePermissionHelper
    {
        public static async Task<PermissionStatus> CheckAndRequestStoragePermission()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (status == PermissionStatus.Granted)
                return status;

            if (Permissions.ShouldShowRationale<Permissions.StorageWrite>())
            {
                // Możesz pokazać użytkownikowi wyjaśnienie, dlaczego potrzebujesz tego uprawnienia
                // await Shell.Current.DisplayAlert("Uprawnienia", "Potrzebujemy dostępu do pamięci, aby zapisać pliki.", "OK");
            }

            status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            return status;
        }
    }
}
