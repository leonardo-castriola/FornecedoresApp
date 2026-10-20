using System.ComponentModel.DataAnnotations;

namespace FornecedoresApp.API.DTOs.Validacao;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class TelefoneFornecedorAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || value is not string telefone || string.IsNullOrWhiteSpace(telefone))
        {
            return ValidationResult.Success;
        }

        var normalized = telefone.Trim();
        var valido = normalized.Length is >= 10 and <= 20 && normalized.All(caractere =>
            char.IsAsciiDigit(caractere) || caractere is '+' or '-' or '(' or ')');

        return valido
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage ?? "O telefone deve possuir de 10 a 20 caracteres e conter somente números e +, -, ( ou ).");
    }
}
