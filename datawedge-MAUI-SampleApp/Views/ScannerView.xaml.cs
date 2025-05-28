// Typ pliku: Plik C# (code-behind)
// Lokalizacja: datawedge_MAUI_SampleApp/Views/ScannerView.xaml.cs
using datawedge_MAUI_SampleApp.ViewModels;
// POPRAWKA: Upewnij siê, ¿e ta przestrzeñ nazw jest poprawna i zawiera BarcodeScannedMessage
using datawedge_MAUI_SampleApp.Messaging;
using CommunityToolkit.Mvvm.Messaging;

namespace datawedge_MAUI_SampleApp.Views;

public partial class ScannerView : ContentPage, IRecipient<BarcodeScannedMessage> // Upewnij siê, ¿e BarcodeScannedMessage jest rozpoznawalne
{
    private readonly ScannerViewModel _viewModel;

    public ScannerView(ScannerViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        WeakReferenceMessenger.Default.Register(this);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public void Receive(BarcodeScannedMessage message) // Upewnij siê, ¿e BarcodeScannedMessage jest rozpoznawalne
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (_viewModel.ProcessScannedCodeCommand.CanExecute(message.Value))
            {
                await _viewModel.ProcessScannedCodeCommand.ExecuteAsync(message.Value);
            }
        });
    }
}
