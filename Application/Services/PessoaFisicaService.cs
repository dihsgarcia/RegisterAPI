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
    private readonly ICepService _cepService;

    public PessoaFisicaService(
        IClienteRepository clienteRepository,
        ICepService cepService)
    {
        _clienteRepository = clienteRepository;
        _cepService = cepService;
    }

    public async Task<Guid> CreateAsync(CreatePessoaFisicaRequest request)
    {
        var normalizeCpfReq = new Cpf(request.Cpf);

        if (await _clienteRepository.GetPessoaFisicaByCpfAsync(normalizeCpfReq.Number) != null)
            throw new BusinessException($"CPF: {normalizeCpfReq.Number} já cadastrado.");

        var cliente = new PessoaFisica(
            request.Nome,
            normalizeCpfReq.Number
        );

        foreach (var enderecoReq in request.Enderecos)
        {
            var cepResponse = await _cepService.GetEnderecoAsync(enderecoReq.Cep);

            if (cepResponse is null)
                throw new BusinessException($"CEP: {enderecoReq.Cep} inválido ou não encontrado.");

            var endereco = new Endereco(
                cepResponse.Cep,
                cepResponse.Logradouro,
                enderecoReq.NumeroEndereco,
                enderecoReq.Complemento,
                cepResponse.Bairro,
                cepResponse.Cidade,
                cepResponse.Estado);

            cliente.AddEndereco(endereco);
        }

        await _clienteRepository.AddAsync(cliente);

        return cliente.ClienteId;
    }

    public async Task<PessoaFisicaResponse> GetByCpfAsync(string cpf)
    {
        var normalizeCpfReq = new Cpf(cpf);

        var cliente = await _clienteRepository.GetPessoaFisicaByCpfAsync(normalizeCpfReq.Number);

        if (cliente is null)
            throw new NotFoundException($"Registro para o CPF: {normalizeCpfReq.Number} não encontrado.");

        return PessoaFisicaMapper.ToResponse(cliente);
    }

    public async Task<PessoaFisicaResponse> GetByIdAsync(Guid clienteId)
    {
        var cliente = await _clienteRepository.GetPessoaFisicaByIdAsync(clienteId);

        if (cliente is null)
            throw new NotFoundException($"Registro para o ClienteId: {clienteId} não encontrado.");

        return PessoaFisicaMapper.ToResponse(cliente);
    }

    public async Task UpdateAsync(UpdatePessoaFisicaRequest request)
    {
        var cliente = await _clienteRepository.GetPessoaFisicaByIdAsync(request.ClienteId);

        if (cliente is null)
            throw new NotFoundException($"Registro para o ClienteId: {request.ClienteId} não encontrado.");

        var normalizeCpfReq = new Cpf(request.Cpf);

        if (normalizeCpfReq.Number != cliente.Documento)
        {
            if (await _clienteRepository.GetPessoaFisicaByCpfAsync(normalizeCpfReq.Number) != null)
                throw new BusinessException($"Já existe um cadastro com o CPF: {normalizeCpfReq.Number}.");
        }

        cliente.Update(request.Nome, normalizeCpfReq.Number);

        foreach (var enderecoReq in request.Enderecos)
        {
            var cepResponse = await _cepService.GetEnderecoAsync(enderecoReq.Cep);

            if (cepResponse is null)
                throw new BusinessException($"CEP: {enderecoReq.Cep} inválido ou não encontrado.");

            if (enderecoReq.EnderecoId == null)
            {
                var endereco = new Endereco(
                    cepResponse.Cep,
                    cepResponse.Logradouro,
                    enderecoReq.NumeroEndereco,
                    enderecoReq.Complemento,
                    cepResponse.Bairro,
                    cepResponse.Cidade,
                    cepResponse.Estado);

                cliente.AddEndereco(endereco);
            }
            else
            {
                cliente.UpdateEndereco(
                    enderecoReq.EnderecoId.Value,
                    cepResponse.Cep,
                    cepResponse.Logradouro,
                    enderecoReq.NumeroEndereco,
                    enderecoReq.Complemento,
                    cepResponse.Bairro,
                    cepResponse.Cidade,
                    cepResponse.Estado);
            }
        }

        await _clienteRepository.UpdateAsync(cliente);
    }

    public async Task DeleteAsync(Guid clienteId)
    {
        var cliente = await _clienteRepository.GetPessoaFisicaByIdAsync(clienteId);

        if (cliente is null)
            throw new NotFoundException($"Registro para o ClienteId: {clienteId} não encontrado.");

        await _clienteRepository.DeleteAsync(cliente);
    }

    
}