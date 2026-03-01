using Application.Configurations;
using Application.DTOs.Responses;
using Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

public class ViaCepService : IViaCepService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ViaCepService(
        HttpClient httpClient,
        IOptions<ViaCepSettings> settings)
    {
        _httpClient = httpClient;
        _baseUrl = settings.Value.ViaCepBaseUrl;
    }

    public async Task<ViaCepResponse> GetEnderecoAsync(string cep)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}{cep}/json/");

        if (!response.IsSuccessStatusCode)
            throw new Exception("Erro ao consultar CEP.");

        var content = await response.Content.ReadAsStringAsync();

        var endereco = JsonSerializer.Deserialize<ViaCepResponse>(
            content,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (endereco == null || endereco.Erro)
            throw new Exception("CEP inválido.");

        return endereco;
    }
}