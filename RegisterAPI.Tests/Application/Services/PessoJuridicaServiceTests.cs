using Application.DTOs.Request;
using Application.DTOs.Responses;
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
    private Mock<IPessoaJuridicaRepository> _repositoryMock = null!;
    private Mock<IViaCepService> _viaCepServiceMock = null!;
    private PessoaJuridicaService _pessoaJuridicaService = null!;
    
    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IPessoaJuridicaRepository>();
        _viaCepServiceMock = new Mock<IViaCepService>();
        _pessoaJuridicaService = new PessoaJuridicaService(_repositoryMock.Object, _viaCepServiceMock.Object);
    }
    
    [Test]
    public async Task CreateAsync_ValidRequest_Should_Return_Id()
    {
        _repositoryMock.Setup(x => x.CnpjExisteAsync(It.IsAny<string>())).ReturnsAsync(false);
        _viaCepServiceMock.Setup(x => x.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync(new ViaCepResponse
            {
                Cep = "01310100",
                Logradouro = "Rua do Brasil",
                Bairro = "Rua do Brasil",
                Localidade = "Rua do Brasil",
                Uf = "SP"
            });
        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<PessoaJuridica>()))
            .Returns(Task.CompletedTask);

        var request = new CreatePessoaJuridicaRequest
        {
            Cnpj = "A1.B2C.3D4/1A2B-99",
            RazaoSocial = "Empresa fantasma",
            NumeroEndereco = "1000",
            Cep =  "01310100",
        };
        
        var result = await _pessoaJuridicaService.CreateAsync(request);
        
        Assert.That(result, Is.Not.EqualTo(Guid.Empty));
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<PessoaJuridica>()), Times.Once);
    }
    
    [Test]
    public async Task CreateAsync_CnpjAlreadyExists_Should_ThrowsBusinessExcepetion()
    {
        _repositoryMock.Setup(x => x.CnpjExisteAsync(It.IsAny<string>())).ReturnsAsync(true);
        
        var request = new CreatePessoaJuridicaRequest
        {
            Cnpj = "A1.B2C.3D4/1A2B-99",
            RazaoSocial = "Empresa fantasma",
            NumeroEndereco = "1000",
            Cep =  "01310100",
        };

        var ex = Assert.ThrowsAsync<BusinessException>(() => _pessoaJuridicaService.CreateAsync(request));
        Assert.That(ex!.Message, Is.EqualTo("CNPJ já cadastrado."));
    }
    
    [Test]
    public async Task GetByCnpjAsync_ExistsCnpj_Should_Return_Pessoa()
    {
        var endereco = new Endereco(
            "01310100", 
            "Av Paulista", 
            "1000", 
            "bela Vista", 
            "Sao Paulo",
            "SP");
        
        var pessoaJuridica = new PessoaJuridica("Empresa fantasma", "A1.B2C.3D4/1A2B-99", endereco);
        
        _repositoryMock.Setup(x => x.GetByCnpjAsync(It.IsAny<string>())).ReturnsAsync(pessoaJuridica);
        
        var result = await _pessoaJuridicaService.GetByCnpjAsync("A1.B2C.3D4/1A2B-99");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Cnpj, Is.EqualTo(pessoaJuridica.Cnpj));
    }
    
    [Test]
    public async Task GetByCnpjAsync_CnpjNotExists_Should_ThrowsNotFoundExcepetion()
    {
        _repositoryMock.Setup(x => x.GetByCnpjAsync(It.IsAny<string>()))
            .ReturnsAsync((PessoaJuridica?)null);
        
        var ex = Assert.ThrowsAsync<NotFoundException>(() => _pessoaJuridicaService.GetByCnpjAsync("A1.B2C.3D4/1A2B-99"));
        Assert.That(ex!.Message, Is.EqualTo("Pessoa não encontrada."));
    }
    
    [Test]
    public async Task UpdateAsync_ValidRequest_Should_Ok()
    {
        var endereco = new Endereco(
            "01310100", 
            "Av Paulista", 
            "1000", 
            "bela Vista", 
            "Sao Paulo",
            "SP");
        
        var pessoaJuridica = new PessoaJuridica("Empresa fantasma", "A1.B2C.3D4/1A2B-99", endereco);
        
        _repositoryMock.Setup(x => x.GetByCnpjAsync(It.IsAny<string>())).ReturnsAsync(pessoaJuridica);
        _viaCepServiceMock.Setup(x => x.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync(new ViaCepResponse
            {
                Cep = "01310100",
                Logradouro = "Rua do Brasil",
                Bairro = "Rua do Brasil",
                Localidade = "Rua do Brasil",
                Uf = "SP"
            });
        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<PessoaJuridica>()))
            .Returns(Task.CompletedTask);

        var request = new UpdatePessoaJuridicaRequest()
        {
            Cnpj = "A1.B2C.3D4/1A2B-99",
            RazaoSocial = "Empresa fantasma",
            NumeroEndereco = "1000",
            Cep =  "01310100",
        };
        
        await _pessoaJuridicaService.UpdateAsync("A1.B2C.3D4/1A2B-99" , request);
        
        _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<PessoaJuridica>()), Times.Once);
    }
    
    [Test]
    public async Task UpdateAsync_CnpjNotExists_Should_ThrowsNotFoundExcepetion()
    {
        _repositoryMock.Setup(x => x.GetByCnpjAsync(It.IsAny<string>()))
            .ReturnsAsync((PessoaJuridica?)null);
        
        var request = new UpdatePessoaJuridicaRequest()
        {
            Cnpj = "A1.B2C.3D4/1A2B-99",
            RazaoSocial = "Empresa fantasma",
            NumeroEndereco = "1000",
            Cep =  "01310100",
        };
        
        var ex = Assert.ThrowsAsync<NotFoundException>(() => _pessoaJuridicaService.UpdateAsync("A1.B2C.3D4/1A2B-99", request));
        Assert.That(ex!.Message, Is.EqualTo("Pessoa não encontrada."));
    }
    
    [Test]
    public async Task DeleteAsync_CnpjExists_Should_Deletes()
    {
        var endereco = new Endereco(
            "01310100", 
            "Av Paulista", 
            "1000", 
            "bela Vista", 
            "Sao Paulo",
            "SP");
        
        var pessoaJuridica = new PessoaJuridica("Empresa fantasma", "A1.B2C.3D4/1A2B-99", endereco);
        
        _repositoryMock.Setup(x => x.GetByCnpjAsync(It.IsAny<string>())).ReturnsAsync(pessoaJuridica);
        _repositoryMock.Setup(x => x.DeleteAsync(It.IsAny<PessoaJuridica>()))
            .Returns(Task.CompletedTask);
        
        await _pessoaJuridicaService.DeleteAsync("A1.B2C.3D4/1A2B-99");
        
        _repositoryMock.Verify(x => x.DeleteAsync(It.IsAny<PessoaJuridica>()), Times.Once);
    }
    
    [Test]
    public async Task DeleteAsync_CnpjNotExists_Should_ThrowsNotFoundExcepetion()
    {
        _repositoryMock.Setup(x => x.GetByCnpjAsync(It.IsAny<string>()))
            .ReturnsAsync((PessoaJuridica?)null);
        
        var ex = Assert.ThrowsAsync<NotFoundException>(() => _pessoaJuridicaService.DeleteAsync("A1.B2C.3D4/1A2B-99"));
        Assert.That(ex!.Message, Is.EqualTo("Pessoa não encontrada."));
    }
}