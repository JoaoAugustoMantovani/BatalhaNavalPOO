# Batalha Naval POO

Batalha Naval no console, em C# (.NET), feito como trabalho de Programação Orientada a Objetos. Você joga contra a máquina em um tabuleiro de 10x10. Ganha quem afundar primeiro todos os navios do outro.

## Como rodar

Precisa do .NET SDK compatível com o `TargetFramework` do `BatalhaNavalPOO.csproj` (hoje `net11.0`).

```bash
git clone https://github.com/JoaoAugustoMantovani/BatalhaNavalPOO.git
cd BatalhaNavalPOO
dotnet run
```

Rode a partir da branch `main`, que junta as três branches de trabalho.

## Como jogar

1. **Posicionar a frota.** Para cada navio, digite `linha coluna H/V`, por exemplo `2 3 H`. As coordenadas vão de 1 a 10. `H` coloca o navio na horizontal, a partir da casa para a direita. `V` coloca na vertical, a partir da casa para baixo. A posição é recusada se o navio sair do tabuleiro ou ficar sobre outro navio, e então é preciso digitar de novo.
2. **A máquina posiciona a frota dela** sozinha, em posições sorteadas.
3. **Atirar.** Digite `linha coluna`, por exemplo `5 7`. Depois de cada tiro válido seu, a máquina responde com um tiro sorteado. A tela mostra o resultado dos dois tiros: água, acertou ou afundou.
4. **Fim.** Quando um lado perde o último navio, a partida acaba e o vencedor é anunciado.

Uma entrada em formato errado, fora do tabuleiro ou em uma casa já atacada não gasta a sua vez. O jogo mostra o erro e pede outra jogada.

### Legenda do tabuleiro

O tabuleiro aparece lado a lado: o seu à esquerda e o inimigo à direita. Os navios inimigos ficam escondidos até serem atingidos.

| Símbolo | Significado |
|---|---|
| `~` | Água |
| `N` | Seu navio |
| `X` | Acerto |
| `O` | Erro (tiro na água) |

### Navios

| Tipo | Classe | Tamanho |
|---|---|---|
| Submarino | `Submarino` | 2 |
| Porta-aviões | `PortaAvioes` | 4 |
| Navio | `NavioComum` | 1 a 4 (escolhido ao criar) |

A frota de cada lado é montada em `CriarFrota()`, no `Program.cs`. Para mudar a quantidade ou os tipos de navio, basta alterar essa lista. Os dois lados usam a mesma frota.

## Estrutura do código

| Arquivo | Responsabilidade |
|---|---|
| `Navio.cs` | Classe abstrata com nome, tamanho, acertos e `Afundado` |
| `Submarino.cs`, `PortaAvioes.cs`, `NavioComum.cs` | Tipos concretos de navio |
| `Tabuleiro.cs` | Grade 10x10: posiciona navios, recebe tiros e informa o estado de cada casa |
| `EstadoCasa.cs` | O que uma casa mostra: `Agua`, `Navio`, `Acerto` ou `Erro` |
| `ResultadoTiro.cs` | Resultado de um tiro: `Agua`, `Acerto`, `Afundou`, `TiroRepetido` ou `ForaDoTabuleiro` |
| `Jogador.cs` | Nome, tabuleiro e frota (`List<Navio>`) de cada lado |
| `Partida.cs` | Regras: posicionamento da máquina, turnos, tiro da máquina e vencedor |
| `JogoConsole.cs` | Interface: leitura das entradas, validação e desenho dos tabuleiros |
| `Program.cs` | Cria os jogadores e as frotas e inicia o jogo |

As dependências seguem uma única direção: **tabuleiro → regras da partida → console**. O console só usa a `Partida` e consulta as casas pelo `Tabuleiro.ObterCasa`. As regras não sabem nada do console.

Conceitos de POO usados:

- **Herança e abstração:** `Navio` é abstrata, e `Submarino`, `PortaAvioes` e `NavioComum` herdam dela. O tabuleiro trata todos só como `Navio`.
- **Encapsulamento:** a grade e os danos ficam privados dentro do `Tabuleiro` e do `Navio`. Quem está de fora só vê os resultados por meio dos enums `ResultadoTiro` e `EstadoCasa`.
- **Composição:** a `Partida` tem dois `Jogador`, e cada um tem um `Tabuleiro` e uma lista de `Navio`.

## Divisão do trabalho

O projeto foi dividido entre três pessoas, cada uma em sua própria branch. O grupo combinou antes os nomes dos métodos, o que cada um recebe e o que devolve. Assim as três branches puderam avançar em paralelo e ser integradas na ordem das dependências.

### Pessoa 1: Navios e tabuleiro (Joao Mantovani)

Branch `feature/tabuleiro-posicionamento`

- Criou a classe abstrata `Navio` e os tipos concretos `Submarino`, `PortaAvioes` e `NavioComum`. Cada tipo define seu tamanho.
- Implementou o posicionamento horizontal e vertical no `Tabuleiro`, validando os limites e a sobreposição. A mesma regra vale para o jogador e para a máquina.
- Encapsulou a grade e os danos. Ao receber um tiro, o tabuleiro devolve um `ResultadoTiro`.
- Criou o repositório e fez a integração das branches na `main`.

### Pessoa 2: Regras da partida e máquina (Alcindo Fernando)

Branch `feature/partida-e-maquina`

- Criou `Jogador` e `Partida`. Cada lado tem seu tabuleiro e uma frota em `List<Navio>`.
- Fez a máquina posicionar a própria frota automaticamente e sortear os tiros, repetindo o sorteio quando cai em uma casa já atacada.
- Controlou a sequência de jogo. Depois de um tiro válido do jogador, a máquina joga se a partida continuar. Tiros repetidos ou fora do tabuleiro mantêm a vez do jogador. Quando um lado perde o último navio, a partida termina e o vencedor é registrado.

### Pessoa 3: Console e integração (Matheus Lopes)

Branch `feature/interface-console`

- Criou o fluxo do console para o jogador posicionar a frota, informar os tiros e acompanhar o resultado das jogadas dos dois lados.
- Validou o formato das entradas e mostrou o tabuleiro do jogador ao lado dos tiros já feitos contra a máquina, mantendo escondidos os navios inimigos ainda não atingidos.
- Testou pelo console os dois finais possíveis, o posicionamento inválido, o tiro repetido e a resposta automática da máquina.
