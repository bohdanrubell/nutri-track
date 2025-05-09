// API/Exceptions/ValidationException.cs
namespace NutriTrack.Exceptions;

public class ValidationException : ApplicationException
{
    public ValidationException(string message)
        : base(message)
    {
    }
    
    // Support for multiple validation errors
    public ValidationException(IEnumerable<string> errors) 
        : base("Виявлено помилки валідації")
    {
        ValidationErrors = errors.ToList();
    }
    
    // Property to hold multiple validation errors
    public List<string>? ValidationErrors { get; }
    
    // Helper to check if we have multiple errors
    public bool HasValidationErrors => ValidationErrors != null && ValidationErrors.Count > 0;
}