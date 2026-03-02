using Domain.Entities;

namespace Domain.Repositories;

public interface IPessoaJuridicaRepository
{
    Task AddAsync(PessoaJuridica pessoa);
    Task<bool> CnpjExisteAsync(string cnpj);
    Task<PessoaJuridica?> GetByCnpjAsync(string cnpj);
    Task UpdateAsync(PessoaJuridica pessoa);
    Task DeleteAsync(PessoaJuridica pessoa);
}