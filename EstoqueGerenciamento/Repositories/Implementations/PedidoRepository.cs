using EstoqueGerenciamento.Data;
using EstoqueGerenciamento.Models;
using EstoqueGerenciamento.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EstoqueGerenciamento.Repositories.Implementations;

public class PedidoRepository : IPedidoRepository
{
    private AppDbContext _context;
    private readonly ILogger<PedidoRepository> _logger;

    public PedidoRepository(AppDbContext context, ILogger<PedidoRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<List<Pedido>> ObterTodosAsync()
    {
        _logger.LogInformation("Retornando todos os pedidos");
        
        return await  _context.Pedidos
            .Include(p => p.Fornecedor)
            .Include(p => p.ItensPedido)
            .ThenInclude(a => a.Produto)
            .ToListAsync();
    }

    public async Task<Pedido> ObterPorIdAsync(Guid id)
    {
        _logger.LogInformation("Retornando pedido por Id");
        
        var pedido = await  _context.Pedidos
            .Include(p => p.Fornecedor)
            .Include(p => p.ItensPedido)
            .ThenInclude(a => a.Produto)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if(pedido == null)
            _logger.LogWarning("Pedido com {Id} nao foi encontrado", id);
        
        return pedido!;
    }

    public async Task<List<Pedido>> ObterPorFornecedorAsync(Guid fornecedorId)
    {
        _logger.LogInformation("Retornando pedidos pelo id do fornecedor");
        
        return await _context.Pedidos
            .Where(p => p.FornecedorId == fornecedorId)
            .Include(p => p.ItensPedido)
            .ThenInclude(a => a.Produto)
            .ToListAsync();
    }

    public async Task AdicionarProdutoAsync(Pedido pedido)
    {
        _logger.LogInformation("Adicionando pedido");
        
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Pedido adicionado com sucesso");
    }

    public async Task AtualizarProdutoAsync(Pedido pedido)
    {
        _logger.LogInformation("Atualizando pedido com Id {Id}", pedido.Id);
        
        _context.Pedidos.Update(pedido);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Pedido com Id {Id} atualizado com sucesso", pedido.Id);
    }

    public async Task RemoverProdutoAsync(Pedido pedido)
    {
        _logger.LogInformation("Removendo pedido com Id {Id}", pedido.Id);
        
        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Pedido removidado com sucesso");
    }

    public async Task<bool> ExisteProduto(Guid id)
    {
        _logger.LogInformation("Verificando se o pedido existe");
        var existe = await _context.Pedidos.AnyAsync(p => p.Id == id);
        
        if(!existe)
            _logger.LogWarning("Produto com Id {Id} nao encontrado ou nao existe", id);
        
        return existe;
    }
}