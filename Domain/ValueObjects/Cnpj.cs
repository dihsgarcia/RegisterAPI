using Domain.Common;
using Domain.Extensions;

namespace Domain.ValueObjects;

public sealed class Cnpj
{
    public string Numero { get; }

    public Cnpj(string numero)
    {
        numero = CnpjUtils.Limpar(numero);

        if (!CnpjUtils.EhValido(numero))
            throw new DomainException("CNPJ inválido.");

        Numero = numero;
    }

    public override string ToString() => Numero;
}