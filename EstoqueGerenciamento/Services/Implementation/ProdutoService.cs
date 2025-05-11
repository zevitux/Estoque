using EstoqueGerenciamento.DTOs.Produto;
using EstoqueGerenciamento.Models;
using EstoqueGerenciamento.Repositories.Interfaces;
using EstoqueGerenciamento.Services.Interfaces;

namespace EstoqueGerenciamento.Services.Implementation;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ILogger<ProdutoService> _logger;

    public ProdutoService(IProdutoRepository produtoRepository, ILogger<ProdutoService> logger)
    {
        _produtoRepository = produtoRepository;
        _logger = logger;
    }

    public async Task<ProdutoResponseDto> CriarProdutoAsync(ProdutoCreateDto produtoCreateDto)
    {
        try
        {
            ValidarProduto(produtoCreateDto.Preco, produtoCreateDto.QuantidadeEstoque, produtoCreateDto.QuantidadeMinima);

            var produto = new Produto
            {
                Id = Guid.NewGuid(),
                Nome = produtoCreateDto.Nome,
                Descricao = produtoCreateDto.Descricao,
                Preco = produtoCreateDto.Preco,
                QuantidadeEstoque = produtoCreateDto.QuantidadeEstoque,
                QuantidadeMinima = produtoCreateDto.QuantidadeMinima
            };

            await _produtoRepository.AdicionarProduto(produto);

            return MapearParaResponseDto(produto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar produto");
            throw;
        }
    }

    public async Task<ProdutoResponseDto> AtualizarProdutoAsync(ProdutoUpdateDto produtoUpdateDto)
    {
        try
        {
            var produtoExistente = await _produtoRepository.ObterPorIdAsync(produtoUpdateDto.Id);
            
            if (produtoExistente is null)
                throw new KeyNotFoundException("Produto não encontrado");

            ValidarProduto(produtoUpdateDto.Preco, produtoUpdateDto.QuantidadeEstoque, produtoUpdateDto.QuantidadeMinima);

            produtoExistente.Nome = produtoUpdateDto.Nome;
            produtoExistente.Descricao = produtoUpdateDto.Descricao;
            produtoExistente.Preco = produtoUpdateDto.Preco;
            produtoExistente.QuantidadeEstoque = produtoUpdateDto.QuantidadeEstoque;
            produtoExistente.QuantidadeMinima = produtoUpdateDto.QuantidadeMinima;

            await _produtoRepository.AtualizarProduto(produtoExistente);

            return MapearParaResponseDto(produtoExistente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar produto ID: {ProdutoId}", produtoUpdateDto.Id);
            throw;
        }
    }

    public async Task<ProdutoResponseDto> ObterProdutoPorIdAsync(Guid produtoId)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(produtoId);
            
            return produto is null 
                ? throw new KeyNotFoundException("Produto não encontrado") 
                : MapearParaResponseDto(produto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter produto ID: {ProdutoId}", produtoId);
            throw;
        }
    }

    public async Task<IEnumerable<ProdutoResponseDto>> ObterTodosProdutosAsync()
    {
        try
        {
            var produtos = await _produtoRepository.ObterTodosAsync();
            return produtos.Select(MapearParaResponseDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter todos os produtos");
            throw;
        }
    }

    public async Task<IEnumerable<ProdutoResponseDto>> ObterComEstoqueBaixoAsync()
    {
        try
        {
            var produtos = await _produtoRepository.ObterComEstoqueBaixo();
            return produtos.Select(MapearParaResponseDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter produtos com estoque baixo");
            throw;
        }
    }

    public async Task<bool> DeletarProdutoAsync(Guid produtoId)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(produtoId) 
                ?? throw new KeyNotFoundException("Produto não encontrado");

            await _produtoRepository.RemoverProduto(produto);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover produto ID: {ProdutoId}", produtoId);
            throw;
        }
    }

    private static ProdutoResponseDto MapearParaResponseDto(Produto produto)
    {
        return new ProdutoResponseDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            QuantidadeEstoque = produto.QuantidadeEstoque,
            QuantidadeMinima = produto.QuantidadeMinima
        };
    }

    private static void ValidarProduto(decimal preco, int quantidadeEstoque, int quantidadeMinima)
    {
        if (preco <= 0)
            throw new ArgumentException("Preço deve ser maior que zero");

        if (quantidadeEstoque < 0)
            throw new ArgumentException("Estoque não pode ser negativo");

        if (quantidadeMinima < 0)
            throw new ArgumentException("Quantidade mínima não pode ser negativa");
    }
}