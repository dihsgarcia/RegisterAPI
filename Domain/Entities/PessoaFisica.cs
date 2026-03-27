namespace Domain.Entities;

public class PessoaFisica : Cliente
{
    
    private PessoaFisica() { }

    public PessoaFisica(string nome, string cpf) : base(nome,cpf)
    {
        Nome = nome;
    }

    public void Update(string nome, string cpf)
    {
        Nome = nome;
        Documento = cpf;
        DataAtualizacao = DateTime.UtcNow;
    }
}