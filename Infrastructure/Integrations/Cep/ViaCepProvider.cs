using System.Text.Json;
using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Integrations.Cep;

public class ViaCepProvider : ICepProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly ILogger<ViaCepProvider> _logger;

    public ViaCepProvider(
        HttpClient httpClient, 
        IOptions<ViaCepSettings> settings,
        ILogger<ViaCepProvider> logger)
    {
        _httpClient = httpClient;
        _baseUrl = settings.Value.ViaCepBaseUrl;
        _logger = logger;
    }

    public async Task<EnderecoCepResult?> GetEnderecoAsync(string cep)
    {
        _logger.LogInformation("Consultando CEP? {Cep} no ViaCEP", cep);

        var response = await _httpClient.GetAsync($"{_baseUrl}{cep}/json/");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Falha ao consultar CEP {Cep}. StatusCode: {StatusCode}", cep, response.StatusCode);
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<ViaCepResponse>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (data == null || data.IsErro)
            return null;

        return new EnderecoCepResult
        {
            Cep = data.Cep,
            Logradouro = data.Logradouro,
            Bairro = data.Bairro,
            Cidade = data.Localidade,
            Estado = data.Uf
        };
    }
}