namespace Application.DTOs.Request;

public class CreateEnderecoRequest
{
    public string Cep { get; set; }
    public string NumeroEndereco { get; set; }
    public string? Complemento { get; set; }
}
