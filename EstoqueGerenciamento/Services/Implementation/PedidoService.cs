using EstoqueGerenciamento.DTOs.ItemPedido;
using EstoqueGerenciamento.DTOs.Pedido;
using EstoqueGerenciamento.Models;
using EstoqueGerenciamento.Repositories.Interfaces;
using EstoqueGerenciamento.Services.Interfaces;

namespace EstoqueGerenciamento.Services.Implementation;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly ILogger<PedidoService> _logger;

    public PedidoService(IPedidoRepository pedidoRepository, ILogger<PedidoService> logger)
    {
        _pedidoRepository = pedidoRepository;
        _logger = logger;
    }
    
    public async Task<IEnumerable<PedidoResponseDto>> ObterTodosPedidosAsync()
    {
        try
        {
            var pedidos = await _pedidoRepository.ObterTodosAsync();
            return pedidos.Select(MapearParaResponseDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter todos os pedidos");
            throw;
        }
    }

    public async Task<PedidoResponseDto> ObterPedidoPorIdAsync(Guid id)
    {
        try
        {
            var pedido = await _pedidoRepository.ObterPorIdAsync(id)
                         ?? throw new KeyNotFoundException("Pedido nao encontrado");

            return MapearParaResponseDto(pedido);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter pedido com Id: {PedidoId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<PedidoResponseDto>> ObterPedidosPorFornecedorAsync(Guid fornecedorId)
    {
        try
        {
            var pedidos = await _pedidoRepository.ObterPorFornecedorAsync(fornecedorId);
            return pedidos.Select(MapearParaResponseDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter pedidos para o fornecedor com Id: {FornecedorId}", fornecedorId);
            throw;
        }
    }

    public async Task<PedidoResponseDto> CriarPedidoAsync(PedidoCreateDto pedidoCreateDto)
    {
        try
        {
            if (pedidoCreateDto.ItensPedido == null || !pedidoCreateDto.ItensPedido.Any())
                throw new ArgumentException("Pedido deve conter ao menos um item");

            
            var pedido = new Pedido
            {
                Id = Guid.NewGuid(),
                DataPedido = DateTime.UtcNow,
                FornecedorId = pedidoCreateDto.FornecedorId,
                ItensPedido = pedidoCreateDto.ItensPedido.Select(i => new ItemPedido
                {
                    Id = Guid.NewGuid(),
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    PrecoTotal = i.PrecoUnitario * i.Quantidade
                }).ToList(),
            };
            
            pedido.Total = pedido.ItensPedido.Sum(i => i.Quantidade);

            await _pedidoRepository.AdicionarPedidoAsync(pedido);
            
            var pedidoSalvo = await _pedidoRepository.ObterPorIdAsync(pedido.Id);
            
            return MapearParaResponseDto(pedidoSalvo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pedido para o fornecedor com Id: {FornecedorId}",
                pedidoCreateDto.FornecedorId);
            throw;
        }
    }

    public async Task<PedidoResponseDto> AtualizarPedidoAsync(PedidoUpdateDto pedidoUpdateDto)
    {
        try
        {
            var pedido = await _pedidoRepository.ObterPorIdAsync(pedidoUpdateDto.Id)
                         ?? throw new KeyNotFoundException("Pedido nao encontrado");

            pedido.FornecedorId = pedidoUpdateDto.FornecedorId;

            pedido.ItensPedido = pedidoUpdateDto.ItensPedido.Select(i => new ItemPedido
            {
                Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                PedidoId = pedidoUpdateDto.Id,
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario,
                PrecoTotal = i.PrecoUnitario * i.Quantidade
            }).ToList();

            pedido.Total = pedido.ItensPedido.Sum(i => i.PrecoTotal);

            await _pedidoRepository.AtualizarPedidoAsync(pedido);

            return MapearParaResponseDto(pedido);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar pedido com Id: {PedidoId}",  pedidoUpdateDto.Id);
            throw;
        }
    }

    public async Task<bool> DeletarPedidoAsync(Guid id)
    {
        try
        {
            var pedido = await _pedidoRepository.ObterPorIdAsync(id)
                         ?? throw new KeyNotFoundException("Pedido não encontrado");

            await _pedidoRepository.RemoverPedidoAsync(pedido);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover pedido com Id: {PedidoId}", id);
            throw;
        }
    }
    
    private PedidoResponseDto MapearParaResponseDto(Pedido pedido)
    {
        return new PedidoResponseDto
        {
            Id = pedido.Id,
            DataPedido = pedido.DataPedido,
            Total = pedido.Total,
            FornecedorId = pedido.FornecedorId,
            FornecedorNome = pedido.Fornecedor?.Nome ?? string.Empty,
            ItensPedido = pedido.ItensPedido.Select(i => new ItemPedidoResponseDto
            {
                Id = i.Id,
                PedidoId = i.PedidoId,
                ProdutoId = i.ProdutoId,
                ProdutoNome = i.Produto.Nome,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario,
                PrecoTotal = i.PrecoTotal
            }).ToList()
        };
    }
}