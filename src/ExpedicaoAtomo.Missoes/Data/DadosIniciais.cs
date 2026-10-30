using ExpedicaoAtomo.Missoes.Models;

namespace ExpedicaoAtomo.Missoes.Data;

public static class DadosIniciais
{
    public static IEnumerable<Missao> Missoes() => new List<Missao>
    {
        new()
        {
            Id = 1, IdPonto = 1, Dificuldade = 1, Resposta = TipoResposta.Verdade,
            Afirmacao = "O IPEN, em São Paulo, produz a maior parte dos radiofármacos usados na medicina nuclear do Brasil.",
            Explicacao = "Verdade. O Instituto de Pesquisas Energéticas e Nucleares, ligado à CNEN, é o principal produtor nacional de radiofármacos, usados em exames de diagnóstico e em tratamentos.",
            FonteUrl = "https://www.ipen.br"
        },
        new()
        {
            Id = 2, IdPonto = 1, Dificuldade = 2, Resposta = TipoResposta.Mito,
            Afirmacao = "Radiofármacos servem apenas para tratar câncer e nunca são usados em exames de diagnóstico.",
            Explicacao = "Mito. A maior parte dos procedimentos de medicina nuclear é de diagnóstico, como a cintilografia do coração e dos ossos. O tratamento é só uma parte das aplicações.",
            FonteUrl = "https://www.gov.br/cnen/pt-br"
        },
        new()
        {
            Id = 3, IdPonto = 2, Dificuldade = 2, Resposta = TipoResposta.Verdade,
            Afirmacao = "O Brasil importa o molibdênio-99, insumo do tecnécio-99m usado na maioria dos exames de medicina nuclear.",
            Explicacao = "Verdade. Hoje o Mo-99 vem de reatores no exterior. O Reator Multipropósito Brasileiro, em Iperó, foi projetado para produzir esse insumo no país e reduzir a dependência.",
            FonteUrl = "https://www.gov.br/cnen/pt-br"
        },
        new()
        {
            Id = 4, IdPonto = 3, Dificuldade = 2, Resposta = TipoResposta.Verdade,
            Afirmacao = "O LABGENE, em Iperó, é um protótipo em terra da planta nuclear que vai propulsionar o submarino brasileiro.",
            Explicacao = "Verdade. O Laboratório de Geração de Energia Nucleoelétrica, do Programa Nuclear da Marinha, testa em terra o reator e os sistemas que serão usados no submarino de propulsão nuclear.",
            FonteUrl = "https://www.marinha.mil.br/ctmsp"
        },
        new()
        {
            Id = 5, IdPonto = 4, Dificuldade = 3, Resposta = TipoResposta.Verdade,
            Afirmacao = "As ultracentrífugas que enriquecem urânio em Resende usam tecnologia desenvolvida no Brasil pela Marinha.",
            Explicacao = "Verdade. A tecnologia de enriquecimento por ultracentrifugação foi desenvolvida pelo CTMSP e é usada pela INB na Fábrica de Combustível Nuclear, em Resende, que abastece Angra.",
            FonteUrl = "https://www.gov.br/inb/pt-br"
        },
        new()
        {
            Id = 6, IdPonto = 5, Dificuldade = 1, Resposta = TipoResposta.Mito,
            Afirmacao = "Usinas nucleares como Angra 1 e 2 emitem muito gás carbônico enquanto geram energia.",
            Explicacao = "Mito. A fissão nuclear não envolve combustão, então a usina não emite CO2 durante a geração. As emissões no ciclo de vida completo são baixas, comparáveis às da energia eólica.",
            FonteUrl = "https://www.iaea.org"
        },
        new()
        {
            Id = 7, IdPonto = 7, Dificuldade = 2, Resposta = TipoResposta.Mito,
            Afirmacao = "Na técnica do inseto estéril, os insetos irradiados ficam radioativos e contaminam as plantações.",
            Explicacao = "Mito. A radiação apenas esteriliza os insetos, que não ficam radioativos. Soltos no campo, eles cruzam sem gerar descendentes e a praga diminui sem uso de agrotóxico.",
            FonteUrl = "https://www.iaea.org"
        },
        new()
        {
            Id = 8, IdPonto = 8, Dificuldade = 1, Resposta = TipoResposta.Mito,
            Afirmacao = "Um alimento irradiado fica radioativo e não deve ser consumido.",
            Explicacao = "Mito. A irradiação elimina fungos, bactérias e insetos e aumenta a validade do alimento, mas não o torna radioativo, assim como uma radiografia não deixa a pessoa radioativa.",
            FonteUrl = "https://www.iaea.org"
        },
        new()
        {
            Id = 9, IdPonto = 10, Dificuldade = 2, Resposta = TipoResposta.Verdade,
            Afirmacao = "Algumas praias de Guarapari têm radiação natural acima da média por causa da areia monazítica.",
            Explicacao = "Verdade. A monazita contém tório e urânio, elementos naturalmente radioativos. É radiação natural, e a permanência curta de turistas na praia resulta numa dose pequena.",
            FonteUrl = "https://www.iag.usp.br"
        },
        new()
        {
            Id = 10, IdPonto = 11, Dificuldade = 1, Resposta = TipoResposta.Mito,
            Afirmacao = "O acidente radiológico de Goiânia, em 1987, aconteceu dentro de uma usina nuclear.",
            Explicacao = "Mito. Começou quando um aparelho de radioterapia abandonado foi levado a um ferro-velho e a cápsula de césio-137 foi aberta. Nenhuma usina estava envolvida.",
            FonteUrl = "https://www.gov.br/cnen/pt-br"
        }
    };
}
