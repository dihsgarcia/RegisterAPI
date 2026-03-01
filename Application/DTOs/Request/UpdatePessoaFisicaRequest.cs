namespace Application.DTOs.Request;

public class UpdatePessoaFisicaRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public string NumeroEndereco { get; set; } = string.Empty;
}