using ExpedicaoAtomo.Missoes.Exceptions;
using ExpedicaoAtomo.Missoes.Models;
using ExpedicaoAtomo.Missoes.Repositories;
using ExpedicaoAtomo.Missoes.Utils;
using ExpedicaoAtomo.Missoes.Validation;

namespace ExpedicaoAtomo.Missoes.Services;

public class MissaoService
{
    public const int TamanhoRodada = 5;
    private readonly IMissaoRepository _repositorio;

    public MissaoService(IMissaoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public Missao Cadastrar(Missao nova)
    {
        nova.Afirmacao = nova.Afirmacao?.Trim() ?? string.Empty;
        nova.Explicacao = nova.Explicacao?.Trim() ?? string.Empty;
        nova.FonteUrl = nova.FonteUrl?.Trim() ?? string.Empty;

        MissaoValidator.Validar(nova);

        var chave = TextoUtils.Normalizar(nova.Afirmacao);
        var repetida = _repositorio.ListarTodas()
            .FirstOrDefault(m => TextoUtils.Normalizar(m.Afirmacao) == chave);
        if (repetida is not null)
            throw new RegraNegocioException(
                $"Já existe uma missão com essa afirmação (missão #{repetida.Id}). " +
                "A comparação ignora maiúsculas, acentos e pontuação final.");

        var uri = new Uri(nova.FonteUrl);
        if (!FonteConfiavel.EhConfiavel(uri))
            throw new RegraNegocioException(
                $"Fonte recusada: \"{uri.Host}\" não é um domínio oficial. " +
                $"Domínios aceitos: {string.Join(", ", FonteConfiavel.Dominios)}.");

        return _repositorio.Adicionar(nova);
    }

    public IReadOnlyList<Missao> ListarTodas() => _repositorio.ListarTodas();

    public IReadOnlyList<Missao> BuscarPorTexto(string termo)
    {
        var chave = TextoUtils.Normalizar(termo);
        if (chave.Length < 2)
            throw new ValidacaoException("Digite pelo menos 2 caracteres para buscar.");

        return _repositorio.ListarTodas()
            .Where(m => TextoUtils.Normalizar(m.Afirmacao).Contains(chave)
                     || TextoUtils.Normalizar(m.Explicacao).Contains(chave))
            .ToList();
    }

    public IReadOnlyList<Missao> FiltrarPorDificuldade(int dificuldade)
    {
        if (dificuldade < MissaoValidator.DificuldadeMin || dificuldade > MissaoValidator.DificuldadeMax)
            throw new ValidacaoException(
                $"Dificuldade deve ser de {MissaoValidator.DificuldadeMin} a {MissaoValidator.DificuldadeMax}.");

        return _repositorio.ListarTodas().Where(m => m.Dificuldade == dificuldade).ToList();
    }

    public IReadOnlyList<Missao> FiltrarPorPonto(int idPonto)
    {
        if (idPonto <= 0)
            throw new ValidacaoException("IdPonto deve ser maior que zero.");

        return _repositorio.ListarTodas().Where(m => m.IdPonto == idPonto).ToList();
    }

    public IReadOnlyList<Missao> SortearRodada(int quantidade = TamanhoRodada)
    {
        var todas = _repositorio.ListarTodas();
        if (todas.Count == 0)
            throw new RegraNegocioException("Não há missões cadastradas para montar uma rodada.");

        return todas.OrderBy(_ => Random.Shared.Next())
                    .Take(Math.Min(quantidade, todas.Count))
                    .ToList();
    }

    public ResultadoResposta Responder(Missao missao, TipoResposta respostaDoJogador)
    {
        var acertou = respostaDoJogador == missao.Resposta;
        return new ResultadoResposta(acertou, acertou ? missao.Xp : 0);
    }
}
