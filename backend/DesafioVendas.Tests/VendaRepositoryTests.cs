using DesafioVendas.Domain.Dtos;
using DesafioVendas.Infrastructure.Data;
using DesafioVendas.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DesafioVendas.Tests;

public class VendaRepositoryTests
{
    private static VendaDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<VendaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new VendaDbContext(options);
    }

    [Fact]
    public async Task DeveAdicionarVendaSemInformarIdEReceberIdGerado()
    {
        await using var context = CreateContext();
        var repository = new VendaRepository(context);

        var venda = await repository.AddAsync(new CreateVenda
        {
            Produto = "Camiseta",
            Quantidade = 3,
            PrecoUnitario = 49.90m,
            DataVenda = new DateTime(2026, 9, 6)
        });

        var resultado = await repository.GetByIdAsync(venda.IdVenda);

        Assert.NotNull(resultado);
        Assert.True(venda.IdVenda > 0);
        Assert.Equal("Camiseta", resultado.Produto);
        Assert.Equal(3, resultado.Quantidade);
    }

    [Fact]
    public async Task DeveAtualizarVendaPeloIdDaRota()
    {
        await using var context = CreateContext();
        var repository = new VendaRepository(context);

        var criada = await repository.AddAsync(new CreateVenda
        {
            Produto = "Camiseta",
            Quantidade = 3,
            PrecoUnitario = 49.90m,
            DataVenda = new DateTime(2026, 9, 6)
        });

        var atualizada = await repository.UpdateAsync(
            criada.IdVenda,
            new UpdateVenda
            {
                Produto = "Camiseta Premium",
                Quantidade = 5,
                PrecoUnitario = 59.90m,
                DataVenda = new DateTime(2026, 9, 7)
            });

        var resultado = await repository.GetByIdAsync(criada.IdVenda);

        Assert.True(atualizada);
        Assert.NotNull(resultado);
        Assert.Equal("Camiseta Premium", resultado.Produto);
        Assert.Equal(5, resultado.Quantidade);
        Assert.Equal(59.90m, resultado.PrecoUnitario);
        Assert.Equal(criada.IdVenda, resultado.IdVenda);
    }

    [Fact]
    public async Task DeveFiltrarPorQuantidade()
    {
        await using var context = CreateContext();
        context.Vendas.AddRange(
            new DesafioVendas.Domain.Entities.Venda
            {
                Produto = "A",
                Quantidade = 1,
                PrecoUnitario = 10,
                DataVenda = DateTime.Today
            },
            new DesafioVendas.Domain.Entities.Venda
            {
                Produto = "B",
                Quantidade = 5,
                PrecoUnitario = 20,
                DataVenda = DateTime.Today
            });

        await context.SaveChangesAsync();

        var repository = new VendaRepository(context);
        var resultado = await repository.GetFilteredAsync(5, null, null, null);

        Assert.Single(resultado);
        Assert.Equal("B", resultado[0].Produto);
    }
}
