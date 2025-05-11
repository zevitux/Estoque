namespace EstoqueGerenciamento.DTOs.Produto;

public class ProdutoCreateDto
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public int QuantidadeEstoque { get; set; }
    public int QuantidadeMinima { get; set; }
}