using Application.DTOs.Request;
using Application.DTOs.Responses;
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
    private Mock<IPessoaFisicaRepository> _repositoryMock = null!;
    private Mock<IViaCepService> _viaCepServiceMock = null!;
    private PessoaFisicaService _pessoaFisicaService = null!;
    
    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IPessoaFisicaRepository>();
        _viaCepServiceMock = new Mock<IViaCepService>();
        _pessoaFisicaService = new PessoaFisicaService(_repositoryMock.Object, _viaCepServiceMock.Object);
    }
    
    [Test]
    public async Task CreateAsync_ValidRequest_Should_Return_Id()
    {
        _repositoryMock.Setup(x => x.CpfExisteAsync(It.IsAny<string>())).ReturnsAsync(false);
        _viaCepServiceMock.Setup(x => x.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync(new ViaCepResponse
            {
                Cep = "01310100",
                Logradouro = "Rua do Brasil",
                Bairro = "Rua do Brasil",
                Localidade = "Rua do Brasil",
                Uf = "SP"
            });
        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<PessoaFisica>()))
            .Returns(Task.CompletedTask);

        var request = new CreatePessoaFisicaRequest
        {
            Cpf = "52998224725",
            Nome = "Joao Silva",
            NumeroEndereco = "1000",
        };
        
        var result = await _pessoaFisicaService.CreateAsync(request);
        
        Assert.That(result, Is.Not.EqualTo(Guid.Empty));
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<PessoaFisica>()), Times.Once);
    }
    
    [Test]
    public async Task CreateAsync_CpfAlreadyExists_Should_ThrowsBusinessExcepetion()
    {
        _repositoryMock.Setup(x => x.CpfExisteAsync(It.IsAny<string>())).ReturnsAsync(true);
        
        var request = new CreatePessoaFisicaRequest
        {
            Cpf = "52998224725",
            Nome = "Joao Silva",
            Cep =  "01310100",
            NumeroEndereco = "1000",
        };

        var ex = Assert.ThrowsAsync<BusinessException>(() => _pessoaFisicaService.CreateAsync(request));
        Assert.That(ex!.Message, Is.EqualTo("CPF já cadastrado."));
    }
    
    [Test]
    public async Task GetByCpfAsync_ExistsCpf_Should_Return_Pessoa()
    {
        var endereco = new Endereco(
            "01310100", 
            "Av Paulista", 
            "1000", 
            "bela Vista", 
            "Sao Paulo",
            "SP");
        
        var pessoFisica = new PessoaFisica("Joao Silva", "52998224725", endereco);
        
        _repositoryMock.Setup(x => x.GetByCpfAsync(It.IsAny<string>())).ReturnsAsync(pessoFisica);
        
        var result = await _pessoaFisicaService.GetByCpfAsync("52998224725");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Cpf, Is.EqualTo(pessoFisica.Cpf));
    }
    
    [Test]
    public async Task GetByCpfAsync_CpfNotExists_Should_ThrowsNotFoundExcepetion()
    {
        _repositoryMock.Setup(x => x.GetByCpfAsync(It.IsAny<string>()))
            .ReturnsAsync((PessoaFisica?)null);
        
        var ex = Assert.ThrowsAsync<NotFoundException>(() => _pessoaFisicaService.GetByCpfAsync("52998224725"));
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
        
        var pessoFisica = new PessoaFisica("Joao Silva", "52998224725", endereco);
        
        _repositoryMock.Setup(x => x.GetByCpfAsync(It.IsAny<string>())).ReturnsAsync(pessoFisica);
        _viaCepServiceMock.Setup(x => x.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync(new ViaCepResponse
            {
                Cep = "01310100",
                Logradouro = "Rua do Brasil",
                Bairro = "Rua do Brasil",
                Localidade = "Rua do Brasil",
                Uf = "SP"
            });
        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<PessoaFisica>()))
            .Returns(Task.CompletedTask);

        var request = new UpdatePessoaFisicaRequest()
        {
            Cpf = "52998224725",
            Nome = "Joao Silva",
            Cep = "01310100",
            NumeroEndereco = "1000",
        };
        
        await _pessoaFisicaService.UpdateAsync("52998224725" , request);
        
        _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<PessoaFisica>()), Times.Once);
    }
    
    [Test]
    public async Task UpdateAsync_CpfNotExists_Should_ThrowsNotFoundExcepetion()
    {
        _repositoryMock.Setup(x => x.GetByCpfAsync(It.IsAny<string>()))
            .ReturnsAsync((PessoaFisica?)null);
        
        var request = new UpdatePessoaFisicaRequest()
        {
            Cpf = "52998224725",
            Nome = "Joao Silva",
            Cep = "01310100",
            NumeroEndereco = "1000",
        };
        
        var ex = Assert.ThrowsAsync<NotFoundException>(() => _pessoaFisicaService.UpdateAsync("52998224725", request));
        Assert.That(ex!.Message, Is.EqualTo("Pessoa não encontrada."));
    }
    
    [Test]
    public async Task DeleteAsync_CpfExists_Should_Deletes()
    {
        var endereco = new Endereco(
            "01310100", 
            "Av Paulista", 
            "1000", 
            "bela Vista", 
            "Sao Paulo",
            "SP");
        
        var pessoFisica = new PessoaFisica("Joao Silva", "52998224725", endereco);
        
        _repositoryMock.Setup(x => x.GetByCpfAsync(It.IsAny<string>())).ReturnsAsync(pessoFisica);
        _repositoryMock.Setup(x => x.DeleteAsync(It.IsAny<PessoaFisica>()))
            .Returns(Task.CompletedTask);
        
        await _pessoaFisicaService.DeleteAsync("52998224725");
        
        _repositoryMock.Verify(x => x.DeleteAsync(It.IsAny<PessoaFisica>()), Times.Once);
    }
    
    [Test]
    public async Task DeleteAsync_CpfNotExists_Should_ThrowsNotFoundExcepetion()
    {
        _repositoryMock.Setup(x => x.GetByCpfAsync(It.IsAny<string>()))
            .ReturnsAsync((PessoaFisica?)null);
        
        var ex = Assert.ThrowsAsync<NotFoundException>(() => _pessoaFisicaService.DeleteAsync("52998224725"));
        Assert.That(ex!.Message, Is.EqualTo("Pessoa não encontrada."));
    }
}