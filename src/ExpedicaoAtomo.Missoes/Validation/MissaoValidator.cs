using ExpedicaoAtomo.Missoes.Exceptions;
using ExpedicaoAtomo.Missoes.Models;

namespace ExpedicaoAtomo.Missoes.Validation;

public static class MissaoValidator
{
    public const int AfirmacaoMin = 15;
    public const int AfirmacaoMax = 250;
    public const int ExplicacaoMin = 40;
    public const int DificuldadeMin = 1;
    public const int DificuldadeMax = 3;

    public static void Validar(Missao missao)
    {
        var erros = new List<string>();

        if (missao.IdPonto <= 0)
            erros.Add("IdPonto deve ser maior que zero.");

        var afirmacao = missao.Afirmacao?.Trim() ?? string.Empty;
        if (afirmacao.Length < AfirmacaoMin || afirmacao.Length > AfirmacaoMax)
            erros.Add($"Afirmação deve ter entre {AfirmacaoMin} e {AfirmacaoMax} caracteres (digitado: {afirmacao.Length}).");

        if (!Enum.IsDefined(missao.Resposta))
            erros.Add("Resposta deve ser Verdade ou Mito.");

        var explicacao = missao.Explicacao?.Trim() ?? string.Empty;
        if (explicacao.Length < ExplicacaoMin)
            erros.Add($"Explicação deve ter pelo menos {ExplicacaoMin} caracteres (digitado: {explicacao.Length}).");

        if (missao.Dificuldade < DificuldadeMin || missao.Dificuldade > DificuldadeMax)
            erros.Add($"Dificuldade deve ser de {DificuldadeMin} a {DificuldadeMax}.");

        if (!TryCriarUrlHttps(missao.FonteUrl, out _))
            erros.Add("FonteUrl deve ser uma URL https válida (ex.: https://www.gov.br/cnen/pt-br).");

        if (erros.Count > 0)
            throw new ValidacaoException(erros);
    }

    public static bool TryCriarUrlHttps(string? url, out Uri? uri)
    {
        uri = null;
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.TryCreate(url.Trim(), UriKind.Absolute, out uri)
               && uri.Scheme == Uri.UriSchemeHttps
               && uri.Host.Contains('.');
    }
}
