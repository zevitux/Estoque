using EstoqueGerenciamento.Models;

namespace EstoqueGerenciamento.Repositories.Interfaces;

public interface IPedidoRepository
{
    Task<List<Pedido>> ObterTodosAsync();
    Task<Pedido> ObterPorIdAsync(Guid id);
    Task<List<Pedido>> ObterPorFornecedorAsync(Guid fornecedorId);
    Task AdicionarProdutoAsync(Pedido pedido);
    Task AtualizarProdutoAsync(Pedido pedido);
    Task RemoverProdutoAsync(Pedido pedido);
    Task<bool> ExisteProduto(Guid id);
}