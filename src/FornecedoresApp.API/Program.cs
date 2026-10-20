using FornecedoresApp.API.Authentication;
using FornecedoresApp.API.Initialization;
using FornecedoresApp.API.Services;
using FornecedoresApp.Domain.Entities;
using FornecedoresApp.Domain.Interfaces;
using FornecedoresApp.Infrastructure.Contexts;
using FornecedoresApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets(typeof(Program).Assembly, optional: true);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FornecedoresApp API",
        Version = "v1",
        Description = "API para cadastro e gerenciamento de fornecedores."
    });

    options.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        Description = "Autenticação HTTP Basic. Informe usuário e senha."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Basic", document)] = []
    });
});
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BDFornecedores")));
builder.Services.AddScoped<IFornecedorRepository, FornecedorRepository>();
builder.Services.AddScoped<FornecedorService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<DevelopmentUserInitializer>();
builder.Services.AddSingleton<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
builder.Services
    .AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", _ => { });
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    await InicializarBancoAsync(app, app.Environment.IsDevelopment());
}

if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();
}

app.UseSwagger(options => options.RouteTemplate = "swagger/{documentName}/swagger.json");
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "swagger";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FornecedoresApp API v1");
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

static async Task InicializarBancoAsync(WebApplication app, bool criarUsuarioDeDesenvolvimento)
{
    var logger = app.Services.GetRequiredService<ILoggerFactory>()
        .CreateLogger("DatabaseStartup");
    var configuration = app.Configuration;
    var tentativas = configuration.GetValue("DatabaseStartup:RetryCount", 5);
    var intervaloSegundos = configuration.GetValue("DatabaseStartup:RetryDelaySeconds", 5);

    tentativas = Math.Max(1, tentativas);
    intervaloSegundos = Math.Max(1, intervaloSegundos);

    for (var tentativa = 1; tentativa <= tentativas; tentativa++)
    {
        try
        {
            await using var scope = app.Services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            // O container do SQL Server pode estar iniciando quando a API é executada.
            await context.Database.MigrateAsync();

            if (criarUsuarioDeDesenvolvimento)
            {
                var inicializador = scope.ServiceProvider.GetRequiredService<DevelopmentUserInitializer>();
                await inicializador.InicializarAsync();
            }

            logger.LogInformation("Banco de dados conectado e inicializado com sucesso.");
            return;
        }
        catch (SqlException exception) when (tentativa < tentativas)
        {
            logger.LogWarning(
                exception,
                "Não foi possível conectar ao SQL Server na tentativa {Tentativa}/{Total}. " +
                "Nova tentativa em {Atraso} segundos.",
                tentativa,
                tentativas,
                intervaloSegundos);

            await Task.Delay(TimeSpan.FromSeconds(intervaloSegundos));
        }
        catch (SqlException exception)
        {
            var connectionString = configuration.GetConnectionString("BDFornecedores");
            var server = connectionString is null ? "não configurado" : ObterServidor(connectionString);

            logger.LogCritical(
                exception,
                "Não foi possível conectar ao SQL Server após {Total} tentativas. Servidor configurado: {Servidor}.",
                tentativas,
                server);

            throw new InvalidOperationException(
                $"Não foi possível conectar ao SQL Server ({server}). " +
                "Verifique se o container está em execução (docker compose up -d), " +
                "se a senha SA coincide com a connection string e se a porta 1434 está disponível.",
                exception);
        }
    }

    throw new InvalidOperationException("A inicialização do banco de dados foi interrompida.");
}

static string ObterServidor(string connectionString)
{
    var builder = new SqlConnectionStringBuilder(connectionString);
    return builder.DataSource;
}

public partial class Program { }
