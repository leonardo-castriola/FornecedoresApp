using FornecedoresApp.Domain.Entities;

namespace FornecedoresApp.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task InserirAsync(Usuario usuario, CancellationToken cancellationToken);

    Task<Usuario?> ObterPorLoginAsync(string login, CancellationToken cancellationToken);
}
