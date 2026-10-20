using FornecedoresApp.Domain.Exceptions;

namespace FornecedoresApp.Domain.Entities;

public static class CnpjValidador
{
    private static readonly int[] PesosPrimeiroDigito = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
    private static readonly int[] PesosSegundoDigito = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

    public static string Normalizar(string? cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
        {
            throw new RegraDeNegocioException("O CNPJ é obrigatório.");
        }

        var cnpjInformado = cnpj.Trim();
        var cnpjSemFormatacao = new string(cnpjInformado.Where(char.IsAsciiDigit).ToArray());

        if (!FormatoValido(cnpjInformado))
        {
            throw new RegraDeNegocioException("O CNPJ deve conter somente números ou a formatação padrão.");
        }

        if (cnpjSemFormatacao.Length != 14)
        {
            throw new RegraDeNegocioException("O CNPJ deve possuir exatamente 14 dígitos.");
        }

        return cnpjSemFormatacao;
    }

    private static bool FormatoValido(string cnpj)
    {
        if (cnpj.Length == 14)
        {
            return cnpj.All(char.IsAsciiDigit);
        }

        if (cnpj.Length != 18)
        {
            return false;
        }

        for (var indice = 0; indice < cnpj.Length; indice++)
        {
            var caractereEsperado = indice switch
            {
                2 or 6 => '.',
                10 => '/',
                15 => '-',
                _ => '\0'
            };

            if (caractereEsperado == '\0' ? !char.IsAsciiDigit(cnpj[indice]) : cnpj[indice] != caractereEsperado)
            {
                return false;
            }
        }

        return true;
    }

    public static string ValidarENormalizar(string? cnpj)
    {
        var cnpjNormalizado = Normalizar(cnpj);

        if (cnpjNormalizado.All(digito => digito == cnpjNormalizado[0]))
        {
            throw new RegraDeNegocioException("O CNPJ informado é inválido.");
        }

        var primeiroDigito = CalcularDigito(cnpjNormalizado[..12], PesosPrimeiroDigito);
        var segundoDigito = CalcularDigito(cnpjNormalizado[..13], PesosSegundoDigito);

        if (cnpjNormalizado[12] - '0' != primeiroDigito || cnpjNormalizado[13] - '0' != segundoDigito)
        {
            throw new RegraDeNegocioException("O CNPJ informado é inválido.");
        }

        return cnpjNormalizado;
    }

    private static int CalcularDigito(string baseCnpj, IReadOnlyList<int> pesos)
    {
        var soma = 0;

        for (var indice = 0; indice < pesos.Count; indice++)
        {
            soma += (baseCnpj[indice] - '0') * pesos[indice];
        }

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}
