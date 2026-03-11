namespace Application.DTOs.Responses;

public class PessoaFisicaResponse
{
    public Guid ClienteId { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public List<EnderecoResponse> Enderecos { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public DateTime? DataExclusao { get; set; }
}
