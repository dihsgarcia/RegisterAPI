using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PessoaFisicaRepository : IPessoaFisicaRepository
{
    private readonly AppDbContext _context;

    public PessoaFisicaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PessoaFisica pessoa)
    {
        await _context.PessoasFisicas.AddAsync(pessoa);
        await _context.SaveChangesAsync();
    }

    public async Task<PessoaFisica?> GetByCpfAsync(string cpf)
    {
        return await _context.PessoasFisicas
            .Include(p => p.Endereco)
            .FirstOrDefaultAsync(x => x.Cpf == cpf);
    }

    public async Task UpdateAsync(PessoaFisica pessoa)
    {
        _context.PessoasFisicas.Update(pessoa);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PessoaFisica pessoa)
    {
        _context.PessoasFisicas.Remove(pessoa);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CpfExisteAsync(string cpf)
    {
        return await _context.PessoasFisicas
            .AnyAsync(x => x.Cpf == cpf);
    }
}