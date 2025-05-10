namespace EstoqueGerenciamento.Models;

public class Fornecedor
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Cnpj { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public List<Pedido> Pedidos { get; set; } = new();
}