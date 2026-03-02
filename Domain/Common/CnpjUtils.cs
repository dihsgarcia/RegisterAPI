namespace Domain.Common;

public static class CnpjUtils
{
    public static bool EhValido(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        cnpj = Limpar(cnpj);

        // Deve ter exatamente 14 caracteres alfanuméricos
        if (cnpj.Length != 14 || cnpj.Any(c => !char.IsLetterOrDigit(c)))
            return false;

        // Bloqueia sequências totalmente repetidas (ex.: "AAAAAAAAAAAAAA" ou "11111111111111")
        if (SequenciaRepetida(cnpj))
            return false;

        bool apenasNumeros = cnpj.All(char.IsDigit);

        // Se for totalmente numérico, valida dígitos verificadores
        if (apenasNumeros)
        {
            int digito1 = CalcularDigito(cnpj, 12);
            int digito2 = CalcularDigito(cnpj, 13, digito1);

            return cnpj.EndsWith($"{digito1}{digito2}");
        }

        // Se contém letras, ainda não há regra oficial de dígito, mas formato está ok
        return true;
    }

    private static bool SequenciaRepetida(string cnpj)
    {
        char primeiro = cnpj[0];
        return cnpj.All(c => c == primeiro);
    }

    private static int CalcularDigito(string cnpj, int tamanho, int digitoAnterior = -1)
    {
        int[] multiplicadores = tamanho == 12
            ? new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 }
            : new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        int soma = 0;
        for (int i = 0; i < tamanho; i++)
            soma += (cnpj[i] - '0') * multiplicadores[i];

        if (tamanho == 13 && digitoAnterior >= 0)
            soma += digitoAnterior * multiplicadores[12];

        int resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }

    public static string Limpar(string cnpj)
        => cnpj.Replace(".", "")
            .Replace("-", "")
            .Replace("/", "")
            .Trim()
            .ToUpper();
}