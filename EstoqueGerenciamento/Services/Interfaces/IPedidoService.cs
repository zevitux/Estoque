using EstoqueGerenciamento.DTOs.Pedido;

namespace EstoqueGerenciamento.Services.Interfaces;

public interface IPedidoService
{
    Task<IEnumerable<PedidoResponseDto>> ObterTodosPedidosAsync();
    Task<PedidoResponseDto> ObterPedidoPorIdAsync(Guid id);
    Task<IEnumerable<PedidoResponseDto>> ObterPedidosPorFornecedorAsync(Guid fornecedorId);
    Task<PedidoResponseDto> CriarPedidoAsync(PedidoCreateDto pedidoCreateDto);
    Task<PedidoResponseDto> AtualizarPedidoAsync(PedidoUpdateDto pedidoUpdateDto);
    Task<bool> DeletarPedidoAsync(Guid id);
}