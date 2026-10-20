using FornecedoresApp.Domain.Entities;

namespace FornecedoresApp.API.DTOs;

public static class FornecedorDtoMapper
{
    public static Fornecedor ToEntity(this CadastrarFornecedorRequest request) =>
        Fornecedor.Criar(
            request.RazaoSocial,
            request.NomeFantasia,
            request.Cnpj,
            request.Email,
            request.Endereco,
            request.Telefone);

    public static Fornecedor AtualizarEntidade(this AtualizarFornecedorRequest request, Fornecedor fornecedor)
    {
        if (request.Ativo is null)
        {
            throw new ArgumentException("A situação do fornecedor é obrigatória.", nameof(request));
        }

        fornecedor.Atualizar(
            request.RazaoSocial,
            request.NomeFantasia,
            request.Email,
            request.Endereco,
            request.Telefone,
            request.Ativo.Value);

        return fornecedor;
    }

    public static FornecedorResponse ToResponse(this Fornecedor fornecedor) => new(
        fornecedor.Id,
        fornecedor.RazaoSocial,
        fornecedor.NomeFantasia,
        fornecedor.Cnpj,
        fornecedor.Email,
        fornecedor.Endereco,
        fornecedor.Telefone,
        fornecedor.Ativo,
        fornecedor.DataCadastro);
}
