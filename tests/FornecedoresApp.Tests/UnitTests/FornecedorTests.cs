using FornecedoresApp.Domain.Entities;
using FornecedoresApp.Domain.Exceptions;
using FornecedoresApp.Tests.TestSupport;

namespace FornecedoresApp.Tests.UnitTests;

public sealed class FornecedorTests
{
    [Fact]
    public void DeveCriarFornecedorComDadosValidos()
    {
        var fornecedor = CriarFornecedor();

        Assert.NotEqual(Guid.Empty, fornecedor.Id);
        Assert.True(fornecedor.Ativo);
        Assert.Equal(DateTimeKind.Utc, fornecedor.DataCadastro.Kind);
    }

    [Fact]
    public void DeveRejeitarRazaoSocialVazia() =>
        Assert.Throws<RegraDeNegocioException>(() => Fornecedor.Criar(" ", null, TestData.NovoCnpj(), "a@b.com", "Rua válida", null));

    [Fact]
    public void DeveRejeitarRazaoSocialMuitoCurta() =>
        Assert.Throws<RegraDeNegocioException>(() => Fornecedor.Criar("AB", null, TestData.NovoCnpj(), "a@b.com", "Rua válida", null));

    [Fact]
    public void DeveRejeitarCnpjInvalido() =>
        Assert.Throws<RegraDeNegocioException>(() => Fornecedor.Criar("Empresa válida", null, "11111111111111", "a@b.com", "Rua válida", null));

    [Fact]
    public void DeveNormalizarCnpjFormatado()
    {
        var cnpj = TestData.NovoCnpj();
        var formatado = $"{cnpj[..2]}.{cnpj[2..5]}.{cnpj[5..8]}/{cnpj[8..12]}-{cnpj[12..]}";

        var fornecedor = Fornecedor.Criar("Empresa válida", null, formatado, "a@b.com", "Rua válida", null);

        Assert.Equal(cnpj, fornecedor.Cnpj);
    }

    [Fact]
    public void DeveAtualizarDadosPermitidos()
    {
        var fornecedor = CriarFornecedor();

        fornecedor.Atualizar("Nova razão", "Nova fantasia", "novo@exemplo.com", "Novo endereço", "11999999999", true);

        Assert.Equal("Nova razão", fornecedor.RazaoSocial);
        Assert.Equal("Novo endereço", fornecedor.Endereco);
        Assert.Equal("novo@exemplo.com", fornecedor.Email);
    }

    [Fact]
    public void DeveDesativarFornecedor()
    {
        var fornecedor = CriarFornecedor();

        fornecedor.Desativar();

        Assert.False(fornecedor.Ativo);
    }

    [Fact]
    public void DevePreservarCnpjNaAtualizacao()
    {
        var fornecedor = CriarFornecedor();
        var cnpj = fornecedor.Cnpj;

        fornecedor.Atualizar("Nova razão", null, "novo@exemplo.com", "Novo endereço", null, true);

        Assert.Equal(cnpj, fornecedor.Cnpj);
    }

    [Fact]
    public void DeveRejeitarEnderecoVazio() =>
        Assert.Throws<RegraDeNegocioException>(() => Fornecedor.Criar("Empresa válida", null, TestData.NovoCnpj(), "a@b.com", "  ", null));

    [Fact]
    public void DeveRejeitarEnderecoMuitoCurto() =>
        Assert.Throws<RegraDeNegocioException>(() => Fornecedor.Criar("Empresa válida", null, TestData.NovoCnpj(), "a@b.com", "AB", null));

    [Fact]
    public void DeveRejeitarEnderecoMuitoLongo() =>
        Assert.Throws<RegraDeNegocioException>(() => Fornecedor.Criar("Empresa válida", null, TestData.NovoCnpj(), "a@b.com", new string('A', 251), null));

    [Fact]
    public void DeveRemoverEspacosExternosDoEndereco()
    {
        var fornecedor = Fornecedor.Criar("Empresa válida", null, TestData.NovoCnpj(), "a@b.com", "  Rua de Teste  ", null);

        Assert.Equal("Rua de Teste", fornecedor.Endereco);
    }

    [Fact]
    public void DeveRejeitarEmailInvalido() =>
        Assert.Throws<RegraDeNegocioException>(() => Fornecedor.Criar("Empresa válida", null, TestData.NovoCnpj(), "email-invalido", "Rua válida", null));

    private static Fornecedor CriarFornecedor() => Fornecedor.Criar(
        "Empresa de Teste Ltda",
        "Empresa Teste",
        TestData.NovoCnpj(),
        "fornecedor@exemplo.com",
        "Rua de Teste, 123",
        "11999999999");
}
