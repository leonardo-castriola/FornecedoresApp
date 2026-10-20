using FornecedoresApp.Domain.Entities;
using FornecedoresApp.Domain.Interfaces;

namespace FornecedoresApp.Tests.TestSupport;

internal sealed class FakeFornecedorRepository : IFornecedorRepository
{
    private readonly List<Fornecedor> _fornecedores = [];

    public IReadOnlyList<Fornecedor> Fornecedores => _fornecedores;

    public Task InserirAsync(Fornecedor fornecedor, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _fornecedores.Add(fornecedor);
        return Task.CompletedTask;
    }

    public Task<Fornecedor?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_fornecedores.SingleOrDefault(fornecedor => fornecedor.Id == id));
    }

    public Task<IReadOnlyList<Fornecedor>> PesquisarAsync(
        string? razaoSocial,
        string? cnpj,
        bool? ativo,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IEnumerable<Fornecedor> resultado = _fornecedores;

        if (!string.IsNullOrWhiteSpace(razaoSocial))
        {
            resultado = resultado.Where(fornecedor => fornecedor.RazaoSocial.Contains(
                razaoSocial.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(cnpj))
        {
            resultado = resultado.Where(fornecedor => fornecedor.Cnpj == cnpj);
        }

        if (ativo.HasValue)
        {
            resultado = resultado.Where(fornecedor => fornecedor.Ativo == ativo.Value);
        }

        return Task.FromResult<IReadOnlyList<Fornecedor>>(resultado
            .OrderBy(fornecedor => fornecedor.RazaoSocial)
            .ToArray());
    }

    public Task<bool> ExisteCnpjAsync(string cnpj, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_fornecedores.Any(fornecedor => fornecedor.Cnpj == cnpj));
    }

    public Task<bool> ExisteEmailAsync(string email, Guid? fornecedorIdIgnorado, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_fornecedores.Any(fornecedor =>
            fornecedor.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
            fornecedor.Id != fornecedorIdIgnorado));
    }

    public Task AtualizarAsync(Fornecedor fornecedor, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var indice = _fornecedores.FindIndex(item => item.Id == fornecedor.Id);
        if (indice < 0)
        {
            _fornecedores.Add(fornecedor);
        }
        else
        {
            _fornecedores[indice] = fornecedor;
        }

        return Task.CompletedTask;
    }
}
