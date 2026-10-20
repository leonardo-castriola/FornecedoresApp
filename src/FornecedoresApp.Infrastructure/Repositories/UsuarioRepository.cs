using FornecedoresApp.Domain.Entities;
using FornecedoresApp.Domain.Interfaces;
using FornecedoresApp.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FornecedoresApp.Infrastructure.Repositories;

public sealed class UsuarioRepository(DataContext context) : IUsuarioRepository
{
    public async Task InserirAsync(Usuario usuario, CancellationToken cancellationToken)
    {
        await context.Usuarios.AddAsync(usuario, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<Usuario?> ObterPorLoginAsync(string login, CancellationToken cancellationToken) =>
        context.Usuarios.AsNoTracking().SingleOrDefaultAsync(usuario => usuario.Login == login, cancellationToken);
}
