using Application.DTOs.Request;
using Application.Interfaces;
using Domain.Entities;
using Domain.Extensions;
using Domain.Repositories;
using Domain.ValueObjects;


namespace Application.Services;

public class PessoaJuridicaService : IPessoaJuridicaService
{
    private readonly IPessoaJuridicaRepository _repository;
    private readonly IViaCepService _viaCepService;

    public PessoaJuridicaService(
        IPessoaJuridicaRepository repository,
        IViaCepService viaCepService)
    {
        _repository = repository;
        _viaCepService = viaCepService;
    }

    public async Task<Guid> CreateAsync(CreatePessoaJuridicaRequest request)
    {
        var validCnpj = new Cnpj(request.Cnpj);
        
        if (await _repository.CnpjExisteAsync(validCnpj.Numero))
            throw new BusinessException("CNPJ já cadastrado.");

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

        var pessoa = new PessoaJuridica(
            request.RazaoSocial,
            validCnpj.Numero,
            endereco
        );

        await _repository.AddAsync(pessoa);

        return pessoa.Id;
    }
    
    public async Task<PessoaJuridica> GetByCnpjAsync(string cnpj)
    {
        var validCnpj = new Cnpj(cnpj);
        
        var pessoa = await _repository.GetByCnpjAsync(validCnpj.Numero);

        if (pessoa is null)
            throw new NotFoundException("Pessoa não encontrada.");

        return pessoa;
    }
    
    public async Task UpdateAsync(string cnpj, UpdatePessoaJuridicaRequest request)
    {
        var validCnpj = new Cnpj(cnpj);
        
        var pessoa = await _repository.GetByCnpjAsync(validCnpj.Numero);
        
        if (pessoa is null)
            throw new NotFoundException("Pessoa não encontrada.");

        var validUpdateCnpj = new Cnpj(request.Cnpj);
        
        if (validCnpj.Numero != validUpdateCnpj.Numero)
        {
            if (await _repository.CnpjExisteAsync(validUpdateCnpj.Numero))
                throw new BusinessException($"Já existe um cadastro com o CNPJ {validUpdateCnpj.Numero}");

            pessoa.AtualizarCnpj(validUpdateCnpj.Numero);
        }

        pessoa.AtualizarRazaoSocial(request.RazaoSocial);

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
    
    public async Task DeleteAsync(string cnpj)
    {
        var validCnpj = new Cnpj(cnpj);
        
        var pessoa = await _repository.GetByCnpjAsync(validCnpj.Numero);

        if (pessoa is null)
            throw new NotFoundException("Pessoa não encontrada.");

        await _repository.DeleteAsync(pessoa);
    }
}