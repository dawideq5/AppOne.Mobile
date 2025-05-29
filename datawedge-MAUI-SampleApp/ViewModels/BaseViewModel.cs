// ViewModels/BaseViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using IntelliJ.Lang.Annotations;

// Zmieniona przestrzeń nazw, aby pasowała do reszty projektu
namespace datawedge_MAUI_SampleApp.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title = string.Empty;

        public bool IsNotBusy => !IsBusy;

        // Usunięto implementację OnIsBusyChanged z BaseViewModel,
        // ponieważ LoginViewModel dostarcza specyficzną implementację.
        // Generator kodu z [ObservableProperty] dla 'isBusy'
        // stworzy deklarację partial void OnIsBusyChanged(bool value);
        // oraz partial void OnIsBusyChanged(bool oldValue, bool newValue);
        // które mogą być zaimplementowane w klasach pochodnych.
    }
}