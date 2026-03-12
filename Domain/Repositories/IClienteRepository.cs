using Domain.Entities;

namespace Domain.Repositories;

public interface IClienteRepository
{
    Task AddAsync(Cliente cliente);
    Task<Cliente?> GetByIdAsync(Guid clienteId);
    Task<Cliente?> GetByDocumentoAsync(string documento);
    Task UpdateAsync(Cliente cliente);
    Task DeleteAsync(Cliente cliente);
}
