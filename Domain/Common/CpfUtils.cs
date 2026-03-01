namespace Domain.Common;

public static class CpfUtils
{
    public static bool EhValido(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = Limpar(cpf);

        if (cpf.Length != 11)
            return false;

        if (cpf.All(c => c == cpf[0]))
            return false;

        int[] multiplicador1 = { 10,9,8,7,6,5,4,3,2 };
        int[] multiplicador2 = { 11,10,9,8,7,6,5,4,3,2 };

        string tempCpf = cpf[..9];
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += (tempCpf[i] - '0') * multiplicador1[i];

        int resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        tempCpf += resto;

        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += (tempCpf[i] - '0') * multiplicador2[i];

        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith(resto.ToString());
    }

    public static string Limpar(string cpf)
        => cpf.Replace(".", "").Replace("-", "").Trim();
}