using FornecedoresApp.API.DTOs;
using FornecedoresApp.API.Services;
using FornecedoresApp.API.Services.Exceptions;
using FornecedoresApp.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FornecedoresApp.API.Controllers;

[ApiController]
[Route("api/v1/fornecedores")]
[Produces("application/json")]
public sealed class FornecedoresController(FornecedorService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<FornecedorResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public Task<IActionResult> Cadastrar(
        [FromBody] CadastrarFornecedorRequest request,
        CancellationToken cancellationToken) =>
        ExecutarAsync(async () =>
        {
            var fornecedor = await service.CadastrarAsync(request, cancellationToken);
            return CreatedAtAction(nameof(ObterPorId), new { id = fornecedor.Id }, fornecedor);
        });

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<FornecedorResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public Task<IActionResult> Pesquisar(
        [FromQuery] string? razaoSocial,
        [FromQuery] string? cnpj,
        [FromQuery] bool? ativo,
        CancellationToken cancellationToken) =>
        ExecutarAsync(async () => Ok(await service.PesquisarAsync(
            razaoSocial,
            cnpj,
            ativo,
            cancellationToken)));

    [HttpGet("{id}")]
    [ProducesResponseType<FornecedorResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public Task<IActionResult> ObterPorId([FromRoute] Guid id, CancellationToken cancellationToken) =>
        ExecutarAsync(async () => Ok(await service.ObterPorIdAsync(id, cancellationToken)));

    [HttpPut("{id}")]
    [ProducesResponseType<FornecedorResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public Task<IActionResult> Atualizar(
        [FromRoute] Guid id,
        [FromBody] AtualizarFornecedorRequest request,
        CancellationToken cancellationToken) =>
        ExecutarAsync(async () => Ok(await service.AtualizarAsync(id, request, cancellationToken)));

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public Task<IActionResult> Excluir([FromRoute] Guid id, CancellationToken cancellationToken) =>
        ExecutarAsync(async () =>
        {
            await service.ExcluirAsync(id, cancellationToken);
            return NoContent();
        });

    private async Task<IActionResult> ExecutarAsync(Func<Task<IActionResult>> operacao)
    {
        try
        {
            return await operacao();
        }
        catch (FornecedorNaoEncontradoException exception)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Fornecedor não encontrado",
                detail: exception.Message);
        }
        catch (FornecedorDuplicadoException exception)
        {
            return Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: "Conflito de dados",
                detail: exception.Message);
        }
        catch (RegraDeNegocioException exception)
        {
            var erros = new ModelStateDictionary();
            erros.AddModelError("cnpj", exception.Message);
            return ValidationProblem(
                modelStateDictionary: erros,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Dados inválidos");
        }
    }
}
