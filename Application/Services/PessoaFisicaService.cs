using Application.DTOs.Request;
using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Mappers;
using Domain.Entities;
using Domain.Extensions;
using Domain.Repositories;
using Domain.ValueObjects;


namespace Application.Services;

public class PessoaFisicaService : IPessoaFisicaService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IViaCepService _viaCepService;

    public PessoaFisicaService(
        IClienteRepository clienteRepository,
        IViaCepService viaCepService)
    {
        _clienteRepository = clienteRepository;
        _viaCepService = viaCepService;
    }

    public async Task<Guid> CreateAsync(CreatePessoaFisicaRequest request)
    {
        var normalizeCpfReq = new Cpf(request.Cpf);
        
        if (await _clienteRepository.GetByDocumentoAsync(normalizeCpfReq.Number) != null)
            throw new BusinessException($"CPF: {normalizeCpfReq.Number} já cadastrado.");
        
        var cliente = new Cliente(
            request.Nome,
            null,
            normalizeCpfReq.Number
        );
        
        foreach (var enderecoReq in request.Enderecos)
        {
            var viaCepResponse = await _viaCepService.GetEnderecoAsync(enderecoReq.Cep);

            if (viaCepResponse.IsErro)
                throw new BusinessException($"CEP: {enderecoReq.Cep} inválido ou não encontrado.");
            
            var endereco = new Endereco(
                viaCepResponse.Cep,
                viaCepResponse.Logradouro,
                enderecoReq.NumeroEndereco,
                enderecoReq.Complemento,
                viaCepResponse.Bairro,
                viaCepResponse.Localidade,
                viaCepResponse.Uf);
            
            cliente.AddEndereco(endereco);
        }
        
        await _clienteRepository.AddAsync(cliente);

        return cliente.ClienteId;
    }
    
    public async Task<PessoaFisicaResponse> GetByCpfAsync(string cpf)
    {
        var normalizeCpfReq = new Cpf(cpf);

        var cliente = await _clienteRepository.GetByDocumentoAsync(normalizeCpfReq.Number);

        if (cliente is null)
            throw new NotFoundException($"Registro para o CPF: {normalizeCpfReq.Number} não encontrado.");

        return PessoaFisicaMapper.ToResponse(cliente);
    }
    
    public async Task<PessoaFisicaResponse> GetByIdAsync(Guid id)
    {
        var cliente = await _clienteRepository.GetByIdAsync(id);

        if (cliente is null)
            throw new NotFoundException($"Registro para o Id: {id} não encontrado.");

        return PessoaFisicaMapper.ToResponse(cliente);
    }

    public async Task UpdateAsync(UpdatePessoaFisicaRequest request)
    {
        var cliente = await _clienteRepository.GetByIdAsync(request.Id);

        if (cliente is null)
            throw new NotFoundException($"Registro para o Id: {request.Id} não encontrado.");

        var normalizeCpfReq = new Cpf(request.Cpf);

        if (normalizeCpfReq.Number != cliente.Documento)
        {
            if (await _clienteRepository.GetByDocumentoAsync(normalizeCpfReq.Number) != null)
                throw new BusinessException($"Já existe um cadastro com o CPF: {normalizeCpfReq.Number}.");
        }
        
        cliente.Update(request.Nome, null,normalizeCpfReq.Number);
        
        foreach (var enderecoReq in request.Enderecos)
        {
            var viaCepResponse = await _viaCepService.GetEnderecoAsync(enderecoReq.Cep);

            if (viaCepResponse.IsErro)
                throw new BusinessException($"CEP: {enderecoReq.Cep} inválido ou não encontrado.");

            if (enderecoReq.Id == null)
            {
                var endereco = new Endereco(
                    viaCepResponse.Cep,
                    viaCepResponse.Logradouro,
                    enderecoReq.NumeroEndereco,
                    enderecoReq.Complemento,
                    viaCepResponse.Bairro,
                    viaCepResponse.Localidade,
                    viaCepResponse.Uf);
                
                cliente.AddEndereco(endereco);
            }
            else
            {
                cliente.UpdateEndereco(
                    enderecoReq.Id.Value,
                    viaCepResponse.Cep,
                    viaCepResponse.Logradouro,
                    enderecoReq.NumeroEndereco,
                    enderecoReq.Complemento,
                    viaCepResponse.Bairro,
                    viaCepResponse.Localidade,
                    viaCepResponse.Uf);
            }
        }
        
        await _clienteRepository.UpdateAsync(cliente);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var cliente = await _clienteRepository.GetByIdAsync(id);

        if (cliente is null)
            throw new NotFoundException($"Registro para o Id: {id} não encontrado.");

        await _clienteRepository.DeleteAsync(cliente);
    }
}