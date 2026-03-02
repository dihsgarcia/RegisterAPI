using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PessoaJuridicaRepository : IPessoaJuridicaRepository
{
    private readonly AppDbContext _context;

    public PessoaJuridicaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PessoaJuridica pessoa)
    {
        await _context.PessoasJuridicas.AddAsync(pessoa);
        await _context.SaveChangesAsync();
    }

    public async Task<PessoaJuridica?> GetByCnpjAsync(string cnpj)
    {
        return await _context.PessoasJuridicas
            .Include(p => p.Endereco)
            .FirstOrDefaultAsync(x => x.Cnpj == cnpj);
    }

    public async Task UpdateAsync(PessoaJuridica pessoa)
    {
        _context.PessoasJuridicas.Update(pessoa);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PessoaJuridica pessoa)
    {
        _context.PessoasJuridicas.Remove(pessoa);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CnpjExisteAsync(string cnpj)
    {
        return await _context.PessoasJuridicas
            .AnyAsync(x => x.Cnpj == cnpj);
    }
}