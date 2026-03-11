using Domain.Extensions;

namespace Domain.Entities;

public class Cliente
{
    public Guid ClienteId { get; private set; }
    public string Nome { get; private set; }
    public string? RazaoSocial{ get; private set; }
    public string Documento { get; private set; }
    
    private List<Endereco> _enderecos = new();
    public IReadOnlyCollection<Endereco> Enderecos => _enderecos;
    
    public DateTime DataCriacao { get; private set; }
    
    public DateTime? DataAtualizacao { get; private set;}
    
    public DateTime? DataExclusao { get; private set; }
    
    private Cliente() { }
    
    public Cliente( string nome, string? razaoSocial, string documento )
    {
        ClienteId = Guid.NewGuid();
        Nome = nome;
        RazaoSocial = razaoSocial;
        Documento = documento;
        DataCriacao = DateTime.UtcNow;
    }
    
    public void Update(string nome, string? razaoSocial, string documento)
    {
        Nome = nome;
        RazaoSocial = razaoSocial;
        Documento = documento;
        DataAtualizacao = DateTime.UtcNow;
    }
    
    public void SoftDelete()
    {
        DataExclusao = DateTime.UtcNow;
    }

    public void AddEndereco(Endereco endereco)
    {
        endereco.SetClienteId(ClienteId);
        _enderecos.Add(endereco);
    }
    
    public void UpdateEndereco(
        Guid enderecoId,
        string cep,
        string logradouro,
        string numero,
        string? complemento,
        string bairro,
        string cidade,
        string estado)
    {
        var endereco = _enderecos.FirstOrDefault(e => e.Id == enderecoId);

        if (endereco == null)
            throw new DomainException($"Endereço não pertence ao cliente.");

        endereco.Update(
            cep,
            logradouro,
            numero,
            complemento,
            bairro,
            cidade,
            estado);
    }
    
    public void RemoveEndereco(Guid enderecoId)
    {
        var endereco = _enderecos.FirstOrDefault(e => e.Id == enderecoId);

        if (endereco != null)
            _enderecos.Remove(endereco);
    }
    
}