using FornecedoresApp.Domain.Entities;

namespace FornecedoresApp.Domain.Interfaces;

public interface IFornecedorRepository
{
    Task InserirAsync(Fornecedor fornecedor, CancellationToken cancellationToken);

    Task<Fornecedor?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Fornecedor>> PesquisarAsync(
        string? razaoSocial,
        string? cnpj,
        bool? ativo,
        CancellationToken cancellationToken);

    Task<bool> ExisteCnpjAsync(string cnpj, CancellationToken cancellationToken);

    Task<bool> ExisteEmailAsync(
        string email,
        Guid? fornecedorIdIgnorado,
        CancellationToken cancellationToken);

    Task AtualizarAsync(Fornecedor fornecedor, CancellationToken cancellationToken);
}
