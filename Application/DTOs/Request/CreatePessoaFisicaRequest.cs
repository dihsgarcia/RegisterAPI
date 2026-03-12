namespace Application.DTOs.Request;

public class CreatePessoaFisicaRequest
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public List<CreateEnderecoRequest> Enderecos { get; set; }
}
