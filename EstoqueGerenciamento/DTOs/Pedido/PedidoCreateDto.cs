using EstoqueGerenciamento.DTOs.ItemPedido;

namespace EstoqueGerenciamento.DTOs.Pedido;

public class PedidoCreateDto
{
    public Guid FornecedorId { get; set; }
    public List<ItemPedidoCreateDto> ItensPedido { get; set; } = new();
}