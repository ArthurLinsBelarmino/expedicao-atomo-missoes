namespace ExpedicaoAtomo.Missoes.Services;

public static class FonteConfiavel
{
    public static readonly IReadOnlyList<string> Dominios = new[]
    {
        "gov.br",         
        "marinha.mil.br", 
        "ipen.br",
        "usp.br",
        "embrapa.br",
        "iaea.org", 
        "unscear.org" 
    };

    public static bool EhConfiavel(Uri uri)
    {
        var host = uri.IdnHost.ToLowerInvariant().TrimEnd('.');

        return Dominios.Any(d => host == d || host.EndsWith("." + d, StringComparison.Ordinal));
    }
}
