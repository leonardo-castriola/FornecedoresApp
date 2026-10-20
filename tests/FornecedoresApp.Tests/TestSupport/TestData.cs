using FornecedoresApp.API.DTOs;

namespace FornecedoresApp.Tests.TestSupport;

internal static class TestData
{
    private static long _cnpjSeed = 123456780001;

    public static string NovoCnpj()
    {
        var baseCnpj = Interlocked.Increment(ref _cnpjSeed).ToString("D12");
        var primeiroDigito = CalcularDigito(baseCnpj, [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]);
        var segundoDigito = CalcularDigito(baseCnpj + primeiroDigito, [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]);
        return $"{baseCnpj}{primeiroDigito}{segundoDigito}";
    }

    public static CadastrarFornecedorRequest NovoCadastro(
        string? cnpj = null,
        string? email = null,
        string? razaoSocial = null) => new()
    {
        RazaoSocial = razaoSocial ?? "Empresa de Teste Ltda",
        NomeFantasia = "Empresa Teste",
        Cnpj = cnpj ?? NovoCnpj(),
        Email = email ?? $"fornecedor-{Guid.NewGuid():N}@exemplo.com",
        Endereco = "Rua de Teste, 123",
        Telefone = "11999999999"
    };

    public static AtualizarFornecedorRequest NovoUpdate(
        string? email = null,
        bool ativo = true,
        string? razaoSocial = null) => new()
    {
        RazaoSocial = razaoSocial ?? "Empresa Atualizada Ltda",
        NomeFantasia = "Nome Atualizado",
        Email = email ?? $"atualizado-{Guid.NewGuid():N}@exemplo.com",
        Endereco = "Avenida Atualizada, 456",
        Telefone = "11988888888",
        Ativo = ativo
    };

    private static int CalcularDigito(string baseCnpj, IReadOnlyList<int> pesos)
    {
        var soma = baseCnpj.Select((digito, indice) => (digito - '0') * pesos[indice]).Sum();
        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}
