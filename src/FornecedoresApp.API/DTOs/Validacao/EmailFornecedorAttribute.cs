using System.ComponentModel.DataAnnotations;

namespace FornecedoresApp.API.DTOs.Validacao;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class EmailFornecedorAttribute : ValidationAttribute
{
    private readonly EmailAddressAttribute _emailAddress = new();

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || value is not string email || string.IsNullOrWhiteSpace(email))
        {
            return ValidationResult.Success;
        }

        var normalized = email.Trim();
        return normalized.Length <= 150 && _emailAddress.IsValid(normalized)
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage ?? "O email informado é inválido ou excede 150 caracteres.");
    }
}
