namespace Domain.Entities;

public class Endereco
{
    public Guid Id { get; private set; }
    public string Cep { get; private set; }
    public string Logradouro { get; private set; }
    public string Numero { get; private set; }
    public string Bairro { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }

    private Endereco() { }

    public Endereco(string cep, string logradouro, string numero, string bairro, string cidade, string estado)
    {
        Id = Guid.NewGuid();
        Cep = cep;
        Logradouro = logradouro;
        Numero = numero;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
    }
    
    public void Atualizar( string cep, string logradouro, string numero, string bairro, string cidade, string estado)
    {
        Cep = cep;
        Logradouro = logradouro;
        Numero = numero;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
    }
}