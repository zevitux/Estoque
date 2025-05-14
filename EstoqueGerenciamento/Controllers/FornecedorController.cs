using EstoqueGerenciamento.DTOs.Fornecedor;
using EstoqueGerenciamento.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueGerenciamento.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FornecedorController : ControllerBase
{
    private readonly IFornecedorService _fornecedorService;
    private readonly ILogger<FornecedorController> _logger;

    public FornecedorController(IFornecedorService fornecedorService, ILogger<FornecedorController> logger)
    {
        _fornecedorService = fornecedorService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CriarFornecedor(FornecedorCreateDto fornecedorDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fornecedorCriado = await _fornecedorService.CriarFornecedorAsync(fornecedorDto);

            return CreatedAtAction(nameof(ObterFornecedorPorId),
                new { id = fornecedorCriado.Id },
                fornecedorCriado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar fornecedor");
            return StatusCode(500, "Ocorreu um erro interno ao criar fornecedor");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterFornecedorPorId(Guid id)
    {
        try
        {
            var fornecedor = await _fornecedorService.ObterFornecedorPorIdAsync(id);

            return Ok(fornecedor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao obter fornecedor com Id: {id}");
            return StatusCode(500, "Ocorreu um erro interno ao obter fornecedor");
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodosFornecedores()
    {
        try
        {
            var fornecedores = await _fornecedorService.ObterTodosFornecedorAsync();
            return Ok(fornecedores);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao obter fornecedores");
            return StatusCode(500, "Ocorreu um erro interno ao obter todos os fornecedores");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarFornecedor(Guid id, FornecedorUpdateDto fornecedorDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != fornecedorDto.Id)
                return BadRequest();

            var fornecedorAtualizado = await _fornecedorService.AtualizarFornecedorAsync(fornecedorDto);

            return Ok(fornecedorAtualizado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao atualizar fornecedor com Id: {id}");
            return StatusCode(500, "Ocorreu um erro interno ao atualizar fornecedor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarFornecedor(Guid id)
    {
        try
        {
            var resultado = await _fornecedorService.DeletarFornecedorAsync(id);

            if (!resultado)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao deletar fornecedor com Id: {id}");
            return StatusCode(500, "Ocorreu um erro interno ao deletar fornecedor");
        }
    }
    
    [HttpGet("cnpj/{cnpj}")]
    public async Task<IActionResult> ObterFornecedorPorCnpj(string cnpj)
    {
        try
        {
            var fornecedor = await _fornecedorService.ObterFornecedorPorCnpjAsync(cnpj);
                
            if (fornecedor == null)
                return NotFound();
                
            return Ok(fornecedor);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "CNPJ inválido fornecido");
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao buscar fornecedor por CNPJ: {cnpj}");
            return StatusCode(500, new { Message = "Ocorreu um erro interno ao processar a requisição" });
        }
    }
}