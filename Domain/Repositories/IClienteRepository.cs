using Domain.Entities;

namespace Domain.Repositories;

public interface IClienteRepository
{
    Task AddAsync(Cliente cliente);

    Task<PessoaFisica?> GetPessoaFisicaByIdAsync(Guid id);

    Task<PessoaJuridica?> GetPessoaJuridicaByIdAsync(Guid id);

    Task<PessoaFisica?> GetPessoaFisicaByCpfAsync(string cpf);

    Task<PessoaJuridica?> GetPessoaJuridicaByCnpjAsync(string cnpj);

    Task UpdateAsync(Cliente cliente);

    Task DeleteAsync(Cliente cliente);
}
