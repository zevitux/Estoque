using EstoqueGerenciamento.Repositories.Interfaces;

namespace EstoqueGerenciamento.Services.Implementation;

public class RelatorioEstoqueService
{
    private readonly IProdutoRepository _produtoRepository;

    public RelatorioEstoqueService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<string> GerarRelatorioEstoqueBaixoAsync()
    {
        var produtos = await _produtoRepository.ObterComEstoqueBaixo();
        
        var caminho = Path.Combine(AppContext.BaseDirectory, "RelatorioEstoqueBaixo.pdf");
    }
}