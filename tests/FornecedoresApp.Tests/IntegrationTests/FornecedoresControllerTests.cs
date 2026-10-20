using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FornecedoresApp.API.DTOs;
using FornecedoresApp.Tests.TestSupport;

namespace FornecedoresApp.Tests.IntegrationTests;

public sealed class FornecedoresControllerTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task DeveCadastrarFornecedorValido()
    {
        using var client = ClienteAutenticado();
        var request = TestData.NovoCadastro();

        var response = await client.PostAsJsonAsync("/api/v1/fornecedores", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        var fornecedor = await response.Content.ReadFromJsonAsync<FornecedorResponse>();
        Assert.NotNull(fornecedor);
        Assert.Equal(request.Cnpj, fornecedor.Cnpj);
        Assert.True(fornecedor.Ativo);
    }

    [Fact]
    public async Task DeveRetornar400ParaFornecedorInvalido()
    {
        using var client = ClienteAutenticado();
        var request = TestData.NovoCadastro(cnpj: "11111111111111");

        var response = await client.PostAsJsonAsync("/api/v1/fornecedores", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(body.RootElement.GetProperty("errors").TryGetProperty("Cnpj", out _) ||
                    body.RootElement.GetProperty("errors").TryGetProperty("cnpj", out _));
    }

    [Fact]
    public async Task DeveRetornar409ParaCnpjDuplicado()
    {
        using var client = ClienteAutenticado();
        var request = TestData.NovoCadastro();
        Assert.Equal(HttpStatusCode.Created, (await client.PostAsJsonAsync("/api/v1/fornecedores", request)).StatusCode);

        var duplicado = await client.PostAsJsonAsync(
            "/api/v1/fornecedores",
            TestData.NovoCadastro(cnpj: request.Cnpj));

        Assert.Equal(HttpStatusCode.Conflict, duplicado.StatusCode);
    }

    [Fact]
    public async Task DeveRetornar409ParaEmailDuplicado()
    {
        using var client = ClienteAutenticado();
        const string email = "email-unico@exemplo.com";
        Assert.Equal(HttpStatusCode.Created, (await client.PostAsJsonAsync(
            "/api/v1/fornecedores", TestData.NovoCadastro(email: email))).StatusCode);

        var duplicado = await client.PostAsJsonAsync(
            "/api/v1/fornecedores",
            TestData.NovoCadastro(email: email));

        Assert.Equal(HttpStatusCode.Conflict, duplicado.StatusCode);
    }

    [Fact]
    public async Task DeveRetornar400ParaEmailInvalido()
    {
        using var client = ClienteAutenticado();

        var response = await client.PostAsJsonAsync(
            "/api/v1/fornecedores",
            TestData.NovoCadastro(email: "email-invalido"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(body.RootElement.GetProperty("errors").TryGetProperty("Email", out _) ||
                    body.RootElement.GetProperty("errors").TryGetProperty("email", out _));
    }

    [Fact]
    public async Task DeveListarFornecedoresAutenticado()
    {
        using var client = ClienteAutenticado();
        var cadastrado = TestData.NovoCadastro();
        await client.PostAsJsonAsync("/api/v1/fornecedores", cadastrado);

        var response = await client.GetAsync("/api/v1/fornecedores");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(cadastrado.Email!, await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task DeveConsultarFornecedorExistente()
    {
        using var client = ClienteAutenticado();
        var criado = await CriarFornecedorAsync(client);

        var response = await client.GetAsync($"/api/v1/fornecedores/{criado.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var fornecedor = await response.Content.ReadFromJsonAsync<FornecedorResponse>();
        Assert.Equal(criado.Id, fornecedor?.Id);
    }

    [Fact]
    public async Task DeveRetornar404ParaFornecedorInexistente()
    {
        using var client = ClienteAutenticado();

        var response = await client.GetAsync($"/api/v1/fornecedores/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeveRetornar400ParaIdMalformado()
    {
        using var client = ClienteAutenticado();

        var response = await client.GetAsync("/api/v1/fornecedores/id-invalido");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeveAtualizarFornecedorExistente()
    {
        using var client = ClienteAutenticado();
        var criado = await CriarFornecedorAsync(client);

        var response = await client.PutAsJsonAsync(
            $"/api/v1/fornecedores/{criado.Id}",
            TestData.NovoUpdate(email: "email-atualizado@exemplo.com"));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var atualizado = await response.Content.ReadFromJsonAsync<FornecedorResponse>();
        Assert.Equal(criado.Cnpj, atualizado?.Cnpj);
        Assert.Equal("email-atualizado@exemplo.com", atualizado?.Email);
    }

    [Fact]
    public async Task DeveRetornar409ParaEmailDuplicadoNaAtualizacao()
    {
        using var client = ClienteAutenticado();
        var primeiro = await CriarFornecedorAsync(client);
        var segundo = await CriarFornecedorAsync(client);

        var response = await client.PutAsJsonAsync(
            $"/api/v1/fornecedores/{segundo.Id}",
            TestData.NovoUpdate(email: primeiro.Email));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task DeveDesativarFornecedorExistente()
    {
        using var client = ClienteAutenticado();
        var criado = await CriarFornecedorAsync(client);

        var response = await client.DeleteAsync($"/api/v1/fornecedores/{criado.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(string.Empty, await response.Content.ReadAsStringAsync());
        var obtido = await client.GetFromJsonAsync<FornecedorResponse>($"/api/v1/fornecedores/{criado.Id}");
        Assert.False(obtido?.Ativo);
    }

    [Fact]
    public async Task DeveReativarFornecedorInativoPeloPut()
    {
        using var client = ClienteAutenticado();
        var criado = await CriarFornecedorAsync(client);
        await client.DeleteAsync($"/api/v1/fornecedores/{criado.Id}");

        var response = await client.PutAsJsonAsync(
            $"/api/v1/fornecedores/{criado.Id}",
            TestData.NovoUpdate(ativo: true));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var atualizado = await response.Content.ReadFromJsonAsync<FornecedorResponse>();
        Assert.True(atualizado?.Ativo);
    }

    private HttpClient ClienteAutenticado()
    {
        var client = factory.CreateClient();
        ApiFactory.Autenticar(client);
        return client;
    }

    private static async Task<FornecedorResponse> CriarFornecedorAsync(HttpClient client)
    {
        var response = await client.PostAsJsonAsync("/api/v1/fornecedores", TestData.NovoCadastro());
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<FornecedorResponse>())!;
    }
}
