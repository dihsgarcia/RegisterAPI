namespace Application.DTOs.Request;

public class CreatePessoaFisicaRequest
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Cep { get; set; }
    public string NumeroEndereco { get; set; }
}