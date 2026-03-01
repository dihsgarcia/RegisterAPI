using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface IViaCepService
{
    Task<ViaCepResponse> GetEnderecoAsync(string cep);
}