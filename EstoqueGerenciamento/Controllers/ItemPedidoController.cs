using EstoqueGerenciamento.DTOs.ItemPedido;
using EstoqueGerenciamento.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueGerenciamento.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemPedidoController : ControllerBase
{
    private readonly IItemPedidoService _itemPedidoService;
    private readonly ILogger<ItemPedidoController> _logger;

    public ItemPedidoController(IItemPedidoService itemPedidoService, ILogger<ItemPedidoController> logger)
    {
        _itemPedidoService = itemPedidoService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemPedidoResponseDto>>> ObterTodos()
    {
        _logger.LogInformation("Request para obter todos os pedidos");
        var itens = await _itemPedidoService.ObterTodosAsync();
        return Ok(itens);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemPedidoResponseDto>> ObterPorId(Guid id)
    {
        _logger.LogInformation("Request para obter por {id}", id);

        try
        {
            var item = await _itemPedidoService.ObterPorIdAsync(id);
            return Ok(item);
        }
        catch (KeyNotFoundException)
        {
            _logger.LogInformation("Item de pedido com Id: {ItemId} n encontrado", id);
            return NotFound();
        }
    }
    
    [HttpGet("pedido/{pedidoId}")]
    public async Task<ActionResult<IEnumerable<ItemPedidoResponseDto>>> ObterPorPedidoId(Guid pedidoId)
    {
        _logger.LogInformation("Request para obter itens do pedido com Id: {PedidoId}", pedidoId);
        var itens = await _itemPedidoService.ObterPorPedidoIdAsync(pedidoId);
        return Ok(itens);
    }
    
    [HttpPost]
    public async Task<ActionResult<ItemPedidoResponseDto>> Criar(ItemPedidoCreateDto dto)
    {
        _logger.LogInformation("Request para criar item de pedido");

        try
        {
            var novoItem = await _itemPedidoService.CriarAsync(dto);
            _logger.LogInformation("Item de pedido criado com sucesso. Id: {ItemId}", novoItem.Id);
            return CreatedAtAction(nameof(ObterPorId), new { id = novoItem.Id }, novoItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar item de pedido");
            throw;
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<ItemPedidoResponseDto>> Atualizar(Guid id, [FromBody] ItemPedidoUpdateDto dto)
    {
        if (id != dto.Id)
        {
            _logger.LogWarning("ID da URL ({UrlId}) não bate com o do corpo da requisição ({BodyId})", id, dto.Id);
            return BadRequest("ID do corpo não corresponde ao ID da URL.");
        }

        try
        {
            _logger.LogInformation("Request para atualizar item de pedido com Id: {ItemId}", id);
            var itemAtualizado = await _itemPedidoService.AtualizarAsync(dto);
            _logger.LogInformation("Item de pedido atualizado com sucesso. Id: {ItemId}", id);
            return Ok(itemAtualizado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar item de pedido com Id: {ItemId}", id);
            throw;
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        _logger.LogInformation("Request para deletar item de pedido com Id: {ItemId}", id);

        try
        {
            await _itemPedidoService.RemoverAsync(id);
            _logger.LogInformation("Item de pedido deletado com sucesso. Id: {ItemId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar item de pedido com Id: {ItemId}", id);
            throw;
        }
    }
}