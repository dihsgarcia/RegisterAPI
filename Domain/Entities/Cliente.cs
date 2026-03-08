namespace Domain.Entities;

public class Cliente
{
    public Guid ClienteId { get; private set; }
    public string Nome { get; private set; }
    public string? RazaoSocial{ get; private set; }
    public string Documento { get; private set; }
    public List<EnderecoNew> Enderecos { get; private set; }

    public DateTime DataCriacao { get; private set; }
    
    public DateTime? DataAtualizacao { get; private set;}
    
    public DateTime? DataExclusao { get; private set; }
    
    private Cliente() { }
    
    public Cliente(
        string nome, 
        string? razaoSocial, 
        string documento, 
        List<EnderecoNew> enderecos,
        DateTime? dataAtualizacao,
        DateTime? dataExclusao)
    {
        ClienteId = Guid.NewGuid();
        Nome = nome;
        RazaoSocial = razaoSocial;
        Documento = documento;
        Enderecos = enderecos;
        DataCriacao = DateTime.UtcNow;
        DataAtualizacao = dataAtualizacao;
        DataExclusao = dataExclusao;
    }
    
    public void atualizar(
        string nome, 
        string? razaoSocial, 
        string documento, 
        List<EnderecoNew> enderecos,
        DateTime? dataAtualizacao)
    {
        Nome = nome;
        RazaoSocial = razaoSocial;
        Documento = documento;
        Enderecos = enderecos;
        DataAtualizacao = DateTime.UtcNow;;
    }
    
    public void Excluir()
    {
        DataAtualizacao = DateTime.UtcNow;;
    }
    
}