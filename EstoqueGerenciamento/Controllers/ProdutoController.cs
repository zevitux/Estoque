using EstoqueGerenciamento.DTOs.Produto;
using EstoqueGerenciamento.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueGerenciamento.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;
    private readonly ILogger<ProdutoController> _logger;

    public ProdutoController(IProdutoService produtoService, ILogger<ProdutoController> logger)
    {
        _produtoService = produtoService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoResponseDto>>> ObterTodos()
    {
        var produtos = await _produtoService.ObterTodosProdutosAsync();
        return Ok(produtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProdutoResponseDto>> ObterPorId(Guid id)
    {
        try
        {
            var produto = await _produtoService.ObterProdutoPorIdAsync(id);
            return Ok(produto);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogWarning(e, "Produto não encontrado: {ProdutoId}", id);
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoResponseDto>> Criar(ProdutoCreateDto dto)
    {
        try
        {
            var produtoCriado = await _produtoService.CriarProdutoAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = produtoCriado.Id }, produtoCriado);
        }
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, "Erro de validação ao criar produto");
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProdutoResponseDto>> Atualizar(Guid id, ProdutoUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest("ID do produto não confere com o corpo da requisição.");

        try
        {
            var atualizado = await _produtoService.AtualizarProdutoAsync(dto);
            return Ok(atualizado);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogWarning(e, "Produto não encontrado para atualização: {ProdutoId}", id);
            return NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, "Erro de validação ao atualizar produto: {ProdutoId}", id);
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        try
        {
            await _produtoService.DeletarProdutoAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogWarning(e, "Produto não encontrado para remoção: {ProdutoId}", id);
            return NotFound(e.Message);
        }
    }

    [HttpGet("estoque-baixo")]
    public async Task<ActionResult<IEnumerable<ProdutoResponseDto>>> ObterComEstoqueBaixo()
    {
        var produtos = await _produtoService.ObterComEstoqueBaixoAsync();
        return Ok(produtos);
    }
}
