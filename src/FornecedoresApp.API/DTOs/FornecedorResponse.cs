namespace FornecedoresApp.API.DTOs;

public sealed record FornecedorResponse(
    Guid Id,
    string RazaoSocial,
    string? NomeFantasia,
    string Cnpj,
    string Email,
    string Endereco,
    string? Telefone,
    bool Ativo,
    DateTime DataCadastro);
