namespace FornecedoresApp.API.Services.Exceptions;

public sealed class FornecedorNaoEncontradoException(Guid id)
    : Exception($"Fornecedor com ID '{id}' não foi encontrado.")
{
    public Guid FornecedorId { get; } = id;
}
