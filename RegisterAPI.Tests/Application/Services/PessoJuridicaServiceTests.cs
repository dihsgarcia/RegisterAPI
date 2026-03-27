using Application.DTOs;
using Application.DTOs.Request;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Extensions;
using Domain.Repositories;
using Moq;

namespace RegisterAPI.Tests.Application.Services;

[TestFixture]
public class PessoJuridicaServiceTests
{
    private Mock<IClienteRepository> _clienteRepository;
    private Mock<ICepService> _cepService;
    private PessoaJuridicaService _service;

    private const string ValidCnpj =   "87640720000108";
    private const string InValidCnpj = "11122233344455";
    private const string ValidCnpjAlpha = "12A3C4560DE199";
    private const string ValidCep = "01001000";
    private const string RazaoSocialMock = "Silph Co.";

    [SetUp]
    public void Setup()
    {
        _clienteRepository = new Mock<IClienteRepository>();
        _cepService = new Mock<ICepService>();

        _service = new PessoaJuridicaService(
            _clienteRepository.Object,
            _cepService.Object);
        
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
    }
    
    [Test]
    [TestCase(ValidCnpj)]
    [TestCase(ValidCnpjAlpha)]
    public async Task Should_Create_PessoaJuridica_With_Valid_Cnpjs(string cnpj)
    {
        var request = new CreatePessoaJuridicaRequest
        {
            Nome = "Giovanni Rocket Leader",
            RazaoSocial = RazaoSocialMock,
            Cnpj = cnpj,
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

        _clienteRepository.Setup(r => r.GetPessoaJuridicaByCnpjAsync(cnpj))
            .ReturnsAsync((PessoaJuridica?)null);
        
        var result = await _service.CreateAsync(request);

        Assert.That(result, Is.Not.EqualTo(Guid.Empty));
        _clienteRepository.Verify(r => r.AddAsync(It.IsAny<PessoaJuridica>()), Times.Once);
        
        _clienteRepository.Invocations.Clear();
    }
    
    [Test]
    [TestCase(InValidCnpj)]
    public void Should_Throw_When_Cnpj_Is_invalid(string cnpj)
    {
        _clienteRepository
            .Setup(r => r.GetPessoaJuridicaByCnpjAsync(It.IsAny<string>()))
            .ReturnsAsync(new PessoaJuridica("Adnaldo Pereira", RazaoSocialMock, cnpj));

        var request = new CreatePessoaJuridicaRequest
        {
            Nome = "Fabio Nunes",
            RazaoSocial = RazaoSocialMock,
            Cnpj = cnpj,
            Enderecos = []
        };

        Assert.ThrowsAsync<DomainException>(
            async () => await _service.CreateAsync(request));
    }
    
    [Test]
    public void Should_Throw_When_Cnpj_Already_Exists()
    {
        _clienteRepository
            .Setup(r => r.GetPessoaJuridicaByCnpjAsync(It.IsAny<string>()))
            .ReturnsAsync(new PessoaJuridica("Adnaldo Pereira", RazaoSocialMock, ValidCnpj));

        var request = new CreatePessoaJuridicaRequest
        {
            Nome = "Fabio Nunes",
            RazaoSocial = RazaoSocialMock,
            Cnpj = ValidCnpj,
            Enderecos = []
        };

        Assert.ThrowsAsync<BusinessException>(
            async () => await _service.CreateAsync(request));
    }

    [Test]
    public void Should_Throw_When_Cep_Is_Invalid()
    {
        _clienteRepository
            .Setup(r => r.GetPessoaJuridicaByCnpjAsync(It.IsAny<string>()))
            .ReturnsAsync((PessoaJuridica?)null);

        _cepService
            .Setup(c => c.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync((EnderecoCepResult?)null);
        
        var request = new CreatePessoaJuridicaRequest
        {
            Nome = "Fabio Nunes",
            RazaoSocial = RazaoSocialMock,
            Cnpj = ValidCnpj,
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
    public async Task Should_Return_PessoaJuridica_By_Cnpj()
    {
        var cliente = new PessoaJuridica("Marina Garcia", RazaoSocialMock, ValidCnpj);

        _clienteRepository
            .Setup(r => r.GetPessoaJuridicaByCnpjAsync(It.IsAny<string>()))
            .ReturnsAsync(cliente);

        var result = await _service.GetByCnpjAsync(ValidCnpj);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Cnpj, Is.EqualTo(ValidCnpj));
    }
    
    [Test]
    public void Should_Throw_When_GetByCnpj_Not_Found()
    {
        _clienteRepository
            .Setup(r => r.GetPessoaJuridicaByCnpjAsync(It.IsAny<string>()))
            .ReturnsAsync((PessoaJuridica?)null);

        Assert.ThrowsAsync<NotFoundException>(
            async () => await _service.GetByCnpjAsync(ValidCnpj));
    }

    [Test]
    public async Task Should_Return_PessoaJuridica_By_Id()
    {
        var cliente = new PessoaJuridica("Bruna Oliveira", RazaoSocialMock, ValidCnpj);

        _clienteRepository
            .Setup(r => r.GetPessoaJuridicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(cliente);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Cnpj, Is.EqualTo(ValidCnpj));
    }

    [Test]
    public void Should_Throw_When_GetById_Not_Found()
    {
        _clienteRepository
            .Setup(r => r.GetPessoaJuridicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((PessoaJuridica?)null);

        Assert.ThrowsAsync<NotFoundException>(
            async () => await _service.GetByIdAsync(Guid.NewGuid()));
    }
    
    [Test]
    public void Should_Throw_When_Update_Client_Not_Found()
    {
        _clienteRepository
            .Setup(r => r.GetPessoaJuridicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((PessoaJuridica?)null);

        var request = new UpdatePessoaJuridicaRequest
        {
            ClienteId = Guid.NewGuid(),
            Nome = "Bruna Oliveira",
            RazaoSocial =  RazaoSocialMock,
            Cnpj = ValidCnpj,
            Enderecos = []
        };

        var exception = Assert.ThrowsAsync<NotFoundException>(
            async () => await _service.UpdateAsync(request));

        Assert.That(exception, Is.Not.Null);
    }
    
    [Test]
    public void Should_Throw_When_Cnpj_Already_Exists_On_Update()
    {
        var cliente = new PessoaJuridica("Jose Castro", RazaoSocialMock, ValidCnpj);

        _clienteRepository
            .Setup(r => r.GetPessoaJuridicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(cliente);

        _clienteRepository
            .Setup(r => r.GetPessoaJuridicaByCnpjAsync(It.IsAny<string>()))
            .ReturnsAsync(new PessoaJuridica("Outro", RazaoSocialMock, ValidCnpjAlpha));

        var request = new UpdatePessoaJuridicaRequest
        {
            ClienteId = Guid.NewGuid(),
            Nome = "Jose Castro",
            RazaoSocial =  RazaoSocialMock,
            Cnpj = ValidCnpjAlpha,
            Enderecos = []
        };

        var exception = Assert.ThrowsAsync<BusinessException>(
            async () => await _service.UpdateAsync(request));

        Assert.That(exception, Is.Not.Null);
    }
    
    [Test]
    public void Should_Throw_When_Cep_Invalid_On_Update()
    {
        var cliente = new PessoaJuridica("Marco Brito", RazaoSocialMock, ValidCnpj);

        _clienteRepository
            .Setup(r => r.GetPessoaJuridicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(cliente);

        _cepService
            .Setup(c => c.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync((EnderecoCepResult?)null);

        var request = new UpdatePessoaJuridicaRequest
        {
            ClienteId = Guid.NewGuid(),
            Nome = "Carla Santos",
            RazaoSocial =  RazaoSocialMock,
            Cnpj = ValidCnpj,
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
        var cliente = new PessoaJuridica("Laura Marinho", RazaoSocialMock, ValidCnpj);

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
            .Setup(r => r.GetPessoaJuridicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(cliente);
        
        var request = new UpdatePessoaJuridicaRequest
        {
            ClienteId = Guid.NewGuid(),
            Nome = "Diego Rodrigues",
            RazaoSocial =  RazaoSocialMock,
            Cnpj = ValidCnpj,
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
        var cliente = new PessoaJuridica("Pedro Hugo", RazaoSocialMock, ValidCnpj);

        _clienteRepository
            .Setup(r => r.GetPessoaJuridicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(cliente);
        
        var request = new UpdatePessoaJuridicaRequest
        {
            ClienteId = Guid.NewGuid(),
            Nome = "Diego Garcia",
            RazaoSocial =  RazaoSocialMock,
            Cnpj = ValidCnpj,
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
    public async Task Should_Delete_PessoaJuridica()
    {
        var cliente = new PessoaJuridica("Jonatas Meireles", RazaoSocialMock, ValidCnpj);

        _clienteRepository
            .Setup(r => r.GetPessoaJuridicaByIdAsync(It.IsAny<Guid>()))
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
            .Setup(r => r.GetPessoaJuridicaByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((PessoaJuridica?)null);

        Assert.ThrowsAsync<NotFoundException>(
            async () => await _service.DeleteAsync(Guid.NewGuid()));
    }
}

