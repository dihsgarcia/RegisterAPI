namespace Domain.Entities;

public class PessoaJuridica : Cliente
{
    public string RazaoSocial { get; private set; }

    private PessoaJuridica() { }

    public PessoaJuridica(string nome, string razaoSocial, string cnpj)
        : base(nome, cnpj)
    {
        RazaoSocial = razaoSocial;
    }

    public void Update(string nome, string razaoSocial, string cnpj)
    {
        Nome = nome;
        RazaoSocial = razaoSocial;
        Documento = cnpj;
        DataAtualizacao = DateTime.UtcNow;
    }
}