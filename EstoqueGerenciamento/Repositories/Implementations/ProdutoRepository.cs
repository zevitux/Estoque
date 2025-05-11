using EstoqueGerenciamento.Data;
using EstoqueGerenciamento.Models;
using EstoqueGerenciamento.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EstoqueGerenciamento.Repositories.Implementations;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProdutoRepository> _logger;

    public ProdutoRepository(AppDbContext context, ILogger<ProdutoRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<List<Produto>> ObterTodosAsync()
    {
        _logger.LogInformation("Retornando todos do produto");
        return await _context.Produtos.ToListAsync();
    }

    public async Task<Produto> ObterPorIdAsync(Guid id)
    {
        _logger.LogInformation("Retornando produto por Id: {Id}", id);
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
        {
            _logger.LogWarning("Produto nao encontrado");
            return null!;
        }
        
        return produto;
    }

    public async Task<List<Produto>> ObterComEstoqueBaixo()
    {
        _logger.LogInformation("Retornando todos os produtos com estoque baixo");
        return await _context.Produtos
            .Where(p => p.QuantidadeEstoque < p.QuantidadeMinima)
            .ToListAsync();
    }

    public async Task AdicionarProduto(Produto produto)
    {
        _logger.LogInformation("Adicionando produto");
        
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Produto adicionado com sucesso");
    }

    public async Task AtualizarProduto(Produto produto)
    {
        _logger.LogInformation("Atualizando produto com Id {Id}", produto.Id);
        
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Produto com Id {Id} atualizado com sucesso", produto.Id);
    }

    public async Task RemoverProduto(Produto produto)
    {
        _logger.LogInformation("Removendo produto");
        
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Produto removido com sucesso");
    }
}