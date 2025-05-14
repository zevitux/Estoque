using EstoqueGerenciamento.DTOs.ItemPedido;

namespace EstoqueGerenciamento.DTOs.Pedido;

public class PedidoResponseDto
{
    public Guid Id { get; set; }
    public DateTime DataPedido { get; set; }
    public decimal Total { get; set; }
    public Guid FornecedorId { get; set; }
    public string FornecedorNome { get; set; }
    public List<ItemPedidoResponseDto> ItensPedido { get; set; } = new();
}