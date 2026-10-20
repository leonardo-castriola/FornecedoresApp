using System.Net;
using FornecedoresApp.Tests.TestSupport;

namespace FornecedoresApp.Tests.IntegrationTests;

public sealed class BasicAuthenticationTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task DeveAutenticarUsuarioComCredenciaisValidas()
    {
        using var client = factory.CreateClient();
        ApiFactory.Autenticar(client);

        var response = await client.GetAsync("/api/v1/fornecedores");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeveRejeitarSenhaIncorreta()
    {
        using var client = factory.CreateClient();
        ApiFactory.Autenticar(client, senha: "senha-errada");

        var response = await client.GetAsync("/api/v1/fornecedores");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Contains(response.Headers.WwwAuthenticate, header => header.Scheme == "Basic");
    }

    [Fact]
    public async Task DeveRejeitarUsuarioInexistente()
    {
        using var client = factory.CreateClient();
        ApiFactory.Autenticar(client, login: "nao-existe");

        var response = await client.GetAsync("/api/v1/fornecedores");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeveRejeitarUsuarioInativo()
    {
        await factory.CriarUsuarioInativoAsync();
        using var client = factory.CreateClient();
        ApiFactory.Autenticar(client, login: "inativo-teste");

        var response = await client.GetAsync("/api/v1/fornecedores");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeveRejeitarAuthorizationAusente()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/fornecedores");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Contains(response.Headers.WwwAuthenticate, header => header.Scheme == "Basic");
    }

    [Fact]
    public async Task DeveRejeitarBase64Invalido()
    {
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "%%%nao-base64%%%");

        var response = await client.GetAsync("/api/v1/fornecedores");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeveRejeitarEsquemaDiferenteDeBasic()
    {
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "token-invalido");

        var response = await client.GetAsync("/api/v1/fornecedores");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
