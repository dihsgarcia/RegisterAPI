using Domain.Common;
using Domain.Extensions;

namespace Domain.ValueObjects;

public sealed class Cpf
{
    public string Numero { get; }

    public Cpf(string numero)
    {
        numero = CpfUtils.Limpar(numero);

        if (!CpfUtils.EhValido(numero))
            throw new DomainException("CPF inválido.");

        Numero = numero;
    }

    public override string ToString() => Numero;
}