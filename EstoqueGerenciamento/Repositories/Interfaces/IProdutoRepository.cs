using EstoqueGerenciamento.Models;

namespace EstoqueGerenciamento.Repositories.Interfaces;

public interface IProdutoRepository
{
    Task<List<Produto>> ObterTodosAsync();
    Task<Produto> ObterPorIdAsync(Guid id);
    Task<List<Produto>> ObterComEstoqueBaixp();
    Task AdicionarProduto(Produto produto);
    Task AtualizarProduto(Produto produto);
    Task RemoverProduto(Produto produto);
    Task<bool> ExisteProduto(Guid id);
}