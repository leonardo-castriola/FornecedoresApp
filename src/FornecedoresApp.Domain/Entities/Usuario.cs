using FornecedoresApp.Domain.Exceptions;

namespace FornecedoresApp.Domain.Entities;

public sealed class Usuario
{
    private const int TamanhoMaximoNome = 100;
    private const int TamanhoMaximoLogin = 100;
    private const int TamanhoMaximoSenhaHash = 500;

    private Usuario()
    {
    }

    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Login { get; private set; } = string.Empty;
    public string SenhaHash { get; private set; } = string.Empty;
    public bool Ativo { get; private set; }
    public DateTime DataCadastro { get; private set; }

    public static Usuario Criar(string? nome, string? login, string? senhaHash)
    {
        var nomeNormalizado = nome?.Trim();
        var loginNormalizado = login?.Trim();
        var senhaHashNormalizado = senhaHash?.Trim();

        if (string.IsNullOrWhiteSpace(nomeNormalizado) || nomeNormalizado.Length > TamanhoMaximoNome)
        {
            throw new RegraDeNegocioException($"O nome do usuário é obrigatório e deve possuir no máximo {TamanhoMaximoNome} caracteres.");
        }

        if (string.IsNullOrWhiteSpace(loginNormalizado) || loginNormalizado.Length > TamanhoMaximoLogin)
        {
            throw new RegraDeNegocioException($"O login é obrigatório e deve possuir no máximo {TamanhoMaximoLogin} caracteres.");
        }

        if (string.IsNullOrWhiteSpace(senhaHashNormalizado) || senhaHashNormalizado.Length > TamanhoMaximoSenhaHash)
        {
            throw new RegraDeNegocioException($"O hash da senha é obrigatório e deve possuir no máximo {TamanhoMaximoSenhaHash} caracteres.");
        }

        return new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = nomeNormalizado,
            Login = loginNormalizado,
            SenhaHash = senhaHashNormalizado,
            Ativo = true,
            DataCadastro = DateTime.UtcNow
        };
    }

    public void Desativar()
    {
        Ativo = false;
    }
}
