using EstoqueGerenciamento.Models;

namespace EstoqueGerenciamento.Repositories.Interfaces;

public interface IItemPedidoRepository
{
    Task<List<ItemPedido>> ObterTodosAsync();
    Task<ItemPedido> ObterPorIdAsync(Guid id);
    Task<List<ItemPedido>> ObterTodosPorPedidoIdAsync(Guid pedidoId);
    Task AdicionarAsync(ItemPedido itemPedido);
    Task AtualizarAsync(ItemPedido itemPedido);
    Task RemoverAsync(ItemPedido itemPedido);
    Task<bool> ExisteAsync(Guid id);
}