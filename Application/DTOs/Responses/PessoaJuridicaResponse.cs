namespace Application.DTOs.Responses;

public class PessoaJuridicaResponse
{
    public Guid ClienteId { get; set; }
    public string Nome { get; set; }
    public string RazaoSocial { get; set; }
    public string Cnpj { get; set; }
    public List<EnderecoResponse> Enderecos { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public DateTime? DataExclusao { get; set; }
}