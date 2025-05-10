namespace EstoqueGerenciamento.Models;

public class Pedido
{
    public Guid Id { get; set; }
    public DateTime DataPedido { get; set; }
    public decimal Total { get; set; }
    public Guid FornecedorId { get; set; }
    public Fornecedor Fornecedor { get; set; }
    public List<ItemPedido> ItensPedido { get; set; } = new();
}