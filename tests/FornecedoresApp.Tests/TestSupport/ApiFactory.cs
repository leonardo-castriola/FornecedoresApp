using FornecedoresApp.Domain.Entities;
using FornecedoresApp.Infrastructure.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.TestHost;

namespace FornecedoresApp.Tests.TestSupport;

public sealed class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string SenhaAdmin = "Senha-Teste-123!";
    private readonly string _nomeBanco = $"FornecedoresAppTests-{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DataContext>();
            services.RemoveAll<DbContextOptions>();
            services.RemoveAll<DbContextOptions<DataContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<DataContext>>();
            services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(_nomeBanco));
        });
    }

    public async Task InitializeAsync()
    {
        await using var scope = Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        await context.Database.EnsureCreatedAsync();

        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Usuario>>();
        var usuarioTemporario = Usuario.Criar("Administrador de Teste", "admin-teste", "hash-temporario");
        var hash = hasher.HashPassword(usuarioTemporario, SenhaAdmin);
        context.Usuarios.Add(Usuario.Criar("Administrador de Teste", "admin-teste", hash));
        await context.SaveChangesAsync();
    }

    public override ValueTask DisposeAsync() => base.DisposeAsync();

    Task IAsyncLifetime.DisposeAsync() => base.DisposeAsync().AsTask();

    public static void Autenticar(HttpClient client, string login = "admin-teste", string senha = SenhaAdmin)
    {
        var credenciais = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{login}:{senha}"));
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credenciais);
    }

    public async Task CriarUsuarioInativoAsync()
    {
        await using var scope = Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Usuario>>();
        var usuarioTemporario = Usuario.Criar("Usuário Inativo", "inativo-teste", "hash-temporario");
        var hash = hasher.HashPassword(usuarioTemporario, SenhaAdmin);
        var usuario = Usuario.Criar("Usuário Inativo", "inativo-teste", hash);
        usuario.Desativar();
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();
    }
}
