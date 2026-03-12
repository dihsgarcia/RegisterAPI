using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task<Cliente?> GetByIdAsync(Guid clienteId)
    {
        return await _context.Clientes
            .Include(c => c.Enderecos)
            .FirstOrDefaultAsync(c => c.ClienteId == clienteId && c.DataExclusao == null);
    }

    public async Task<Cliente?> GetByDocumentoAsync(string documento)
    {
        return await _context.Clientes
            .Include(c => c.Enderecos)
            .FirstOrDefaultAsync(c => c.Documento == documento && c.DataExclusao == null);
    }

    
    public async Task UpdateAsync(Cliente cliente)
    {
        var teste = _context.ChangeTracker.Entries<Endereco>();
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(Cliente cliente)
    {
        cliente.SoftDelete();
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
    }
}
