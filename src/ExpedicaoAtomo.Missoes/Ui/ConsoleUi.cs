using ExpedicaoAtomo.Missoes.Exceptions;
using ExpedicaoAtomo.Missoes.Models;
using ExpedicaoAtomo.Missoes.Services;

namespace ExpedicaoAtomo.Missoes.Ui;

public class ConsoleUi
{
    private readonly MissaoService _service;

    public ConsoleUi(MissaoService service)
    {
        _service = service;
    }

    public void Executar(string? avisoInicial)
    {
        if (!string.IsNullOrEmpty(avisoInicial))
            Info(avisoInicial);

        while (true)
        {
            MostrarMenu();
            try
            {
                var opcao = LerLinha("Escolha uma opção: ").Trim();
                switch (opcao)
                {
                    case "1": Cadastrar(); break;
                    case "2": ListarTodas(); break;
                    case "3": BuscarPorTexto(); break;
                    case "4": FiltrarPorDificuldade(); break;
                    case "5": FiltrarPorPonto(); break;
                    case "6": RodadaDeTreino(); break;
                    case "0":
                        Info("Até a próxima expedição!");
                        return;
                    default:
                        Erro("Opção inválida. Digite um número de 0 a 6.");
                        break;
                }
            }
            catch (EndOfStreamException)
            {
                return;
            }
            catch (ValidacaoException ex)
            {
                Erro("Não foi possível concluir. Corrija os itens abaixo:");
                foreach (var erro in ex.Erros)
                    Erro($"  - {erro}");
            }
            catch (RegraNegocioException ex)
            {
                Erro(ex.Message);
            }
            catch (IOException ex)
            {
                Erro($"Falha ao ler ou gravar o arquivo de dados: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Erro($"Sem permissão para acessar o arquivo de dados: {ex.Message}");
            }
            catch (Exception ex)
            {
                Erro($"Erro inesperado: {ex.Message}");
            }
        }
    }

    private static void MostrarMenu()
    {
        Console.WriteLine();
        Titulo("EXPEDIÇÃO ÁTOMO · Motor de Missões Mito ou Verdade");
        Console.WriteLine("1 - Cadastrar missão");
        Console.WriteLine("2 - Listar todas as missões");
        Console.WriteLine("3 - Buscar por texto");
        Console.WriteLine("4 - Filtrar por dificuldade");
        Console.WriteLine("5 - Filtrar por ponto do mapa");
        Console.WriteLine("6 - Rodada de treino (5 missões sorteadas)");
        Console.WriteLine("0 - Sair");
    }

    private void Cadastrar()
    {
        Titulo("Cadastrar missão");
        var idPonto = LerInteiro("Id do ponto do mapa (maior que zero): ");
        var afirmacao = LerLinha("Afirmação (15 a 250 caracteres): ");
        var resposta = LerResposta("Resposta correta, [V]erdade ou [M]ito: ");
        var explicacao = LerLinha("Explicação (mínimo 40 caracteres): ");
        var dificuldade = LerInteiro("Dificuldade (1 fácil, 2 média, 3 difícil): ");
        var fonte = LerLinha("URL da fonte oficial (https): ");

        var salva = _service.Cadastrar(new Missao
        {
            IdPonto = idPonto,
            Afirmacao = afirmacao,
            Resposta = resposta,
            Explicacao = explicacao,
            Dificuldade = dificuldade,
            FonteUrl = fonte
        });

        Sucesso($"Missão #{salva.Id} cadastrada. Vale {salva.Xp} XP (dificuldade {salva.Dificuldade} × 10).");
    }

    private void ListarTodas()
    {
        Titulo("Todas as missões");
        Imprimir(_service.ListarTodas(), "Nenhuma missão cadastrada.");
    }

    private void BuscarPorTexto()
    {
        Titulo("Buscar por texto");
        var termo = LerLinha("Termo (busca na afirmação e na explicação): ");
        Imprimir(_service.BuscarPorTexto(termo), $"Nenhuma missão encontrada para \"{termo.Trim()}\".");
    }

    private void FiltrarPorDificuldade()
    {
        Titulo("Filtrar por dificuldade");
        var dificuldade = LerInteiro("Dificuldade (1, 2 ou 3): ");
        Imprimir(_service.FiltrarPorDificuldade(dificuldade), "Nenhuma missão com essa dificuldade.");
    }

    private void FiltrarPorPonto()
    {
        Titulo("Filtrar por ponto do mapa");
        var idPonto = LerInteiro("Id do ponto: ");
        Imprimir(_service.FiltrarPorPonto(idPonto), $"Nenhuma missão ligada ao ponto {idPonto}.");
    }

    private void RodadaDeTreino()
    {
        var rodada = _service.SortearRodada();
        Titulo($"Rodada de treino · {rodada.Count} missões");
        int acertos = 0, xpTotal = 0;

        for (var i = 0; i < rodada.Count; i++)
        {
            var missao = rodada[i];
            Console.WriteLine();
            Console.WriteLine($"Missão {i + 1}/{rodada.Count} · {NomeDificuldade(missao.Dificuldade)} · vale {missao.Xp} XP");
            Console.WriteLine($"\"{missao.Afirmacao}\"");

            var resultado = _service.Responder(missao, LerResposta("Mito ou Verdade? [V/M]: "));
            if (resultado.Acertou)
            {
                acertos++;
                xpTotal += resultado.XpGanho;
                Sucesso($"Acertou! +{resultado.XpGanho} XP");
            }
            else
            {
                Erro($"Errou. A resposta é {missao.Resposta}.");
            }

            Console.WriteLine(missao.Explicacao);
            Console.WriteLine($"Fonte: {missao.FonteUrl}");
        }

        Console.WriteLine();
        Titulo($"Placar: {acertos}/{rodada.Count} acertos · {xpTotal} XP");
    }

    private static void Imprimir(IReadOnlyList<Missao> missoes, string mensagemVazia)
    {
        if (missoes.Count == 0)
        {
            Info(mensagemVazia);
            return;
        }

        foreach (var m in missoes)
        {
            Console.WriteLine($"#{m.Id} · Ponto {m.IdPonto} · {NomeDificuldade(m.Dificuldade)} · {m.Xp} XP · Resposta: {m.Resposta}");
            Console.WriteLine($"   {m.Afirmacao}");
            Console.WriteLine($"   Fonte: {m.FonteUrl}");
        }
        Info($"{missoes.Count} missão(ões) encontrada(s).");
    }

    private static string LerLinha(string rotulo)
    {
        Console.Write(rotulo);
        return Console.ReadLine() ?? throw new EndOfStreamException();
    }

    private static int LerInteiro(string rotulo)
    {
        while (true)
        {
            var texto = LerLinha(rotulo).Trim();
            if (int.TryParse(texto, out var valor))
                return valor;
            Erro($"\"{texto}\" não é um número inteiro. Tente de novo.");
        }
    }

    private static TipoResposta LerResposta(string rotulo)
    {
        while (true)
        {
            var texto = LerLinha(rotulo).Trim().ToUpperInvariant();
            if (texto is "V" or "VERDADE") return TipoResposta.Verdade;
            if (texto is "M" or "MITO") return TipoResposta.Mito;
            Erro("Digite V para Verdade ou M para Mito.");
        }
    }

    private static string NomeDificuldade(int dificuldade) => dificuldade switch
    {
        1 => "Fácil",
        2 => "Média",
        3 => "Difícil",
        _ => $"Nível {dificuldade}"
    };

    private static void Titulo(string texto) => Escrever($"== {texto} ==", ConsoleColor.Cyan);
    private static void Sucesso(string texto) => Escrever(texto, ConsoleColor.Green);
    private static void Erro(string texto) => Escrever(texto, ConsoleColor.Red);
    private static void Info(string texto) => Escrever(texto, ConsoleColor.Yellow);

    private static void Escrever(string texto, ConsoleColor cor)
    {
        var anterior = Console.ForegroundColor;
        Console.ForegroundColor = cor;
        Console.WriteLine(texto);
        Console.ForegroundColor = anterior;
    }
}
