using System.Globalization;
using System.Text;

namespace ExpedicaoAtomo.Missoes.Utils;

public static class TextoUtils
{
    public static string Normalizar(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return string.Empty;

        var decomposto = texto.Trim().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(decomposto.Length);
        var ultimoFoiEspaco = false;

        foreach (var c in decomposto)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
                continue;

            if (char.IsWhiteSpace(c))
            {
                if (!ultimoFoiEspaco) sb.Append(' ');
                ultimoFoiEspaco = true;
                continue;
            }

            ultimoFoiEspaco = false;
            sb.Append(char.ToLowerInvariant(c));
        }

        return sb.ToString().TrimEnd('.', '!', '?', ';', ' ').Normalize(NormalizationForm.FormC);
    }
}
