namespace Domain.Entities;

public class PessoaJuridica
{
    public Guid Id { get; private set; }
    public string RazaoSocial { get; private set; }
    public string Cnpj { get; private set; }

    public Guid EnderecoId { get; private set; }
    public Endereco Endereco { get; private set; }

    private PessoaJuridica() { }

    public PessoaJuridica(string razaoSocial, string cnpj, Endereco endereco)
    {
        Id = Guid.NewGuid();
        RazaoSocial = razaoSocial;
        Cnpj = cnpj;
        Endereco = endereco;
        EnderecoId = endereco.Id;
    }
}