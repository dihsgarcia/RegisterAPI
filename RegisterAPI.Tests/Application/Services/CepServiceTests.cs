using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Moq;

namespace RegisterAPI.Tests.Services;

[TestFixture]
public class CepServiceTests
{
    private Mock<ICepProvider> _provider1;
    private Mock<ICepProvider> _provider2;
    private CepService _service;

    [SetUp]
    public void Setup()
    {
        _provider1 = new Mock<ICepProvider>();
        _provider2 = new Mock<ICepProvider>();

        _service = new CepService(new[]
        {
            _provider1.Object,
            _provider2.Object
        });
    }

    [Test]
    public async Task Should_Return_Address_When_First_Provider_Returns()
    {
        var endereco = new EnderecoCepResult
        {
            Cep = "01001902",
            Logradouro = "Praça da Sé",
            Bairro = "Sé",
            Cidade = "São Paulo",
            Estado = "SP"
        };

        _provider1
            .Setup(p => p.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync(endereco);

        var result = await _service.GetEnderecoAsync("01001902");
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Cep, Is.EqualTo("01001902"));
    }

    [Test]
    public async Task Should_Use_Second_Provider_When_First_Returns_Null()
    {
        _provider1
            .Setup(p => p.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync((EnderecoCepResult?)null);

        _provider2
            .Setup(p => p.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync(new EnderecoCepResult
            {
                Cep = "01001902",
                Logradouro = "Praça da Sé",
                Bairro = "Sé",
                Cidade = "São Paulo",
                Estado = "SP"
            });

        var result = await _service.GetEnderecoAsync("01001902");
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Cep, Is.EqualTo("01001902"));
    }

    [Test]
    public async Task Should_Return_Null_When_All_Providers_Fail()
    {
        _provider1
            .Setup(p => p.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync((EnderecoCepResult?)null);

        _provider2
            .Setup(p => p.GetEnderecoAsync(It.IsAny<string>()))
            .ReturnsAsync((EnderecoCepResult?)null);

        var result = await _service.GetEnderecoAsync("01001902");

        Assert.That(result, Is.Null);
    }
}