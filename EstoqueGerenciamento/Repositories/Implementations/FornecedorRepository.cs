using EstoqueGerenciamento.Data;
using EstoqueGerenciamento.Models;
using EstoqueGerenciamento.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EstoqueGerenciamento.Repositories.Implementations;

public class FornecedorRepository : IFornecedorRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<FornecedorRepository> _logger;

    public FornecedorRepository(AppDbContext context, ILogger<FornecedorRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<List<Fornecedor>> ObterTodosAsync()
    {
        _logger.LogInformation("Obter todos os fornecedores");
        
        return await _context.Fornecedores
            .Include(p => p.Pedidos)
            .ToListAsync();
    }

    public async Task<Fornecedor> ObterPorIdAsync(Guid id)
    {
        _logger.LogInformation("Obter fornecedores por id");
        
        var fornecedor  = await _context.Fornecedores
            .Include(p => p.Pedidos)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if(fornecedor == null)
            _logger.LogWarning("Fornecedor com id {Id} nao encontrado ou nao existe", id);
        
        return fornecedor!;
    }

    public async Task AdicionarAsync(Fornecedor fornecedor)
    {
        _logger.LogInformation("Adicionar fornecedor");
        
        _context.Fornecedores.Add(fornecedor);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Fornecedor adicionado com sucesso");
    }

    public async Task AtualizarAsync(Fornecedor fornecedor)
    {
        _logger.LogInformation("Atualizando fornecedor com Id {Id}", fornecedor.Id);
        
        _context.Fornecedores.Update(fornecedor);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Fornecedor atualizado com sucesso");
    }

    public async Task RemoverAsync(Fornecedor fornecedor)
    {
        _logger.LogInformation("Remover fornecedor com id {Id}", fornecedor.Id);
        
        _context.Fornecedores.Remove(fornecedor);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Fornecedor com id {Id} removido com sucesso", fornecedor.Id);
    }

    public async Task<bool> ExisteAsync(Guid id)
    {
        _logger.LogInformation("Existe fornecedor com Id {Id}", id);
        var existe = await _context.Fornecedores.AnyAsync(p => p.Id == id);
        
        if(!existe)
            _logger.LogWarning("Fornecedor com Id {Id} nao existe", id);
        
        return existe;
    }
}