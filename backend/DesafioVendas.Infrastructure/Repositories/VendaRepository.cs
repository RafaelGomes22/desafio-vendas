using DesafioVendas.Domain.Dtos;
using DesafioVendas.Domain.Entities;
using DesafioVendas.Domain.Interfaces;
using DesafioVendas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DesafioVendas.Infrastructure.Repositories;

public class VendaRepository(VendaDbContext context) : IVendaRepository
{
    public async Task<IReadOnlyList<Venda>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await context.Vendas
            .AsNoTracking()
            .OrderBy(x => x.DataVenda)
            .ThenBy(x => x.Produto)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Venda>> GetFilteredAsync(
        int? quantidadeMin,
        int? quantidadeMax,
        DateTime? dataInicial,
        DateTime? dataFinal,
        CancellationToken cancellationToken = default)
    {
        var query = context.Vendas.AsNoTracking().AsQueryable();

        if (quantidadeMin.HasValue)
            query = query.Where(x => x.Quantidade >= quantidadeMin.Value);

        if (quantidadeMax.HasValue)
            query = query.Where(x => x.Quantidade <= quantidadeMax.Value);

        if (dataInicial.HasValue)
            query = query.Where(x => x.DataVenda.Date >= dataInicial.Value.Date);

        if (dataFinal.HasValue)
            query = query.Where(x => x.DataVenda.Date <= dataFinal.Value.Date);

        return await query
            .OrderBy(x => x.DataVenda)
            .ThenBy(x => x.Produto)
            .ToListAsync(cancellationToken);
    }

    public Task<Venda?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        context.Vendas
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdVenda == id, cancellationToken);

    public async Task<Venda> AddAsync(
        CreateVenda venda,
        CancellationToken cancellationToken = default)
    {
        var entity = new Venda
        {
            Produto = venda.Produto,
            Quantidade = venda.Quantidade,
            PrecoUnitario = venda.PrecoUnitario,
            DataVenda = venda.DataVenda
        };

        context.Vendas.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateVenda venda,
        CancellationToken cancellationToken = default)
    {
        var entity = await context.Vendas
            .FirstOrDefaultAsync(x => x.IdVenda == id, cancellationToken);

        if (entity is null)
            return false;

        entity.Produto = venda.Produto;
        entity.Quantidade = venda.Quantidade;
        entity.PrecoUnitario = venda.PrecoUnitario;
        entity.DataVenda = venda.DataVenda;

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var venda = await context.Vendas.FindAsync([id], cancellationToken);

        if (venda is null)
            return false;

        context.Vendas.Remove(venda);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
