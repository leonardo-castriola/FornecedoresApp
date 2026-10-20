using FornecedoresApp.API.Services;
using FornecedoresApp.API.Services.Exceptions;
using FornecedoresApp.Domain.Entities;
using FornecedoresApp.Tests.TestSupport;

namespace FornecedoresApp.Tests.UnitTests;

public sealed class FornecedorServiceTests
{
    private readonly FakeFornecedorRepository _repository = new();
    private readonly FornecedorService _service;

    public FornecedorServiceTests() => _service = new FornecedorService(_repository);

    [Fact]
    public async Task DeveCadastrarFornecedorComSucesso()
    {
        var request = TestData.NovoCadastro();

        var response = await _service.CadastrarAsync(request, CancellationToken.None);

        Assert.Single(_repository.Fornecedores);
        Assert.Equal(request.Cnpj, response.Cnpj);
        Assert.True(response.Ativo);
    }

    [Fact]
    public async Task DeveRejeitarFornecedorComCnpjDuplicado()
    {
        var request = TestData.NovoCadastro();
        await _service.CadastrarAsync(request, CancellationToken.None);

        await Assert.ThrowsAsync<FornecedorDuplicadoException>(() =>
            _service.CadastrarAsync(TestData.NovoCadastro(cnpj: request.Cnpj), CancellationToken.None));

        Assert.Single(_repository.Fornecedores);
    }

    [Fact]
    public async Task DeveRejeitarFornecedorComEmailDuplicado()
    {
        const string email = "unico@exemplo.com";
        await _service.CadastrarAsync(TestData.NovoCadastro(email: email), CancellationToken.None);

        await Assert.ThrowsAsync<FornecedorDuplicadoException>(() =>
            _service.CadastrarAsync(TestData.NovoCadastro(email: email), CancellationToken.None));

        Assert.Single(_repository.Fornecedores);
    }

    [Fact]
    public async Task DeveRejeitarEmailDuplicadoSemDiferenciarMaiusculas()
    {
        await _service.CadastrarAsync(TestData.NovoCadastro(email: "contato@exemplo.com"), CancellationToken.None);

        await Assert.ThrowsAsync<FornecedorDuplicadoException>(() =>
            _service.CadastrarAsync(TestData.NovoCadastro(email: "CONTATO@EXEMPLO.COM"), CancellationToken.None));
    }

    [Fact]
    public async Task DeveConsultarFornecedorPorId()
    {
        var esperado = await _service.CadastrarAsync(TestData.NovoCadastro(), CancellationToken.None);

        var obtido = await _service.ObterPorIdAsync(esperado.Id, CancellationToken.None);

        Assert.Equal(esperado.Id, obtido.Id);
        Assert.Equal(esperado.Email, obtido.Email);
    }

    [Fact]
    public async Task DeveRetornarNaoEncontradoParaIdInexistente() =>
        await Assert.ThrowsAsync<FornecedorNaoEncontradoException>(() =>
            _service.ObterPorIdAsync(Guid.NewGuid(), CancellationToken.None));

    [Fact]
    public async Task DevePesquisarFornecedoresPorRazaoSocial()
    {
        await _service.CadastrarAsync(TestData.NovoCadastro(razaoSocial: "Alfa Comércio Ltda"), CancellationToken.None);
        await _service.CadastrarAsync(TestData.NovoCadastro(razaoSocial: "Beta Serviços Ltda"), CancellationToken.None);

        var encontrados = await _service.PesquisarAsync("alfa", null, null, CancellationToken.None);

        Assert.Single(encontrados);
        Assert.Contains("Alfa", encontrados[0].RazaoSocial);
    }

    [Fact]
    public async Task DevePesquisarFornecedoresPorSituacao()
    {
        var cadastrado = await _service.CadastrarAsync(TestData.NovoCadastro(), CancellationToken.None);
        var entidade = await _repository.ObterPorIdAsync(cadastrado.Id, CancellationToken.None);
        entidade!.Desativar();

        var encontrados = await _service.PesquisarAsync(null, null, false, CancellationToken.None);

        Assert.Single(encontrados);
        Assert.False(encontrados[0].Ativo);
    }

    [Fact]
    public async Task DeveAtualizarFornecedorExistente()
    {
        var cadastrado = await _service.CadastrarAsync(TestData.NovoCadastro(), CancellationToken.None);

        var atualizado = await _service.AtualizarAsync(
            cadastrado.Id,
            TestData.NovoUpdate(email: "novo@exemplo.com", razaoSocial: "Razão Atualizada Ltda"),
            CancellationToken.None);

        Assert.Equal("Razão Atualizada Ltda", atualizado.RazaoSocial);
        Assert.Equal("novo@exemplo.com", atualizado.Email);
        Assert.Equal(cadastrado.Cnpj, atualizado.Cnpj);
    }

    [Fact]
    public async Task DeveRejeitarEmailDuplicadoNaAtualizacao()
    {
        await _service.CadastrarAsync(TestData.NovoCadastro(email: "ocupado@exemplo.com"), CancellationToken.None);
        var fornecedor = await _service.CadastrarAsync(TestData.NovoCadastro(), CancellationToken.None);

        await Assert.ThrowsAsync<FornecedorDuplicadoException>(() => _service.AtualizarAsync(
            fornecedor.Id,
            TestData.NovoUpdate(email: "OCUPADO@EXEMPLO.COM"),
            CancellationToken.None));
    }

    [Fact]
    public async Task DeveDesativarFornecedorExistente()
    {
        var cadastrado = await _service.CadastrarAsync(TestData.NovoCadastro(), CancellationToken.None);

        await _service.ExcluirAsync(cadastrado.Id, CancellationToken.None);

        var inativo = await _service.ObterPorIdAsync(cadastrado.Id, CancellationToken.None);
        Assert.False(inativo.Ativo);
    }

    [Fact]
    public async Task DeveDesativarFornecedorDeFormaIdempotente()
    {
        var cadastrado = await _service.CadastrarAsync(TestData.NovoCadastro(), CancellationToken.None);

        await _service.ExcluirAsync(cadastrado.Id, CancellationToken.None);
        await _service.ExcluirAsync(cadastrado.Id, CancellationToken.None);

        Assert.Single(_repository.Fornecedores);
    }

    [Fact]
    public async Task DeveReativarFornecedorInativoPeloPut()
    {
        var cadastrado = await _service.CadastrarAsync(TestData.NovoCadastro(), CancellationToken.None);
        await _service.ExcluirAsync(cadastrado.Id, CancellationToken.None);

        var reativado = await _service.AtualizarAsync(
            cadastrado.Id,
            TestData.NovoUpdate(ativo: true),
            CancellationToken.None);

        Assert.True(reativado.Ativo);
    }
}
