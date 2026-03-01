using Domain.Entities;

namespace Domain.Repositories;

public interface IPessoaFisicaRepository
{
    Task AddAsync(PessoaFisica pessoa);
    Task<bool> CpfExisteAsync(string cpf);
}