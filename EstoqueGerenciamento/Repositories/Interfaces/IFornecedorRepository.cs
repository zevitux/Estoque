using EstoqueGerenciamento.Models;

namespace EstoqueGerenciamento.Repositories.Interfaces;

public interface IFornecedorRepository
{
    Task<List<Fornecedor>> ObterTodosAsync();
    Task<Fornecedor> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Fornecedor fornecedor);
    Task AtualizarAsync(Fornecedor fornecedor);
    Task RemoverAsync(Fornecedor fornecedor);
}