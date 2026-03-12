using Application.DTOs;

namespace Application.Interfaces;

public interface ICepProvider
{
    Task<EnderecoCepResult?> GetEnderecoAsync(string cep);
}