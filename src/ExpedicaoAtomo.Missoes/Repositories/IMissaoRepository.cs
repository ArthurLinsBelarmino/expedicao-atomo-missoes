using ExpedicaoAtomo.Missoes.Models;

namespace ExpedicaoAtomo.Missoes.Repositories;

public interface IMissaoRepository
{
    IReadOnlyList<Missao> ListarTodas();

    Missao Adicionar(Missao missao);
}
