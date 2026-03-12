namespace Application.DTOs.Request;

public class UpdatePessoaJuridicaRequest
{
    public Guid ClienteId { get; set; }
    public string Nome { get; set; }
    public string RazaoSocial { get; set; }
    public string Cnpj { get; set; }
    public List<UpdateEnderecoRequest> Enderecos { get; set; } = new();
}