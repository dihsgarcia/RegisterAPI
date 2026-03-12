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

        if (await _clienteRepository.GetPessoaFisicaByCpfAsync(normalizeCpfReq.Number) != null)
            throw new BusinessException($"CPF: {normalizeCpfReq.Number} já cadastrado.");

        var cliente = new PessoaFisica(
            request.Nome,
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
        var cliente = await _clienteRepository.GetPessoaFisicaByIdAsync(clienteId);

        if (cliente is null)
            throw new NotFoundException($"Registro para o ClienteId: {clienteId} não encontrado.");

        await _clienteRepository.DeleteAsync(cliente);
    }

    
}