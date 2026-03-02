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
    
    public void AtualizarRazaoSocial(string razaoSocial)
    {
        RazaoSocial = razaoSocial;
    }

    public void AtualizarCnpj(string cnpj)
    {
        Cnpj = cnpj;
    }

    public void AtualizarEndereco(string cep, string logradouro, string numero, string bairro, string cidade, string uf)
    {
        Endereco.Atualizar(
            cep,
            logradouro,
            numero,
            bairro,
            cidade,
            uf);
    }
}