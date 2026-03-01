using Application.DTOs.Request;
using Domain.Entities;

namespace Application.Interfaces;

public interface IPessoaFisicaService
{
    Task<Guid> CreateAsync(CreatePessoaFisicaRequest request);
    Task<PessoaFisica> GetByCpfAsync(string cpf);
    Task UpdateAsync(string cpf, UpdatePessoaFisicaRequest request);
    Task DeleteAsync(string cpf);
}