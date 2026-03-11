using Application.DTOs.Request;
using Domain.Entities;

namespace Application.Interfaces;

public interface IPessoaJuridicaService
{
    Task<Guid> CreateAsync(CreatePessoaJuridicaRequest request);
    Task UpdateAsync(string cnpj, UpdatePessoaJuridicaRequest request);
    Task DeleteAsync(string cnpj);
}