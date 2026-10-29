namespace ExpedicaoAtomo.Missoes.Exceptions;

public class ValidacaoException : Exception
{
    public IReadOnlyList<string> Erros { get; }

    public ValidacaoException(IEnumerable<string> erros)
        : base("Dados inválidos.")
    {
        Erros = erros.ToList();
    }

    public ValidacaoException(string erro) : this(new[] { erro }) { }
}
