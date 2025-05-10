namespace EstoqueGerenciamento.Models;

public class Produto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public int QuantidadeEstoque { get; set; }
    public int QuantidadeMinima { get; set; }
    public List<ItemPedido> ItensPedido { get; set; } = new();
}