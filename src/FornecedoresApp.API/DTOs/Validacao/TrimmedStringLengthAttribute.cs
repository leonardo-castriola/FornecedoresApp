using System.ComponentModel.DataAnnotations;

namespace FornecedoresApp.API.DTOs.Validacao;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class TrimmedStringLengthAttribute(int minimumLength, int maximumLength, bool optional = false) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is not string text)
        {
            return new ValidationResult("O valor deve ser um texto.");
        }

        var normalized = text.Trim();
        if (optional && normalized.Length == 0)
        {
            return ValidationResult.Success;
        }

        return normalized.Length >= minimumLength && normalized.Length <= maximumLength
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage ?? $"O campo deve possuir entre {minimumLength} e {maximumLength} caracteres.");
    }
}
