namespace Application.DTOs.Request;

public class UpdateEnderecoRequest
{
    public Guid? EnderecoId { get; set; }
    public string Cep { get; set; }
    public string NumeroEndereco { get; set; }
    public string? Complemento { get; set; }
}