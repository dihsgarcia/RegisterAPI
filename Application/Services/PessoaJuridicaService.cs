using Application.DTOs.Request;
using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Mappers;
using Domain.Entities;
using Domain.Extensions;
using Domain.Repositories;
using Domain.ValueObjects;


namespace Application.Services;

public class PessoaJuridicaService : IPessoaJuridicaService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IViaCepService _viaCepService;

    public PessoaJuridicaService(
        IClienteRepository clienteRepository,
        IViaCepService viaCepService)
    {
        _clienteRepository = clienteRepository;
        _viaCepService = viaCepService;
    }

    public async Task<Guid> CreateAsync(CreatePessoaJuridicaRequest request)
    {
        var normalizeCnpjReq = new Cnpj(request.Cnpj);
        
        if (await _clienteRepository.GetPessoaJuridicaByCnpjAsync(normalizeCnpjReq.Number) != null)
            throw new BusinessException($"CNPJ: {normalizeCnpjReq.Number} já cadastrado.");
        
        var cliente = new PessoaJuridica(
            request.Nome,
            request.RazaoSocial,
            normalizeCnpjReq.Number
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
    
    public async Task<PessoaJuridicaResponse> GetByCnpjAsync(string cnpj)
    {
        var normalizeCnpjReq = new Cnpj(cnpj);

        var cliente = await _clienteRepository.GetPessoaJuridicaByCnpjAsync(normalizeCnpjReq.Number);

        if (cliente is null)
            throw new NotFoundException($"Registro para o CNPJ: {normalizeCnpjReq.Number} não encontrado.");

        return PessoaJuridicaMapper.ToResponse(cliente);
    }
    
    public async Task<PessoaJuridicaResponse> GetByIdAsync(Guid clienteId)
    {
        var cliente = await _clienteRepository.GetPessoaJuridicaByIdAsync(clienteId);

        if (cliente is null)
            throw new NotFoundException($"Registro para o ClienteId: {clienteId} não encontrado.");

        return PessoaJuridicaMapper.ToResponse(cliente);
    }

    public async Task UpdateAsync(UpdatePessoaJuridicaRequest request)
    {
        var cliente = await _clienteRepository.GetPessoaJuridicaByIdAsync(request.ClienteId);

        if (cliente is null)
            throw new NotFoundException($"Registro para o ClienteId: {request.ClienteId} não encontrado.");

        var normalizeCnpjReq = new Cnpj(request.Cnpj);

        if (normalizeCnpjReq.Number != cliente.Documento)
        {
            if (await _clienteRepository.GetPessoaJuridicaByCnpjAsync(normalizeCnpjReq.Number) != null)
                throw new BusinessException($"Já existe um cadastro com o CNPJ: {normalizeCnpjReq.Number}.");
        }
        
        cliente.Update(request.Nome, request.RazaoSocial,normalizeCnpjReq.Number);
        
        foreach (var enderecoReq in request.Enderecos)
        {
            var viaCepResponse = await _viaCepService.GetEnderecoAsync(enderecoReq.Cep);

            if (viaCepResponse.IsErro)
                throw new BusinessException($"CEP: {enderecoReq.Cep} inválido ou não encontrado.");

            if (enderecoReq.EnderecoId == null)
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
                    enderecoReq.EnderecoId.Value,
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
    
    public async Task DeleteAsync(Guid clienteId)
    {
        var cliente = await _clienteRepository.GetPessoaJuridicaByIdAsync(clienteId);

        if (cliente is null)
            throw new NotFoundException($"Registro para o ClienteId: {clienteId} não encontrado.");

        await _clienteRepository.DeleteAsync(cliente);
    }
}