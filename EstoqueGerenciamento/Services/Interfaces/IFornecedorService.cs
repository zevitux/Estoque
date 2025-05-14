using EstoqueGerenciamento.DTOs.Fornecedor;
using EstoqueGerenciamento.Models;

namespace EstoqueGerenciamento.Services.Interfaces;

public interface IFornecedorService
{
    Task<FornecedorResponseDto> CriarFornecedorAsync(FornecedorCreateDto fornecedorCreateDto);
    Task<FornecedorResponseDto> AtualizarFornecedorAsync(FornecedorUpdateDto fornecedorUpdateDto);
    Task<FornecedorResponseDto> ObterFornecedorPorIdAsync(Guid fornecedorId);
    Task<IEnumerable<FornecedorResponseDto>> ObterTodosFornecedorAsync();
    Task<bool> DeletarFornecedorAsync(Guid fornecedorId);
    Task<FornecedorResponseDto?> ObterFornecedorPorCnpjAsync(string cnpj);
}