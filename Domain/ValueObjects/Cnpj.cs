using Domain.Common;
using Domain.Extensions;

namespace Domain.ValueObjects;

public sealed class Cnpj
{
    public string Number { get; }

    public Cnpj(string number)
    {
        number = CnpjUtils.Limpar(number);

        if (!CnpjUtils.EhValido(number))
            throw new DomainException("CNPJ inválido.");

        Number = number;
    }

    public override string ToString() => Number;
}