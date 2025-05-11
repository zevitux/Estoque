using EstoqueGerenciamento.DTOs.ItemPedido;

namespace EstoqueGerenciamento.DTOs.Pedido;

public class PedidoUpdateDto
{
    public Guid Id { get; set; }
    public DateTime DataPedido { get; set; }
    public decimal Total { get; set; }
    public Guid FornecedorId { get; set; }
    public List<ItemPedidoUpdateDto> ItensPedido { get; set; } = new();
}