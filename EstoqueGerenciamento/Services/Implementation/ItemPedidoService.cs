using EstoqueGerenciamento.DTOs.ItemPedido;
using EstoqueGerenciamento.Models;
using EstoqueGerenciamento.Repositories.Interfaces;
using EstoqueGerenciamento.Services.Interfaces;

namespace EstoqueGerenciamento.Services.Implementation;

public class ItemPedidoService : IItemPedidoService
{
    private readonly IItemPedidoRepository _itemPedidoRepository;
    private readonly ILogger<ItemPedidoService> _logger;
    
    public ItemPedidoService(IItemPedidoRepository itemPedidoRepository, ILogger<ItemPedidoService> logger)
    {
        _itemPedidoRepository = itemPedidoRepository;
        _logger = logger;
    }
    
    public async Task<IEnumerable<ItemPedidoResponseDto>> ObterTodosAsync()
    {
        try
        {
            var itens = await _itemPedidoRepository.ObterTodosAsync();
            return itens.Select(MapearParaResponseDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter todos os itens do pedido");
            throw;
        }
    }

    public async Task<ItemPedidoResponseDto> ObterPorIdAsync(Guid id)
    {
        try
        {
            var item = await _itemPedidoRepository.ObterPorIdAsync(id)
                       ?? throw new KeyNotFoundException("Item do pedido n encontrado");

            return MapearParaResponseDto(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter o item do pedido com Id: {ItemId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ItemPedidoResponseDto>> ObterPorPedidoIdAsync(Guid pedidoId)
    {
        try
        {
            var itens = await _itemPedidoRepository.ObterTodosPorPedidoIdAsync(pedidoId);
            return itens.Select(MapearParaResponseDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter itens do pedido com PedidoId: {PedidoId}", pedidoId);
            throw;
        }
    }

    public async Task<ItemPedidoResponseDto> CriarAsync(ItemPedidoCreateDto itemPedidoCreateDto)
    {
        try
        {
            var item = new ItemPedido
            {
                Id = Guid.NewGuid(),
                PedidoId = itemPedidoCreateDto.PedidoId,
                ProdutoId = itemPedidoCreateDto.ProdutoId,
                Quantidade = itemPedidoCreateDto.Quantidade,
                PrecoUnitario = itemPedidoCreateDto.PrecoUnitario,
                PrecoTotal = itemPedidoCreateDto.PrecoUnitario * itemPedidoCreateDto.Quantidade
            };

            await _itemPedidoRepository.AdicionarAsync(item);
            
            var itemCompleto = await _itemPedidoRepository.ObterPorIdAsync(item.Id);
            return MapearParaResponseDto(itemCompleto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar item pedido");
            throw;
        }
    }

    public async Task<ItemPedidoResponseDto> AtualizarAsync(ItemPedidoUpdateDto itemPedidoUpdateDto)
    {
        try
        {
            var item = await _itemPedidoRepository.ObterPorIdAsync(itemPedidoUpdateDto.Id)
                       ?? throw new KeyNotFoundException("Item do pedido n encontrado");

            item.ProdutoId = itemPedidoUpdateDto.ProdutoId;
            item.Quantidade = itemPedidoUpdateDto.Quantidade;
            item.PrecoUnitario = itemPedidoUpdateDto.PrecoUnitario;
            item.PrecoTotal = itemPedidoUpdateDto.PrecoUnitario * itemPedidoUpdateDto.Quantidade;

            await _itemPedidoRepository.AtualizarAsync(item);
            return MapearParaResponseDto(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar o item pedido com Id: {ItemId}",  itemPedidoUpdateDto.Id);
            throw;
        }
    }

    public async Task<bool> RemoverAsync(Guid id)
    {
        try
        {
            var item = await _itemPedidoRepository.ObterPorIdAsync(id)
                       ?? throw new KeyNotFoundException("Item do pedido n encontrado");

            await _itemPedidoRepository.RemoverAsync(item);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover item do pedido com Id: {ItemId}", id);
            throw;
        }
    }

    private ItemPedidoResponseDto MapearParaResponseDto(ItemPedido item)
    {
        return new ItemPedidoResponseDto
        {
            Id = item.Id,
            PedidoId = item.PedidoId,
            ProdutoId = item.ProdutoId,
            ProdutoNome = item.Produto.Nome,
            Quantidade = item.Quantidade,
            PrecoUnitario = item.PrecoUnitario,
            PrecoTotal = item.PrecoTotal,
        };
    }
}