using Domain.Entities;

namespace Domain.Repositories;

public interface IPessoaFisicaRepository
{
    Task AddAsync(PessoaFisica pessoa);
    Task<bool> CpfExisteAsync(string cpf);
    Task<PessoaFisica?> GetByCpfAsync(string cpf);
    Task UpdateAsync(PessoaFisica pessoa);
    Task DeleteAsync(PessoaFisica pessoa);
}