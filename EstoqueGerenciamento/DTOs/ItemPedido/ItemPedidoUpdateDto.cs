namespace EstoqueGerenciamento.DTOs.ItemPedido;

public class ItemPedidoUpdateDto
{
    public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}