using Application.DTOs.Request;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface IPessoaJuridicaService
{
    Task<Guid> CreateAsync(CreatePessoaJuridicaRequest request);
    Task<PessoaJuridicaResponse> GetByIdAsync(Guid clienteId);
    Task<PessoaJuridicaResponse> GetByCnpjAsync(string cnpj);
    Task UpdateAsync(UpdatePessoaJuridicaRequest request);
    Task DeleteAsync(Guid clienteId);
}