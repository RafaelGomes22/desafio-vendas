using DesafioVendas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioVendas.Infrastructure.Data;

public class VendaDbContext(DbContextOptions<VendaDbContext> options) : DbContext(options)
{
    public DbSet<Venda> Vendas => Set<Venda>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Venda>(entity =>
        {
            entity.ToTable("Vendas");
            entity.HasKey(x => x.IdVenda);
            entity.Property(v => v.IdVenda)
           .ValueGeneratedOnAdd();
            entity.Property(x => x.Produto).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Quantidade).IsRequired();
            entity.Property(x => x.PrecoUnitario).HasPrecision(18, 2).IsRequired();
            entity.Property(x => x.DataVenda).IsRequired();
        });
    }
}
