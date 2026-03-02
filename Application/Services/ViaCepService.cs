using Application.Configurations;
using Application.DTOs.Responses;
using Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Domain.Extensions;

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
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}{cep}/json/");

            if (!response.IsSuccessStatusCode)
                throw new DomainException("Erro ao consultar CEP.");

            var content = await response.Content.ReadAsStringAsync();

            var endereco = JsonSerializer.Deserialize<ViaCepResponse>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (endereco == null || endereco.IsErro)
                throw new DomainException("CEP inválido.");

            return endereco;
        }
        catch (HttpRequestException)
        {
            throw new DomainException("Não foi possível acessar o serviço de ViaCEP.");
        }
        catch (TaskCanceledException)
        {
            throw new DomainException("A consulta ao serviço de ViaCEP expirou.");
        }
        catch (JsonException)
        {
            throw new DomainException("Resposta inválida do serviço de CEP.");
        }
        
    }
}