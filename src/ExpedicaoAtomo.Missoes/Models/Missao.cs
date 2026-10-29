using System.Text.Json.Serialization;

namespace ExpedicaoAtomo.Missoes.Models;

public class Missao
{
    public int Id { get; set; }
    public int IdPonto { get; set; }
    public string Afirmacao { get; set; } = string.Empty;
    public TipoResposta Resposta { get; set; }
    public string Explicacao { get; set; } = string.Empty;
    public int Dificuldade { get; set; }
    public string FonteUrl { get; set; } = string.Empty;

    [JsonIgnore]
    public int Xp => Dificuldade * 10;
}
