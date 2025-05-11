namespace EstoqueGerenciamento.DTOs.Fornecedor;

public class FornecedorUpdateDto
{
    public Guid Id { get; set; }  
    public string Nome { get; set; }
    public string Cnpj { get; set; } 
    public string Email { get; set; }
    public string Telefone { get; set; }
}