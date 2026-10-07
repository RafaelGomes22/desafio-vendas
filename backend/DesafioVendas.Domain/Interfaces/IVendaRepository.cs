using DesafioVendas.Domain.Dtos;
using DesafioVendas.Domain.Entities;

namespace DesafioVendas.Domain.Interfaces;

public interface IVendaRepository
{
    Task<IReadOnlyList<Venda>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Venda>> GetFilteredAsync(
        int? quantidadeMin,
        int? quantidadeMax,
        DateTime? dataInicial,
        DateTime? dataFinal,
        CancellationToken cancellationToken = default);

    Task<Venda?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Venda> AddAsync(CreateVenda venda, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(int id, UpdateVenda venda, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
