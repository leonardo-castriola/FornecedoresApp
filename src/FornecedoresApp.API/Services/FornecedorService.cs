using System.ComponentModel.DataAnnotations;
using FornecedoresApp.API.DTOs;
using FornecedoresApp.API.Services.Exceptions;
using FornecedoresApp.Domain.Entities;
using FornecedoresApp.Domain.Interfaces;

namespace FornecedoresApp.API.Services;

public sealed class FornecedorService(IFornecedorRepository repository)
{
    public async Task<FornecedorResponse> CadastrarAsync(
        CadastrarFornecedorRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidarRequest(request);

        var fornecedor = request.ToEntity();
        if (await repository.ExisteCnpjAsync(fornecedor.Cnpj, cancellationToken))
        {
            throw new FornecedorDuplicadoException("CNPJ");
        }

        if (await repository.ExisteEmailAsync(fornecedor.Email, fornecedorIdIgnorado: null, cancellationToken))
        {
            throw new FornecedorDuplicadoException("email");
        }

        await repository.InserirAsync(fornecedor, cancellationToken);
        return fornecedor.ToResponse();
    }

    public async Task<IReadOnlyList<FornecedorResponse>> PesquisarAsync(
        string? razaoSocial,
        string? cnpj,
        bool? ativo,
        CancellationToken cancellationToken)
    {
        var cnpjNormalizado = string.IsNullOrWhiteSpace(cnpj) ? null : CnpjValidador.Normalizar(cnpj);

        var fornecedores = await repository.PesquisarAsync(
            razaoSocial,
            cnpjNormalizado,
            ativo,
            cancellationToken);

        return fornecedores.Select(fornecedor => fornecedor.ToResponse()).ToArray();
    }

    public async Task<FornecedorResponse> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var fornecedor = await repository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new FornecedorNaoEncontradoException(id);

        return fornecedor.ToResponse();
    }

    public async Task<FornecedorResponse> AtualizarAsync(
        Guid id,
        AtualizarFornecedorRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidarRequest(request);

        var fornecedor = await repository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new FornecedorNaoEncontradoException(id);

        var emailNormalizado = request.Email!.Trim();
        if (await repository.ExisteEmailAsync(emailNormalizado, id, cancellationToken))
        {
            throw new FornecedorDuplicadoException("email");
        }

        request.AtualizarEntidade(fornecedor);
        await repository.AtualizarAsync(fornecedor, cancellationToken);
        return fornecedor.ToResponse();
    }

    public async Task ExcluirAsync(Guid id, CancellationToken cancellationToken)
    {
        var fornecedor = await repository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new FornecedorNaoEncontradoException(id);

        if (!fornecedor.Ativo)
        {
            return;
        }

        fornecedor.Desativar();
        await repository.AtualizarAsync(fornecedor, cancellationToken);
    }

    private static void ValidarRequest<TRequest>(TRequest request)
        where TRequest : notnull
    {
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);
    }
}
