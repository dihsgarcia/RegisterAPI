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

    public async Task<PessoaFisica?> GetPessoaFisicaByIdAsync(Guid clienteId)
    {
        return await _context.PessoasFisicas
            .Include(x => x.Enderecos)
            .FirstOrDefaultAsync(x =>
                x.ClienteId == clienteId &&
                x.DataExclusao == null);
    }

    public async Task<PessoaJuridica?> GetPessoaJuridicaByIdAsync(Guid clienteId)
    {
        return await _context.PessoasJuridicas
            .Include(x => x.Enderecos)
            .FirstOrDefaultAsync(x =>
                x.ClienteId == clienteId &&
                x.DataExclusao == null);
    }

    public async Task<PessoaFisica?> GetPessoaFisicaByCpfAsync(string cpf)
    {
        return await _context.PessoasFisicas
            .Include(x => x.Enderecos)
            .FirstOrDefaultAsync(x =>
                x.Documento == cpf &&
                x.DataExclusao == null);
    }

    public async Task<PessoaJuridica?> GetPessoaJuridicaByCnpjAsync(string cnpj)
    {
        return await _context.PessoasJuridicas
            .Include(x => x.Enderecos)
            .FirstOrDefaultAsync(x =>
                x.Documento == cnpj &&
                x.DataExclusao == null);
    }

    public async Task UpdateAsync(Cliente cliente)
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Cliente cliente)
    {
        cliente.SoftDelete();
        await _context.SaveChangesAsync();
    }
}
