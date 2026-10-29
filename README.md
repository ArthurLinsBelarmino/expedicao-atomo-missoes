# Expedição Átomo · Motor de Missões (.NET)

Challenge FIAP 2026 · 2º ano ADS · Parceiro: CTMSP (Centro Tecnológico da Marinha em São Paulo)
Disciplina: Advanced Business Development with .NET · Sprint 1

## Grupo e integrantes

| Nome | RM |
|------|----|
| Arthur Lins Belarmino | RM566845 |
| Henrique Spoltore Moreno Pavão dos Santos | RM568130 |
| Raphael Oliveira Sinelli Mendonça | RM568346 |


## A solução

A **Expedição Átomo** é um app gamificado que apresenta a tecnologia nuclear para estudantes do Ensino Médio. O aluno explora um mapa do Brasil com lugares reais onde ela aparece (IPEN, Aramar, Angra, Moscamed, Guarapari, Goiânia) e, em cada ponto, cumpre missões curtas de **Mito ou Verdade** nas cinco áreas do desafio: saúde, indústria, agricultura, pesquisa científica e energia.

## O que foi desenvolvido nesta Sprint

Uma prova de conceito em console do **motor de missões**, a parte do produto responsável pela gamificação. Ela não repete a API Java, que cuida do catálogo de pontos do mapa.

- **Entidade `Missao`**: Id, IdPonto, Afirmacao, Resposta (enum Verdade/Mito), Explicacao, Dificuldade e FonteUrl.
- **Validações**: afirmação de 15 a 250 caracteres, explicação com pelo menos 40, dificuldade de 1 a 3, IdPonto maior que zero e URL https válida. Todos os erros são mostrados de uma vez.
- **Regras de negócio**:
  1. O XP é calculado (dificuldade × 10) e nunca digitado.
  2. Afirmações repetidas são recusadas, ignorando maiúsculas, acentos e pontuação final.
  3. Só são aceitas fontes oficiais: `gov.br`, `marinha.mil.br`, `ipen.br`, `usp.br`, `embrapa.br`, `iaea.org` e `unscear.org`, incluindo subdomínios. O sistema barra conteúdo sem fonte confiável.
- **Cadastro** de novas missões.
- **Consulta**: listagem de todas, busca por texto (na afirmação e na explicação), filtro por dificuldade e filtro por ponto do mapa.
- **Rodada de treino**: 5 missões sorteadas, com explicação e fonte após cada resposta e placar de XP no final.
- **Tratamento de erros** com `try/catch`: entradas inválidas mostram uma mensagem e voltam ao menu, sem quebrar a aplicação.
- **Persistência em arquivo JSON**: os dados são mantidos entre execuções. Na primeira execução, o arquivo é criado com 10 missões de teste ligadas aos pontos reais do mapa. Se o arquivo estiver corrompido, é salvo como `.bak` e recriado.

## Arquitetura

```
src/ExpedicaoAtomo.Missoes/
├── Program.cs                 Ponto de entrada: monta as dependências e abre o menu
├── Models/                    Missao, TipoResposta, ResultadoResposta
├── Validation/                MissaoValidator: validações dos atributos
├── Services/                  MissaoService: regras de negócio e consultas
│                              FonteConfiavel: lista de domínios oficiais
├── Repositories/              IMissaoRepository + JsonMissaoRepository (persistência)
├── Exceptions/                ValidacaoException, RegraNegocioException
├── Utils/                     TextoUtils: normalização de texto (acentos, caixa)
├── Data/                      DadosIniciais: missões de teste
└── Ui/                        ConsoleUi: menu e tratamento de erros
```

Fluxo: `ConsoleUi` → `MissaoService` (valida e aplica as regras) → `IMissaoRepository` → arquivo JSON.
O repositório fica atrás de uma interface, então na Sprint 2 a troca para Entity Framework Core + Oracle não muda o serviço nem o menu.

## Como executar

Pré-requisito: [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0).

**Visual Studio:** abra `ExpedicaoAtomo.Missoes.sln` e pressione **F5**.

**Terminal:**

```bash
git clone <url-do-repositorio>
cd ExpedicaoAtomo.Missoes
dotnet run --project src/ExpedicaoAtomo.Missoes
```

O arquivo de dados é criado automaticamente em `bin/Debug/net8.0/data/missoes.json`, ao lado do executável. Para voltar às missões iniciais, basta apagar esse arquivo.

Sugestões de teste: cadastrar uma missão com fonte `https://www.gov.br/cnen/pt-br` (aceita) e outra com `https://fakegov.br` (recusada), e repetir uma afirmação trocando maiúsculas e acentos (recusada).

## Como o .NET se aplica à solução do grupo

Na Expedição Átomo, o .NET é o motor da gamificação e da qualidade do conteúdo. Enquanto a API Java serve o catálogo de pontos do mapa, o .NET decide quanto vale cada missão, impede conteúdo repetido e só aceita afirmações apoiadas em fontes oficiais, o que garante que o estudante aprenda com informação confiável e não com boatos sobre radiação. A separação entre modelo, validação, regras, repositório e interface deixa a lógica pronta para virar uma Web API em ASP.NET Core na Sprint 2, consumida pelo app Flutter, e para trocar o JSON por Entity Framework Core com Oracle sem reescrever as regras.

## Tecnologias

C# 12 · .NET 8 · System.Text.Json
