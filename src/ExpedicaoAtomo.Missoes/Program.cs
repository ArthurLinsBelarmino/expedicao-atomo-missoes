using System.Text;
using ExpedicaoAtomo.Missoes.Data;
using ExpedicaoAtomo.Missoes.Repositories;
using ExpedicaoAtomo.Missoes.Services;
using ExpedicaoAtomo.Missoes.Ui;

Console.OutputEncoding = Encoding.UTF8;

var caminhoDados = Path.Combine(AppContext.BaseDirectory, "data", "missoes.json");

try
{
    var repositorio = new JsonMissaoRepository(caminhoDados, DadosIniciais.Missoes);
    var service = new MissaoService(repositorio);
    new ConsoleUi(service).Executar(repositorio.Aviso);
}
catch (Exception ex)
{
    Console.WriteLine($"Não foi possível iniciar o motor de missões: {ex.Message}");
}
