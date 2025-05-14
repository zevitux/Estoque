using EstoqueGerenciamento.DTOs.Produto;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EstoqueGerenciamento.Reports;

public class EstoqueBaixoReport : IDocument
{
    public List<ProdutoResponseDto> Produtos { get; }

    public EstoqueBaixoReport(List<ProdutoResponseDto> produtos)
    {
        Produtos = produtos;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);
            page.Header().Text("Relatório de Produtos com Estoque Baixo").FontSize(20).Bold();
            page.Content().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.ConstantColumn(100);
                    columns.ConstantColumn(100);
                });

                table.Header(header =>
                {
                    header.Cell().Text("Quantidade de Produtos").Bold();
                    header.Cell().Text("Estoque").Bold();
                    header.Cell().Text("Estoque minimo").Bold();
                });

                foreach (var p in Produtos)
                {
                    table.Cell().Text(p.Nome);
                    table.Cell().Text(p.QuantidadeEstoque.ToString());
                    table.Cell().Text(p.QuantidadeMinima.ToString());
                }
            });
        });
    }
}