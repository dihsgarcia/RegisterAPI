using Domain.Extensions;

namespace Domain.Entities;

public abstract class Cliente
{
    public Guid ClienteId { get; protected set; }
    public string Nome { get; protected set; }
    public string Documento { get; protected set; }

    private List<Endereco> _enderecos = new();
    public IReadOnlyCollection<Endereco> Enderecos => _enderecos;

    public DateTime DataCriacao { get; protected set; }
    public DateTime? DataAtualizacao { get; protected set; }
    public DateTime? DataExclusao { get; protected set; }

    protected Cliente()
    {
    }

    protected Cliente(string nome, string documento)
    {
        ClienteId = Guid.NewGuid();
        Nome = nome;
        Documento = documento;
        DataCriacao = DateTime.UtcNow;
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
        var endereco = _enderecos.FirstOrDefault(e => e.EnderecoId == enderecoId);

        if (endereco == null)
            throw new DomainException($"EnderecoId: {enderecoId} não encontrado.");

        endereco.Update(
            cep,
            logradouro,
            numero,
            complemento,
            bairro,
            cidade,
            estado);
    }
}