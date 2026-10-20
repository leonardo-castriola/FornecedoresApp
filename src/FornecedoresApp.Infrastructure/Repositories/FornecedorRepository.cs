using FornecedoresApp.Domain.Entities;
using FornecedoresApp.Domain.Interfaces;
using FornecedoresApp.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FornecedoresApp.Infrastructure.Repositories;

public sealed class FornecedorRepository(DataContext context) : IFornecedorRepository
{
    public async Task InserirAsync(Fornecedor fornecedor, CancellationToken cancellationToken)
    {
        await context.Fornecedores.AddAsync(fornecedor, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<Fornecedor?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Fornecedores.AsNoTracking().SingleOrDefaultAsync(fornecedor => fornecedor.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Fornecedor>> PesquisarAsync(
        string? razaoSocial,
        string? cnpj,
        bool? ativo,
        CancellationToken cancellationToken)
    {
        var consulta = context.Fornecedores.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(razaoSocial))
        {
            var termo = $"%{razaoSocial.Trim()}%";
            consulta = consulta.Where(fornecedor => EF.Functions.Like(fornecedor.RazaoSocial, termo));
        }

        if (!string.IsNullOrWhiteSpace(cnpj))
        {
            consulta = consulta.Where(fornecedor => fornecedor.Cnpj == cnpj);
        }

        if (ativo.HasValue)
        {
            consulta = consulta.Where(fornecedor => fornecedor.Ativo == ativo.Value);
        }

        return await consulta.OrderBy(fornecedor => fornecedor.RazaoSocial).ToListAsync(cancellationToken);
    }

    public Task<bool> ExisteCnpjAsync(string cnpj, CancellationToken cancellationToken) =>
        context.Fornecedores.AsNoTracking().AnyAsync(fornecedor => fornecedor.Cnpj == cnpj, cancellationToken);

    public Task<bool> ExisteEmailAsync(string email, Guid? fornecedorIdIgnorado, CancellationToken cancellationToken) =>
        context.Fornecedores.AsNoTracking().AnyAsync(
            fornecedor => fornecedor.Email == email &&
                          (!fornecedorIdIgnorado.HasValue || fornecedor.Id != fornecedorIdIgnorado.Value),
            cancellationToken);

    public async Task AtualizarAsync(Fornecedor fornecedor, CancellationToken cancellationToken)
    {
        context.Fornecedores.Update(fornecedor);
        await context.SaveChangesAsync(cancellationToken);
    }
}
