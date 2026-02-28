namespace Domain.Entities;

public class PessoaFisica
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Cpf { get; private set; }

    public Guid EnderecoId { get; private set; }
    public Endereco Endereco { get; private set; }

    private PessoaFisica() { }

    public PessoaFisica(string nome, string cpf, Endereco endereco)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Cpf = cpf;
        Endereco = endereco;
        EnderecoId = endereco.Id;
    }
}