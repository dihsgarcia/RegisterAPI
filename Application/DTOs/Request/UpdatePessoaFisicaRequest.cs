namespace Application.DTOs.Request;

public class UpdatePessoaFisicaRequest
{
    public Guid ClienteId { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public List<UpdateEnderecoRequest> Enderecos { get; set; } = new();
}