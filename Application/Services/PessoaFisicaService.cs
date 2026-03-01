using Application.DTOs.Request;
using Application.Interfaces;
using Domain.Entities;
using Domain.Extensions;
using Domain.Repositories;
using Domain.ValueObjects;


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
    
    public async Task<PessoaFisica> GetByCpfAsync(string cpf)
    {
        var validCpf = new Cpf(cpf);
        
        var pessoa = await _repository.GetByCpfAsync(validCpf.Numero);

        if (pessoa is null)
            throw new NotFoundException("Pessoa não encontrada.");

        return pessoa;
    }
    
    public async Task UpdateAsync(string cpf, UpdatePessoaFisicaRequest request)
    {
        var validCpf = new Cpf(cpf);
        
        var pessoa = await _repository.GetByCpfAsync(validCpf.Numero);

        if (pessoa is null)
            throw new NotFoundException("Pessoa não encontrada.");

        if (cpf != request.Cpf)
        {
            if (await _repository.CpfExisteAsync(request.Cpf))
                throw new BusinessException("CPF já cadastrado.");

            pessoa.AtualizarCpf(request.Cpf);
        }

        pessoa.AtualizarNome(request.Nome);

        var enderecoViaCep = await _viaCepService.GetEnderecoAsync(request.Cep);

        pessoa.AtualizarEndereco(
            enderecoViaCep.Cep,
            enderecoViaCep.Logradouro,
            request.NumeroEndereco,
            enderecoViaCep.Bairro,
            enderecoViaCep.Localidade,
            enderecoViaCep.Uf
        );

        await _repository.UpdateAsync(pessoa);
    }
    
    public async Task DeleteAsync(string cpf)
    {
        var validCpf = new Cpf(cpf);
        
        var pessoa = await _repository.GetByCpfAsync(validCpf.Numero);

        if (pessoa is null)
            throw new NotFoundException("Pessoa não encontrada.");

        await _repository.DeleteAsync(pessoa);
    }
}