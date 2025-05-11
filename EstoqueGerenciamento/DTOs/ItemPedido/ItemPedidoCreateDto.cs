namespace EstoqueGerenciamento.DTOs.ItemPedido;

public class ItemPedidoCreateDto
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}