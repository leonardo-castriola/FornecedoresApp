using System.ComponentModel.DataAnnotations;
using FornecedoresApp.Domain.Exceptions;

namespace FornecedoresApp.Domain.Entities;

public sealed class Fornecedor
{
    private const int TamanhoMinimoNome = 3;
    private const int TamanhoMaximoNome = 150;
    private const int TamanhoMinimoEndereco = 3;
    private const int TamanhoMaximoEndereco = 250;
    private const int TamanhoMaximoEmail = 150;
    private const int TamanhoMinimoTelefone = 10;
    private const int TamanhoMaximoTelefone = 20;

    private Fornecedor()
    {
    }

    public Guid Id { get; private set; }
    public string RazaoSocial { get; private set; } = string.Empty;
    public string? NomeFantasia { get; private set; }
    public string Cnpj { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Endereco { get; private set; } = string.Empty;
    public string? Telefone { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime DataCadastro { get; private set; }

    public static Fornecedor Criar(
        string? razaoSocial,
        string? nomeFantasia,
        string? cnpj,
        string? email,
        string? endereco,
        string? telefone)
    {
        var fornecedor = new Fornecedor
        {
            Id = Guid.NewGuid(),
            Cnpj = CnpjValidador.ValidarENormalizar(cnpj),
            Ativo = true,
            DataCadastro = DateTime.UtcNow
        };

        fornecedor.DefinirDadosPermitidos(razaoSocial, nomeFantasia, email, endereco, telefone);
        return fornecedor;
    }

    public void Atualizar(
        string? razaoSocial,
        string? nomeFantasia,
        string? email,
        string? endereco,
        string? telefone,
        bool ativo)
    {
        DefinirDadosPermitidos(razaoSocial, nomeFantasia, email, endereco, telefone);
        Ativo = ativo;
    }

    public void Desativar()
    {
        Ativo = false;
    }

    private void DefinirDadosPermitidos(
        string? razaoSocial,
        string? nomeFantasia,
        string? email,
        string? endereco,
        string? telefone)
    {
        var razaoSocialValidada = ValidarTextoObrigatorio(razaoSocial, "Razão Social", TamanhoMinimoNome, TamanhoMaximoNome);
        var nomeFantasiaValidado = ValidarTextoOpcional(nomeFantasia, "Nome Fantasia", TamanhoMinimoNome, TamanhoMaximoNome);
        var emailValidado = ValidarEmail(email);
        var enderecoValidado = ValidarTextoObrigatorio(endereco, "Endereço", TamanhoMinimoEndereco, TamanhoMaximoEndereco);
        var telefoneValidado = ValidarTelefone(telefone);

        RazaoSocial = razaoSocialValidada;
        NomeFantasia = nomeFantasiaValidado;
        Email = emailValidado;
        Endereco = enderecoValidado;
        Telefone = telefoneValidado;
    }

    private static string ValidarTextoObrigatorio(string? valor, string campo, int tamanhoMinimo, int tamanhoMaximo)
    {
        var valorNormalizado = valor?.Trim();

        if (string.IsNullOrWhiteSpace(valorNormalizado))
        {
            throw new RegraDeNegocioException($"O campo {campo} é obrigatório.");
        }

        if (valorNormalizado.Length < tamanhoMinimo || valorNormalizado.Length > tamanhoMaximo)
        {
            throw new RegraDeNegocioException($"{campo} deve possuir entre {tamanhoMinimo} e {tamanhoMaximo} caracteres.");
        }

        return valorNormalizado;
    }

    private static string? ValidarTextoOpcional(string? valor, string campo, int tamanhoMinimo, int tamanhoMaximo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return null;
        }

        return ValidarTextoObrigatorio(valor, campo, tamanhoMinimo, tamanhoMaximo);
    }

    private static string ValidarEmail(string? email)
    {
        var emailNormalizado = email?.Trim();

        if (string.IsNullOrWhiteSpace(emailNormalizado))
        {
            throw new RegraDeNegocioException("O email é obrigatório.");
        }

        if (emailNormalizado.Length > TamanhoMaximoEmail)
        {
            throw new RegraDeNegocioException($"O email deve possuir no máximo {TamanhoMaximoEmail} caracteres.");
        }

        if (!new EmailAddressAttribute().IsValid(emailNormalizado))
        {
            throw new RegraDeNegocioException("O email informado é inválido.");
        }

        return emailNormalizado;
    }

    private static string? ValidarTelefone(string? telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
        {
            return null;
        }

        var telefoneNormalizado = telefone.Trim();

        if (telefoneNormalizado.Length < TamanhoMinimoTelefone || telefoneNormalizado.Length > TamanhoMaximoTelefone)
        {
            throw new RegraDeNegocioException($"O telefone deve possuir entre {TamanhoMinimoTelefone} e {TamanhoMaximoTelefone} caracteres.");
        }

        if (telefoneNormalizado.Any(caractere =>
                !char.IsAsciiDigit(caractere) && caractere is not '+' and not '-' and not '(' and not ')'))
        {
            throw new RegraDeNegocioException("O telefone pode conter somente números e os caracteres +, -, ( e ).");
        }

        return telefoneNormalizado;
    }
}
