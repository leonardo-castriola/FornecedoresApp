using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using FornecedoresApp.Domain.Entities;
using FornecedoresApp.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace FornecedoresApp.API.Authentication;

public sealed class BasicAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    System.Text.Encodings.Web.UrlEncoder encoder,
    IUsuarioRepository usuarioRepository,
    IPasswordHasher<Usuario> passwordHasher)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private static readonly UTF8Encoding Utf8Strict = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var valoresAutorizacao))
        {
            return AuthenticateResult.NoResult();
        }

        if (valoresAutorizacao.Count != 1 || !AuthenticationHeaderValue.TryParse(valoresAutorizacao[0], out var cabecalho))
        {
            return AuthenticateResult.Fail("Credenciais inválidas.");
        }

        if (!string.Equals(cabecalho.Scheme, "Basic", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.NoResult();
        }

        string credenciais;
        try
        {
            credenciais = Utf8Strict.GetString(Convert.FromBase64String(cabecalho.Parameter ?? string.Empty));
        }
        catch (FormatException)
        {
            return AuthenticateResult.Fail("Credenciais inválidas.");
        }
        catch (DecoderFallbackException)
        {
            return AuthenticateResult.Fail("Credenciais inválidas.");
        }

        var separador = credenciais.IndexOf(':');
        if (separador <= 0 || separador == credenciais.Length - 1)
        {
            return AuthenticateResult.Fail("Credenciais inválidas.");
        }

        var login = credenciais[..separador];
        var senha = credenciais[(separador + 1)..];
        var usuario = await usuarioRepository.ObterPorLoginAsync(login, Context.RequestAborted);

        if (usuario is null || !usuario.Ativo)
        {
            return AuthenticateResult.Fail("Credenciais inválidas.");
        }

        var resultadoSenha = passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, senha);
        if (resultadoSenha == PasswordVerificationResult.Failed)
        {
            return AuthenticateResult.Fail("Credenciais inválidas.");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Login)
        };
        var identidade = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identidade);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers[HeaderNames.WWWAuthenticate] = "Basic realm=\"FornecedoresApp\"";
        return base.HandleChallengeAsync(properties);
    }
}
