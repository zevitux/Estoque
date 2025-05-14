using EstoqueGerenciamento.DTOs.Pedido;
using EstoqueGerenciamento.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueGerenciamento.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;
    private readonly ILogger<PedidoController> _logger;

    public PedidoController(IPedidoService pedidoService, ILogger<PedidoController> logger)
    {
        _pedidoService = pedidoService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoResponseDto>>> ObterTodos()
    {
        _logger.LogInformation("Request de todos os pedidos");
        var pedidos = await _pedidoService.ObterTodosPedidosAsync();
        return Ok(pedidos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PedidoResponseDto>> ObterPorId(Guid id)
    {
        _logger.LogInformation("Request de pedido com Id: {PedidoId}", id);

        try
        {
            var pedido = await _pedidoService.ObterPedidoPorIdAsync(id);
            return Ok(pedido);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Pedido com Id: {PedidoId} n encontrado", id);
            return NotFound();
        }
    }

    [HttpGet("fornecedor/{fornecedorId}")]
    public async Task<ActionResult<IEnumerable<PedidoResponseDto>>> ObterPorFornecedor(Guid fornecedorId)
    {
        _logger.LogInformation("Request para obter pedidos do fornecedor com Id: {FornecedorId}", fornecedorId);
        var pedidos = await _pedidoService.ObterPedidosPorFornecedorAsync(fornecedorId);
        return Ok(pedidos);
    }

    [HttpPost]
    public async Task<ActionResult<PedidoResponseDto>> Criar(PedidoCreateDto dto)
    {
        _logger.LogInformation("Request para criar um novo pedido");

        try
        {
            var novoPedido = await _pedidoService.CriarPedidoAsync(dto);
            _logger.LogInformation("Pedido criado com sucesso, Id: {PedidoId}", novoPedido.Id);
            return CreatedAtAction(nameof(ObterPorId), new { novoPedido.Id }, novoPedido);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pedido");
            throw;
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<PedidoResponseDto>> Atualizar(Guid id, PedidoUpdateDto dto)
    {
        if (id != dto.Id)
        {
            _logger.LogWarning("ID da URL ({UrlId}) n bate com o do corpo da requisição ({BodyId})", id, dto.Id);
            return BadRequest("ID do corpo n corresponde ao ID da URL");
        }

        try
        {
            _logger.LogInformation("Request para atualizar pedido com Id: {PedidoId}", id);
            var pedidoAtualizado = await _pedidoService.AtualizarPedidoAsync(dto);
            _logger.LogInformation("Pedido atualizado com sucesso, Id: {PedidoId}", id);
            return Ok(pedidoAtualizado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar pedido com Id: {PedidoId}", id);
            throw;
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        _logger.LogInformation("Requisição para deletar pedido com Id: {PedidoId}", id);

        try
        {
            var sucesso = await _pedidoService.DeletarPedidoAsync(id);
            if (sucesso)
            {
                _logger.LogInformation("Pedido deletado com sucesso, Id: {PedidoId}", id);
                return NoContent();
            }

            _logger.LogWarning("Falha ao deletar pedido, Id: {PedidoId}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar pedido com Id: {PedidoId}", id);
            throw;
        }
    }
}