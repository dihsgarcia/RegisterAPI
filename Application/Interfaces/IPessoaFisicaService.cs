using Application.DTOs.Request;

namespace Application.Interfaces;

public interface IPessoaFisicaService
{
    Task<Guid> CreateAsync(CreatePessoaFisicaRequest request);
}