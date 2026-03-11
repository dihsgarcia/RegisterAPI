using Application.DTOs.Request;
using Application.DTOs.Responses;
using Domain.Entities;

namespace Application.Interfaces;

public interface IPessoaFisicaService
{
    Task<Guid> CreateAsync(CreatePessoaFisicaRequest request);
    Task<PessoaFisicaResponse> GetByIdAsync(Guid id);
    Task<PessoaFisicaResponse> GetByCpfAsync(string cpf);
    Task UpdateAsync(UpdatePessoaFisicaRequest request);
    Task DeleteAsync(Guid id);
}