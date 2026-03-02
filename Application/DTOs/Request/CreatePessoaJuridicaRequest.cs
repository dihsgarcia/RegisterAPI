namespace Application.DTOs.Request;

public class CreatePessoaJuridicaRequest
{
    public string RazaoSocial { get; set; }
    public string Cnpj { get; set; }
    public string Cep { get; set; }
    public string NumeroEndereco { get; set; }
}