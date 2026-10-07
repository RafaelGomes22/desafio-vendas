using DesafioVendas.Domain.Dtos;
using DesafioVendas.Domain.Entities;
using DesafioVendas.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesafioVendas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VendasController(IVendaRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Venda>>> Get(
        [FromQuery] int? quantidadeMin,
        [FromQuery] int? quantidadeMax,
        [FromQuery] DateTime? dataInicial,
        [FromQuery] DateTime? dataFinal,
        CancellationToken cancellationToken)
    {
        var vendas = await repository.GetFilteredAsync(
            quantidadeMin, quantidadeMax, dataInicial, dataFinal, cancellationToken);

        return Ok(vendas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Venda>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var venda = await repository.GetByIdAsync(id, cancellationToken);

        return venda is null ? NotFound() : Ok(venda);
    }

    [HttpPost]
    public async Task<ActionResult<Venda>> Post(
        CreateVenda venda,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(venda.Produto))
            return BadRequest("Produto é obrigatório.");

        if (venda.Quantidade <= 0)
            return BadRequest("Quantidade deve ser maior que zero.");

        if (venda.PrecoUnitario < 0)
            return BadRequest("Preço unitário não pode ser negativo.");

        var criada = await repository.AddAsync(venda, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = criada.IdVenda },
            criada);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(
        int id,
        UpdateVenda venda,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(venda.Produto))
            return BadRequest("Produto é obrigatório.");

        if (venda.Quantidade <= 0)
            return BadRequest("Quantidade deve ser maior que zero.");

        if (venda.PrecoUnitario < 0)
            return BadRequest("Preço unitário não pode ser negativo.");

        var atualizada = await repository.UpdateAsync(
            id, venda, cancellationToken);

        return atualizada ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var removida = await repository.DeleteAsync(id, cancellationToken);

        return removida ? NoContent() : NotFound();
    }
}
