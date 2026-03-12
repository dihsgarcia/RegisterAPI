using Application.DTOs;

namespace Application.Interfaces;

public interface ICepService
{
    Task<EnderecoCepResult?> GetEnderecoAsync(string cep);
}