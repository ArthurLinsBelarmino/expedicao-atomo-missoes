using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExpedicaoAtomo.Missoes.Models;

namespace ExpedicaoAtomo.Missoes.Repositories;

public class JsonMissaoRepository : IMissaoRepository
{
    private static readonly JsonSerializerOptions Opcoes = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, 
        Converters = { new JsonStringEnumConverter() } 
    };

    private readonly List<Missao> _missoes;

    public string Caminho { get; }
    public string? Aviso { get; private set; }

    public JsonMissaoRepository(string caminho, Func<IEnumerable<Missao>> dadosIniciais)
    {
        Caminho = caminho;
        _missoes = Carregar(dadosIniciais);
    }

    public IReadOnlyList<Missao> ListarTodas() => _missoes.OrderBy(m => m.Id).ToList();

    public Missao Adicionar(Missao missao)
    {
        missao.Id = _missoes.Count == 0 ? 1 : _missoes.Max(m => m.Id) + 1;

        var novaLista = new List<Missao>(_missoes) { missao };
        Gravar(novaLista);
        _missoes.Add(missao);
        return missao;
    }

    private List<Missao> Carregar(Func<IEnumerable<Missao>> dadosIniciais)
    {
        var pasta = Path.GetDirectoryName(Caminho);
        if (!string.IsNullOrEmpty(pasta))
            Directory.CreateDirectory(pasta);

        if (!File.Exists(Caminho))
        {
            var iniciais = dadosIniciais().ToList();
            Gravar(iniciais);
            Aviso = $"Arquivo de dados criado com {iniciais.Count} missões iniciais.";
            return iniciais;
        }

        try
        {
            var json = File.ReadAllText(Caminho);
            return JsonSerializer.Deserialize<List<Missao>>(json, Opcoes) ?? new List<Missao>();
        }
        catch (JsonException)
        {
            var backup = $"{Caminho}.corrompido-{DateTime.Now:yyyyMMddHHmmss}.bak";
            File.Move(Caminho, backup);
            var iniciais = dadosIniciais().ToList();
            Gravar(iniciais);
            Aviso = $"O arquivo de dados estava corrompido. Cópia salva em {Path.GetFileName(backup)} e dados iniciais recriados.";
            return iniciais;
        }
    }

    private void Gravar(List<Missao> missoes)
    {
        var temporario = Caminho + ".tmp";
        File.WriteAllText(temporario, JsonSerializer.Serialize(missoes, Opcoes));
        File.Move(temporario, Caminho, overwrite: true);
    }
}
