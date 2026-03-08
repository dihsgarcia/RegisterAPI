namespace Application.DTOs.Request;

public class CreatePessoaJuridicaNewRequest
{
    public string Nome { get; set; }
    public string RazaoSocial { get; set; }
    public string Cnpj { get; set; }
    public List<CreateEnderecoPessoa> Enderecos { get; set; }
}