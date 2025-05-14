using System.Text.RegularExpressions;
using EstoqueGerenciamento.DTOs.Fornecedor;
using EstoqueGerenciamento.Models;
using EstoqueGerenciamento.Repositories.Interfaces;
using EstoqueGerenciamento.Services.Interfaces;

namespace EstoqueGerenciamento.Services.Implementation;

public class FornecedorService : IFornecedorService
{
    private readonly IFornecedorRepository  _fornecedorRepository;
    private readonly ILogger<FornecedorService> _logger;

    public FornecedorService(IFornecedorRepository fornecedorRepository, ILogger<FornecedorService> logger)
    {
        _fornecedorRepository = fornecedorRepository;
        _logger = logger;
    }
    
    public async Task<FornecedorResponseDto> CriarFornecedorAsync(FornecedorCreateDto fornecedorCreateDto)
    {
        try
        {
            var cnpjFormatado = Regex.Replace(fornecedorCreateDto.Cnpj, @"[^\d]", "");
            
            var (isValid, mensagemErro) = await ValidarFornecedorAsync(fornecedorCreateDto, cnpjFormatado);
            
            if(!isValid)
                throw new Exception(mensagemErro);
            
            var fornecedor = new Fornecedor
            {
                Nome = fornecedorCreateDto.Nome,
                Cnpj = cnpjFormatado,
                Email = fornecedorCreateDto.Email,
                Telefone = Regex.Replace(fornecedorCreateDto.Telefone, "[^0-9]", ""),
            };

            await _fornecedorRepository.AdicionarAsync(fornecedor);

            return MapearParaResponseDto(fornecedor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar fornecedor");
            throw;
        }
    }

    public async Task<FornecedorResponseDto> AtualizarFornecedorAsync(FornecedorUpdateDto fornecedorUpdateDto)
    {
        try
        {
            var fornecedorExistente = await _fornecedorRepository.ObterPorIdAsync(fornecedorUpdateDto.Id);

            if (fornecedorExistente is null)
            {
                _logger.LogWarning($"Fornecedor com ID {fornecedorUpdateDto.Id} não encontrado para atualização");
                throw new KeyNotFoundException("Fornecedor não encontrado");
            }

            var (isValid, errorMessage) = await ValidarFornecedorParaUpdate(
                fornecedorUpdateDto.Nome, 
                fornecedorUpdateDto.Cnpj, 
                fornecedorUpdateDto.Telefone, 
                fornecedorUpdateDto.Id);

            if (!isValid)
            {
                _logger.LogError($"Validação falhou ao atualizar fornecedor: {errorMessage}");
                throw new ArgumentException(errorMessage);
            }
            
            fornecedorExistente.Nome = fornecedorUpdateDto.Nome;
            fornecedorExistente.Cnpj = Regex.Replace(fornecedorUpdateDto.Cnpj, "[^0-9]", "");
            fornecedorExistente.Email = fornecedorUpdateDto.Email;
            fornecedorExistente.Telefone = Regex.Replace(fornecedorUpdateDto.Telefone, "[^0-9]", "");
            
            await _fornecedorRepository.AtualizarAsync(fornecedorExistente);

            _logger.LogInformation($"Fornecedor ID {fornecedorUpdateDto.Id} atualizado com sucesso");
            
            return MapearParaResponseDto(fornecedorExistente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao atualizar fornecedor ID {fornecedorUpdateDto.Id}");
            throw;
        }
    }

    public async Task<FornecedorResponseDto> ObterFornecedorPorIdAsync(Guid fornecedorId)
    {
        try
        {
            var fornecedor = await _fornecedorRepository.ObterPorIdAsync(fornecedorId);

            return fornecedor is null
                ? throw new KeyNotFoundException("Fornecedor nao encontrado")
                : MapearParaResponseDto(fornecedor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter fornecedor com Id {Id}", fornecedorId);
            throw;
        }
    }

    public async Task<IEnumerable<FornecedorResponseDto>> ObterTodosFornecedorAsync()
    {
        try
        {
            var fornecedor = await _fornecedorRepository.ObterTodosAsync();
            return fornecedor.Select(MapearParaResponseDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter todos os fornecedores");
            throw;
        }
    }

    public async Task<bool> DeletarFornecedorAsync(Guid fornecedorId)
    {
        try
        {
            var fornecedor = await _fornecedorRepository.ObterPorIdAsync(fornecedorId)
                             ?? throw new KeyNotFoundException("Fornecedor nao encontrado");

            await _fornecedorRepository.RemoverAsync(fornecedor);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover fornecedor com Id {Id}", fornecedorId);
            throw;
        }
    }
    
    public async Task<FornecedorResponseDto?> ObterFornecedorPorCnpjAsync(string cnpj)
    {
        try
        {
            var cnpjNumeros = Regex.Replace(cnpj, @"[^\d]", "");
                
            if (cnpjNumeros.Length != 14)
            {
                _logger.LogWarning($"CNPJ com formato inválido: {cnpj}");
                return null;
            }

            var fornecedor = await _fornecedorRepository.ObterPorCnpjAsync(cnpjNumeros);
                
            if (fornecedor == null)
            {
                _logger.LogInformation($"Fornecedor com CNPJ {cnpj} não encontrado");
                return null;
            }

            return MapearParaResponseDto(fornecedor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao buscar fornecedor por CNPJ: {cnpj}");
            throw;
        }
    }

    private static FornecedorResponseDto MapearParaResponseDto(Fornecedor fornecedor)
    {
        return new FornecedorResponseDto
        {
            Id = fornecedor.Id,
            Nome = fornecedor.Nome,
            Cnpj = fornecedor.Cnpj,
            Email = fornecedor.Email,
            Telefone = fornecedor.Telefone
        };
    }

    private async Task<(bool isValid, string mensagemErro)> ValidarFornecedorAsync(FornecedorCreateDto dto, string cnpj)
    {
        _logger.LogInformation($"Iniciando validação do fornecedor com CNPJ: {cnpj}");
        
        if (string.IsNullOrWhiteSpace(dto.Nome))
        {
            _logger.LogWarning("Nome do fornecedor é obrigatório");
            return (false, "Nome do fornecedor é obrigatório");
        }

        if (string.IsNullOrWhiteSpace(cnpj))
        {
            _logger.LogWarning("CNPJ obrigatorio");
            return (false, "CNPJ obrigatorio");
        }

        if (cnpj.Length != 14)
        {
            _logger.LogWarning($"CNPJ Invalido (tamanho:) {cnpj}");
            return (false, "CNPJ Invalido, deve conter 14 digitos");
        }

        if (!ValidarCnpj(cnpj))
        {
            _logger.LogWarning($"CNPJ Invalido {cnpj}");
            return (false, "CNPJ Invalido");
        }

        if (await _fornecedorRepository.ExisteCnpjAsync(cnpj))
        {
            _logger.LogWarning($"CNPJ Existente {cnpj}");
            return (false, "CNPJ Existente");
        }

        if (!string.IsNullOrWhiteSpace(dto.Email) && !Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            _logger.LogWarning($"Email Invalido {dto.Email}");
            return (false, "Email Invalido");
        }
        
        var telefone = Regex.Replace(dto.Telefone, @"[^0-9]", "");
        if (telefone.Length < 10)
        {
            _logger.LogWarning($"Telefone Invalido {telefone}");
            return (false, "Telefone Invalido");
        }
        
        _logger.LogInformation("Validacao do fornecedor concluida com sucesso");
        return (true, null!);
    }

    private bool ValidarCnpj(string cnpj)
    {
        cnpj = Regex.Replace(cnpj, @"[^\d]", "");
        
        if (cnpj.Length != 14)
            return false;
        
        if (new string(cnpj[0], 14) == cnpj)
            return false;
        
        return true;
    }
    
    private async Task<(bool isValid, string mensagemErro)> ValidarFornecedorParaUpdate(string nome, string cnpj,
        string telefone, Guid fornecedorId)
    {
        // Validação do Nome
        if (string.IsNullOrWhiteSpace(nome))
            return (false, "Nome do fornecedor é obrigatório");

        if (nome.Length > 100)
            return (false, "Nome não pode exceder 100 caracteres");

        // Validação do CNPJ
        var cnpjNumeros = Regex.Replace(cnpj, "[^0-9]", "");
    
        if (cnpjNumeros.Length != 14)
            return (false, "CNPJ inválido");

        if (!ValidarCnpj(cnpjNumeros))
            return (false, "CNPJ inválido");
    
        // Verifica se o CNPJ já está em uso por outro fornecedor
        var fornecedorComMesmoCnpj = await _fornecedorRepository.ObterPorCnpjAsync(cnpjNumeros);
        if (fornecedorComMesmoCnpj != null && fornecedorComMesmoCnpj.Id != fornecedorId)
            return (false, "CNPJ já está em uso por outro fornecedor");

        // Validação do Telefone
        var telefoneNumeros = Regex.Replace(telefone, "[^0-9]", "");
    
        if (telefoneNumeros.Length < 10)
            return (false, "Telefone deve conter pelo menos 10 dígitos");

        return (true, null)!;
    }
}