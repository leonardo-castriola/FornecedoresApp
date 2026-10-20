using System.ComponentModel.DataAnnotations;
using FornecedoresApp.Domain.Entities;
using FornecedoresApp.Domain.Exceptions;

namespace FornecedoresApp.API.DTOs.Validacao;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class CnpjValidoAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || value is not string cnpj || string.IsNullOrWhiteSpace(cnpj))
        {
            return ValidationResult.Success;
        }

        try
        {
            CnpjValidador.ValidarENormalizar(cnpj);
            return ValidationResult.Success;
        }
        catch (RegraDeNegocioException)
        {
            return new ValidationResult(ErrorMessage ?? "O CNPJ informado é inválido.");
        }
    }
}
