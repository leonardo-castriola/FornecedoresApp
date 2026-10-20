using FornecedoresApp.Domain.Entities;
using FornecedoresApp.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FornecedoresApp.API.Initialization;

public sealed class DevelopmentUserInitializer(
    IUsuarioRepository usuarioRepository,
    IPasswordHasher<Usuario> passwordHasher,
    IConfiguration configuration,
    ILogger<DevelopmentUserInitializer> logger)
{
    private const string LoginInicial = "admin";
    private const string NomeInicial = "Administrador";
    private const string HashTemporario = "__hash_temporario_apenas_em_memoria__";

    public async Task InicializarAsync(CancellationToken cancellationToken = default)
    {
        var usuarioExistente = await usuarioRepository.ObterPorLoginAsync(LoginInicial, cancellationToken);
        if (usuarioExistente is not null)
        {
            logger.LogInformation("O usuário inicial de desenvolvimento já existe; nenhuma alteração foi feita.");
            return;
        }

        var senha = configuration["DevelopmentUser:Password"];
        if (string.IsNullOrWhiteSpace(senha))
        {
            throw new InvalidOperationException(
                "Defina a configuração DevelopmentUser:Password em User Secrets ou em uma variável de ambiente antes de iniciar a API pela primeira vez em Development.");
        }

        // Esta entidade transitória fornece o contexto exigido pelo hasher e nunca é persistida.
        var usuarioParaHash = Usuario.Criar(NomeInicial, LoginInicial, HashTemporario);
        var senhaHash = passwordHasher.HashPassword(usuarioParaHash, senha);
        var usuarioInicial = Usuario.Criar(NomeInicial, LoginInicial, senhaHash);

        await usuarioRepository.InserirAsync(usuarioInicial, cancellationToken);
        logger.LogInformation("Usuário inicial de desenvolvimento criado.");
    }
}
