using EstoqueGerenciamento.DTOs.ItemPedido;

namespace EstoqueGerenciamento.Services.Interfaces;

public interface IItemPedidoService
{
    Task<IEnumerable<ItemPedidoResponseDto>> ObterTodosAsync();
    Task<ItemPedidoResponseDto> ObterPorIdAsync(Guid id);
    Task<IEnumerable<ItemPedidoResponseDto>> ObterPorPedidoIdAsync(Guid pedidoId);
    Task<ItemPedidoResponseDto> CriarAsync(ItemPedidoCreateDto itemPedidoCreateDto);
    Task<ItemPedidoResponseDto> AtualizarAsync(ItemPedidoUpdateDto itemPedidoUpdateDto);
    Task<bool> RemoverAsync(Guid id);
}