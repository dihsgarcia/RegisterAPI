using Application.DTOs.Request;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;


namespace Application.Services;

public class PessoaFisicaService : IPessoaFisicaService
{
    private readonly IPessoaFisicaRepository _repository;
    private readonly IViaCepService _viaCepService;

    public PessoaFisicaService(
        IPessoaFisicaRepository repository,
        IViaCepService viaCepService)
    {
        _repository = repository;
        _viaCepService = viaCepService;
    }

    public async Task<Guid> CreateAsync(CreatePessoaFisicaRequest request)
    {
        if (await _repository.CpfExisteAsync(request.Cpf))
            throw new Exception("CPF já cadastrado.");

        var enderecoViaCep = await _viaCepService
            .GetEnderecoAsync(request.Cep);

        var endereco = new Endereco(
            enderecoViaCep.Cep,
            enderecoViaCep.Logradouro,
            request.NumeroEndereco, 
            enderecoViaCep.Bairro,
            enderecoViaCep.Localidade,
            enderecoViaCep.Uf
        );

        var pessoa = new PessoaFisica(
            request.Nome,
            request.Cpf,
            endereco
        );

        await _repository.AddAsync(pessoa);

        return pessoa.Id;
    }
}