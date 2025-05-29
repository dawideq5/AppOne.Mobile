// Services/MockApiClient.cs
using AppOne.Mobile.Interfaces;
using AppOne.Mobile.Models;
using datawedge_MAUI_SampleApp.Models;
using System;
using System.Threading.Tasks;

namespace AppOne.Mobile.Services
{
    public class MockApiClient : IApiClient
    {
        public async Task<ValidationResponse> ValidateCodeAsync(string code)
        {
            // Symulacja opóźnienia sieciowego
            await Task.Delay(TimeSpan.FromMilliseconds(300));

            if (string.IsNullOrWhiteSpace(code))
            {
                return new ValidationResponse { IsValid = false, Message = "Kod nie może być pusty." };
            }

            if (code.StartsWith("INVALID", StringComparison.OrdinalIgnoreCase))
            {
                return new ValidationResponse { IsValid = false, Message = "Kod nieprawidłowy (mock)." };
            }

            if (code.StartsWith("ERROR", StringComparison.OrdinalIgnoreCase))
            {
                // Symulacja błędu serwera lub wyjątku
                return new ValidationResponse { IsValid = false, Message = "Wystąpił błąd serwera (mock)." };
            }

            // Domyślnie kod jest prawidłowy
            return new ValidationResponse { IsValid = true, Message = "Kod prawidłowy (mock)." };
        }

        // Implementuj inne metody interfejsu IApiClient, jeśli istnieją
    }
}
