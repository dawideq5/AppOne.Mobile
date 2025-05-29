// Models/ValidationResponse.cs
namespace AppOne.Mobile.Models
{
    public class ValidationResponse
    {
        public bool IsValid { get; set; }
        public string? Message { get; set; } // Może być null
    }
}
