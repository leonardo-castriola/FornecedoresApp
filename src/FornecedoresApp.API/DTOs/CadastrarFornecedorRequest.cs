using System.ComponentModel.DataAnnotations;
using FornecedoresApp.API.DTOs.Validacao;

namespace FornecedoresApp.API.DTOs;

public sealed class CadastrarFornecedorRequest
{
    [Required(ErrorMessage = "A razão social é obrigatória.")]
    [TrimmedStringLength(3, 150)]
    public string? RazaoSocial { get; init; }

    [TrimmedStringLength(3, 150, optional: true)]
    public string? NomeFantasia { get; init; }

    [Required(ErrorMessage = "O CNPJ é obrigatório.")]
    [CnpjValido]
    public string? Cnpj { get; init; }

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailFornecedor]
    public string? Email { get; init; }

    [Required(ErrorMessage = "O endereço é obrigatório.")]
    [TrimmedStringLength(3, 250)]
    public string? Endereco { get; init; }

    [TelefoneFornecedor]
    public string? Telefone { get; init; }
}
