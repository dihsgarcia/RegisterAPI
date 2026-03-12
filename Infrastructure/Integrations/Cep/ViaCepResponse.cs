namespace Infrastructure.Integrations.Cep;

public class ViaCepResponse
{
    public string Cep { get; set; }
    public string Logradouro { get; set; }
    public string Bairro { get; set; }
    public string Localidade { get; set; }
    public string Uf { get; set; }
    public string Erro { get; set; }
    public bool IsErro => !string.IsNullOrEmpty(Erro) && Erro.Equals("true", StringComparison.OrdinalIgnoreCase);
}