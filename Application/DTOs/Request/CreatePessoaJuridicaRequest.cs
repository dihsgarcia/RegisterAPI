namespace Application.DTOs.Request;

public class CreatePessoaJuridicaRequest
{
    public string Nome { get; set; }
    public string RazaoSocial { get; set; }
    public string Cnpj { get; set; }
    public List<CreateEnderecoRequest> Enderecos { get; set; }
}