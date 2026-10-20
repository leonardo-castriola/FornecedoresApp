namespace FornecedoresApp.API.Services.Exceptions;

public sealed class FornecedorDuplicadoException(string campo)
    : Exception($"Já existe fornecedor cadastrado com este {campo}.")
{
    public string Campo { get; } = campo;
}
