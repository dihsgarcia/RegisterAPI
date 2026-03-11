using Domain.Common;
using Domain.Extensions;

namespace Domain.ValueObjects;

public sealed class Cpf
{
    public string Number { get; }

    public Cpf(string number)
    {
        number = CpfUtils.Limpar(number);

        if (!CpfUtils.EhValido(number))
            throw new DomainException("CPF inválido.");

        Number = number;
    }

    public override string ToString() => Number;
}