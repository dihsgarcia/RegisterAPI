namespace Domain.Entities;

public class EnderecoNew
{
    public Guid Id { get; private set; }
    public Guid ClienteId { get; private set; }
    public string Cep { get; private set; }
    public string Logradouro { get; private set; }
    public string Numero { get; private set; }
    public string Complemento { get; private set; }
    public string Bairro { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    
    public DateTime DataCriacao { get; private set; }
    
    public DateTime? DataAtualizacao { get; private set;}
    
    public DateTime? DataExclusao { get; private set; }

    private EnderecoNew() { }

    public EnderecoNew(
        Guid clienteId, 
        string cep, 
        string logradouro,
        string numero, 
        string complemento,
        string bairro, 
        string cidade, 
        string estado,
        DateTime? dataAtualizacao,
        DateTime? dataExclusao)
    {
        Id = Guid.NewGuid();
        ClienteId = clienteId;
        Cep = cep;
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        DataCriacao = DateTime.UtcNow;
        DataAtualizacao = dataAtualizacao;
        DataExclusao = dataExclusao;
    }

    public void Atualizar( 
        string cep, 
        string logradouro, 
        string numero, 
        string complemento, 
        string bairro, 
        string cidade, 
        string estado)
    {
        Cep = cep;
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        DataAtualizacao = DateTime.UtcNow;
    }
    
    public void Excluir( 
        string cep, 
        string logradouro, 
        string numero, 
        string complemento, 
        string bairro, 
        string cidade, 
        string estado)
    {
        Cep = cep;
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        DataExclusao = DateTime.UtcNow;
    }
    
}