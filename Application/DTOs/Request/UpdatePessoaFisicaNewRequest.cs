namespace Application.DTOs.Request;

public class UpdatePessoaFisicaNewRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public List<CreateEnderecoPessoa> Enderecos { get; set; } = null;
}