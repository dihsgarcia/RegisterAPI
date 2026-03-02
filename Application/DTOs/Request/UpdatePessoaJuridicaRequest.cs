namespace Application.DTOs.Request;

public class UpdatePessoaJuridicaRequest
{
    public string RazaoSocial { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public string NumeroEndereco { get; set; } = string.Empty;
}