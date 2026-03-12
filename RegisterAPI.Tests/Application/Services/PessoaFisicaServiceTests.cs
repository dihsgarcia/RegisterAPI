using Application.DTOs;
using Application.DTOs.Request;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Extensions;
using Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace RegisterAPI.Tests.Application.Services;

[TestFixture]
public class PessoaFisicaServiceTests
{
    private Mock<IClienteRepository> _clienteRepository;
    private Mock<ICepService> _cepService;
    private PessoaFisicaService _service;

    private const string ValidCpf = "52998224725";
    private const string ValidCep = "01001000";

    [SetUp]
    public void Setup()
    {
        _clienteRepository = new Mock<IClienteRepository>();
        _cepService = new Mock<ICepService>();

        _service = new PessoaFisicaService(
            _clienteRepository.Object,
            _cepService.Object);
    }

    [Test]
    public void Should_Throw_When_Cpf_Already_Exists()
    {
        _clienteRepository
            .Setup(r => r.GetPessoaFisicaByCpfAsync(It.IsAny<string>()))
            .ReturnsAsync(new PessoaFisica("Adnaldo Pereira", ValidCpf));

        var request = new CreatePessoaFisicaRequest
        {
            Nome = "Fabio Nunes",
            Cpf = ValidCpf,
            Enderecos = []
        };

        Assert.ThrowsAsync<BusinessException>(
            async () => await _service.CreateAsync(request));
    }

    [Test]
    public void Should_Throw_When_Cep_Is_Invalid()
    {
        _clienteRepository
            .Setup(r => r.GetPessoaFisicaByCpfAsync(It.IsAny<string>()))
            .ReturnsAsync((PessoaFisica?)null);

        _cepService
            .Setup(c => c.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync((EnderecoCepResult?)null);

        var request = new CreatePessoaFisicaRequest
        {
            Nome = "Fabio Nunes",
            Cpf = ValidCpf,
            Enderecos =
            [
                new CreateEnderecoRequest
                {
                    Cep = ValidCep,
                    NumeroEndereco = "100",
                    Complemento = "casa 2"
                }
            ]
        };

        Assert.ThrowsAsync<BusinessException>(
            async () => await _service.CreateAsync(request));
    }

    [Test]
    public async Task Should_Return_PessoaFisica_By_Cpf()
    {
        var cliente = new PessoaFisica("Marina Garcia", ValidCpf);

        _clienteRepository
            .Setup(r => r.GetPessoaFisicaByCpfAsync(It.IsAny<string>()))
            .ReturnsAsync(cliente);

        var result = await _service.GetByCpfAsync(ValidCpf);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Cpf, Is.EqualTo(ValidCpf));
    }

    [Test]
    public void Should_Throw_When_GetByCpf_Not_Found()
    {
        _clienteRepository
            .Setup(r => r.GetPessoaFisicaByCpfAsync(It.IsAny<string>()))
            .ReturnsAsync((PessoaFisica?)null);

        Assert.ThrowsAsync<NotFoundException>(
            async () => await _service.GetByCpfAsync(ValidCpf));
    }

    [Test]
    public async Task Should_Return_PessoaFisica_By_Id()
    {
        var cliente = new PessoaFisica("Bruna Oliveira", ValidCpf);

        _clienteRepository
            .Setup(r => r.GetPessoaFisicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(cliente);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Cpf, Is.EqualTo(ValidCpf));
    }

    [Test]
    public void Should_Throw_When_GetById_Not_Found()
    {
        _clienteRepository
            .Setup(r => r.GetPessoaFisicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((PessoaFisica?)null);

        Assert.ThrowsAsync<NotFoundException>(
            async () => await _service.GetByIdAsync(Guid.NewGuid()));
    }
    
    [Test]
    public void Should_Throw_When_Update_Client_Not_Found()
    {
        _clienteRepository
            .Setup(r => r.GetPessoaFisicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((PessoaFisica?)null);

        var request = new UpdatePessoaFisicaRequest
        {
            ClienteId = Guid.NewGuid(),
            Nome = "Bruna Oliveira",
            Cpf = ValidCpf,
            Enderecos = []
        };

        var exception = Assert.ThrowsAsync<NotFoundException>(
            async () => await _service.UpdateAsync(request));

        Assert.That(exception, Is.Not.Null);
    }
    
    [Test]
    public void Should_Throw_When_Cpf_Already_Exists_On_Update()
    {
        var cliente = new PessoaFisica("Jose Castro", ValidCpf);

        _clienteRepository
            .Setup(r => r.GetPessoaFisicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(cliente);

        _clienteRepository
            .Setup(r => r.GetPessoaFisicaByCpfAsync(It.IsAny<string>()))
            .ReturnsAsync(new PessoaFisica("Outro", "55706301808"));

        var request = new UpdatePessoaFisicaRequest
        {
            ClienteId = Guid.NewGuid(),
            Nome = "Jose Castro",
            Cpf = "55706301808",
            Enderecos = []
        };

        var exception = Assert.ThrowsAsync<BusinessException>(
            async () => await _service.UpdateAsync(request));

        Assert.That(exception, Is.Not.Null);
    }
    
    [Test]
    public void Should_Throw_When_Cep_Invalid_On_Update()
    {
        var cliente = new PessoaFisica("Marco Brito", ValidCpf);

        _clienteRepository
            .Setup(r => r.GetPessoaFisicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(cliente);

        _cepService
            .Setup(c => c.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync((EnderecoCepResult?)null);

        var request = new UpdatePessoaFisicaRequest
        {
            ClienteId = Guid.NewGuid(),
            Nome = "Carla Santos",
            Cpf = ValidCpf,
            Enderecos =
            [
                new UpdateEnderecoRequest
                {
                    Cep = ValidCep,
                    NumeroEndereco = "100",
                    Complemento = ""
                }
            ]
        };

        var exception = Assert.ThrowsAsync<BusinessException>(
            async () => await _service.UpdateAsync(request));

        Assert.That(exception, Is.Not.Null);
    }
    
    [Test]
    public async Task Should_Update_Existing_Address()
    {
        var cliente = new PessoaFisica("Laura Marinho", ValidCpf);

        var endereco = new Endereco(
            ValidCep,
            "Rua Teste",
            "10",
            "",
            "Centro",
            "São Paulo",
            "SP");

        cliente.AddEndereco(endereco);

        _clienteRepository
            .Setup(r => r.GetPessoaFisicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(cliente);

        _cepService
            .Setup(c => c.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync(new EnderecoCepResult
            {
                Cep = ValidCep,
                Logradouro = "Rua Atualizada",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP"
            });

        var request = new UpdatePessoaFisicaRequest
        {
            ClienteId = Guid.NewGuid(),
            Nome = "Diego Rodrigues",
            Cpf = ValidCpf,
            Enderecos =
            [
                new UpdateEnderecoRequest
                {
                    EnderecoId = endereco.EnderecoId,
                    Cep = ValidCep,
                    NumeroEndereco = "200",
                    Complemento = "Ap 10"
                }
            ]
        };

        await _service.UpdateAsync(request);

        _clienteRepository.Verify(
            r => r.UpdateAsync(It.IsAny<Cliente>()),
            Times.Once);
    }
    
    [Test]
    public async Task Should_Add_New_Address_On_Update()
    {
        var cliente = new PessoaFisica("Pedro Hugo", ValidCpf);

        _clienteRepository
            .Setup(r => r.GetPessoaFisicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(cliente);

        _cepService
            .Setup(c => c.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync(new EnderecoCepResult
            {
                Cep = ValidCep,
                Logradouro = "Rua Nova",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP"
            });

        var request = new UpdatePessoaFisicaRequest
        {
            ClienteId = Guid.NewGuid(),
            Nome = "Diego Garcia",
            Cpf = ValidCpf,
            Enderecos =
            [
                new UpdateEnderecoRequest
                {
                    Cep = ValidCep,
                    NumeroEndereco = "300",
                    Complemento = ""
                }
            ]
        };

        await _service.UpdateAsync(request);

        _clienteRepository.Verify(
            r => r.UpdateAsync(It.IsAny<Cliente>()),
            Times.Once);
    }
    
    [Test]
    public async Task Should_Delete_PessoaFisica()
    {
        var cliente = new PessoaFisica("Jonatas Meireles", ValidCpf);

        _clienteRepository
            .Setup(r => r.GetPessoaFisicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(cliente);

        await _service.DeleteAsync(Guid.NewGuid());

        _clienteRepository.Verify(
            r => r.DeleteAsync(It.IsAny<Cliente>()),
            Times.Once);
    }

    [Test]
    public void Should_Throw_When_Delete_Not_Found()
    {
        _clienteRepository
            .Setup(r => r.GetPessoaFisicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((PessoaFisica?)null);

        Assert.ThrowsAsync<NotFoundException>(
            async () => await _service.DeleteAsync(Guid.NewGuid()));
    }
}