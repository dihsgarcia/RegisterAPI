using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class CepService : ICepService
{
    private readonly IEnumerable<ICepProvider> _providers;

    public CepService(IEnumerable<ICepProvider> providers)
    {
        _providers = providers;
    }

    public async Task<EnderecoCepResult?> GetEnderecoAsync(string cep)
    {
        foreach (var provider in _providers)
        {
            var result = await provider.GetEnderecoAsync(cep);

            if (result != null)
                return result;
        }

        return null;
    }
}