using EstoqueGerenciamento.Models;

namespace EstoqueGerenciamento.Repositories.Interfaces;

public interface IPedidoRepository
{
    Task<List<Pedido>> ObterTodosAsync();
    Task<Pedido> ObterPorIdAsync(Guid id);
    Task<List<Pedido>> ObterPorFornecedorAsync(Guid fornecedorId);
    Task AdicionarPedidoAsync(Pedido pedido);
    Task AtualizarPedidoAsync(Pedido pedido);
    Task RemoverPedidoAsync(Pedido pedido);
}