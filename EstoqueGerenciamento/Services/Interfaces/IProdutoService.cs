using EstoqueGerenciamento.DTOs.Produto;

namespace EstoqueGerenciamento.Services.Interfaces;

public interface IProdutoService
{
    Task<ProdutoResponseDto> CriarProdutoAsync(ProdutoCreateDto produtoCreateDto);
    Task<ProdutoResponseDto> AtualizarProdutoAsync(ProdutoUpdateDto produtoUpdateDto);
    Task<ProdutoResponseDto> ObterProdutoPorIdAsync(Guid produtoId);
    Task<IEnumerable<ProdutoResponseDto>> ObterTodosProdutosAsync();
    Task<IEnumerable<ProdutoResponseDto>> ObterComEstoqueBaixoAsync();
    Task<bool> DeletarProdutoAsync(Guid produtoId);
}