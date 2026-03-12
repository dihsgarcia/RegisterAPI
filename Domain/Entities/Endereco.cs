namespace Domain.Entities;

public class Endereco
{
    public Guid EnderecoId { get; private set; }
    public Guid ClienteId { get; private set; }
    public string Cep { get; private set; }
    public string Logradouro { get; private set; }
    public string Numero { get; private set; }
    public string? Complemento { get; private set; }
    public string Bairro { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }

    public Cliente Cliente { get; private set; } = null!;

    private Endereco() { }

    public Endereco(
        string cep,
        string logradouro,
        string numero,
        string? complemento,
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
    }

    public void Update(
        string cep,
        string logradouro,
        string numero,
        string? complemento,
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
    }
    
    internal void SetClienteId(Guid clienteId)
    {
        ClienteId = clienteId;
    }
    
    
}
