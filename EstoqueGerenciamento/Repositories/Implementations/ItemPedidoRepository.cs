using EstoqueGerenciamento.Data;
using EstoqueGerenciamento.Models;
using EstoqueGerenciamento.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EstoqueGerenciamento.Repositories.Implementations;

public class ItemPedidoRepository : IItemPedidoRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ItemPedidoRepository> _logger;

    public ItemPedidoRepository(AppDbContext context, ILogger<ItemPedidoRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<List<ItemPedido>> ObterTodosAsync()
    {
        _logger.LogInformation("Obter todos os itens de pedidos");
        
        return await _context.ItensPedido
            .Include(p => p.Produto)
            .Include(p => p.Pedido)
            .ToListAsync();
    }

    public async Task<ItemPedido> ObterPorIdAsync(Guid id)
    {
        _logger.LogInformation("Obter item de pedido por id {Id}", id);
        
        var item = await _context.ItensPedido
            .Include(p => p.Produto)
            .Include(p => p.Pedido)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if(item == null)
            _logger.LogWarning("Item de pedido com Id {Id} nao existe", id);
        
        return item!;
    }

    public async Task<List<ItemPedido>> ObterTodosPorPedidoIdAsync(Guid pedidoId)
    {
        _logger.LogInformation("Retornando todos os itens de pedidos pelo id do pedido");

        return await _context.ItensPedido
            .Include(p => p.Produto)
            .Include(p => p.Pedido)
            .ToListAsync();
    }

    public async Task AdicionarAsync(ItemPedido itemPedido)
    {
        _logger.LogInformation("Adicionar item de pedido");
        
        _context.ItensPedido.Add(itemPedido);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Item de pedido adicionado com sucesso");
    }

    public async Task AtualizarAsync(ItemPedido itemPedido)
    {
        _logger.LogInformation("Atualizando item de pedido com Id {Id}", itemPedido.Id);
        
        _context.ItensPedido.Update(itemPedido);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Item de pedido atualizado com sucesso");
    }

    public async Task RemoverAsync(ItemPedido itemPedido)
    {
        _logger.LogInformation("Removendo item de pedido com id {Id}", itemPedido.Id);
        
        _context.ItensPedido.Remove(itemPedido);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Item de pedido removidado com sucesso");
    }
}