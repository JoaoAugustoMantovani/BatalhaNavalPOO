public class JogoConsole
{
    private const int TAMANHO = 10;
    private Partida partida;
    private string mensagem = "";

    public JogoConsole(Partida partida)
    {
        this.partida = partida;
    }

    public void Executar()
    {
        PosicionarFrota();
        partida.PosicionarNaviosMaquina();

        while (!partida.PartidaFinalizada)
        {
            MostrarTela();
            RealizarTiro();
        }

        MostrarTela();
        if (partida.Vencedor == partida.Jogador)
        {
            Console.WriteLine("VOCE VENCEU! Todos os navios da maquina foram afundados.");
        }
        else
        {
            Console.WriteLine("VOCE PERDEU! A maquina afundou todos os seus navios.");
        }
        Console.WriteLine();
        Console.WriteLine("Pressione Enter para sair...");
        Console.ReadLine();
    }

    private void PosicionarFrota()
    {
        foreach (Navio navio in partida.Jogador.Frota)
        {
            while (true)
            {
                MostrarTela();
                Console.Write($"Posicione o {navio.Nome} (tamanho {navio.Tamanho}) - linha coluna H/V, ex: 2 3 H: ");
                string[] partes = LerPartes();

                if (partes.Length != 3 || !LerCoordenada(partes, out int linha, out int coluna)
                    || (partes[2] != "H" && partes[2] != "V"))
                {
                    mensagem = "Formato invalido. Use: linha coluna H/V (ex: 2 3 H).";
                    continue;
                }

                if (partida.Jogador.Tabuleiro.PosicionarNavio(navio, linha, coluna, partes[2] == "H"))
                {
                    mensagem = $"{navio.Nome} posicionado.";
                    break;
                }
                mensagem = "Posicao invalida: o navio sai do tabuleiro ou fica sobre outro navio.";
            }
        }
    }

    private void RealizarTiro()
    {
        Console.Write("Sua jogada (linha coluna, ex: 2 3): ");
        string[] partes = LerPartes();

        if (partes.Length != 2 || !LerCoordenada(partes, out int linha, out int coluna))
        {
            mensagem = "Formato invalido. Digite dois numeros separados por espaco (ex: 2 3).";
            return;
        }
        if (linha < 0 || linha >= TAMANHO || coluna < 0 || coluna >= TAMANHO)
        {
            mensagem = $"Fora do tabuleiro. Use numeros de 1 a {TAMANHO}.";
            return;
        }

        Tabuleiro inimigo = partida.Maquina.Tabuleiro;
        Tabuleiro meu = partida.Jogador.Tabuleiro;
        if (inimigo.ObterCasa(linha, coluna) == EstadoCasa.Acerto || inimigo.ObterCasa(linha, coluna) == EstadoCasa.Erro)
        {
            mensagem = "Voce ja atirou nessa posicao. Tente outra.";
            return;
        }

        int afundadosAntes = Afundados(partida.Maquina);
        int afundadosMeusAntes = Afundados(partida.Jogador);
        EstadoCasa[,] meuAntes = CopiarCasas(meu);

        partida.AtaqueJogador(linha, coluna);

        mensagem = $">> Voce atirou em {linha + 1} {coluna + 1}: "
            + Descrever(inimigo.ObterCasa(linha, coluna), Afundados(partida.Maquina) > afundadosAntes);

        // AtaqueJogador nao informa o tiro da maquina, entao ele e a casa que mudou no meu tabuleiro
        for (int l = 0; l < TAMANHO; l++)
        {
            for (int c = 0; c < TAMANHO; c++)
            {
                if (meu.ObterCasa(l, c) != meuAntes[l, c])
                {
                    mensagem += $"\n>> A maquina atirou em {l + 1} {c + 1}: "
                        + Descrever(meu.ObterCasa(l, c), Afundados(partida.Jogador) > afundadosMeusAntes);
                }
            }
        }
    }

    private static string[] LerPartes()
    {
        string entrada = Console.ReadLine() ?? "";
        return entrada.ToUpper().Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }

    private static bool LerCoordenada(string[] partes, out int linha, out int coluna)
    {
        coluna = 0;
        if (!int.TryParse(partes[0], out linha) || !int.TryParse(partes[1], out coluna))
        {
            return false;
        }
        linha--;
        coluna--;
        return true;
    }

    private static int Afundados(Jogador jogador)
    {
        return jogador.Frota.Count(navio => navio.Afundado);
    }

    private static EstadoCasa[,] CopiarCasas(Tabuleiro tabuleiro)
    {
        EstadoCasa[,] casas = new EstadoCasa[TAMANHO, TAMANHO];
        for (int l = 0; l < TAMANHO; l++)
        {
            for (int c = 0; c < TAMANHO; c++)
            {
                casas[l, c] = tabuleiro.ObterCasa(l, c);
            }
        }
        return casas;
    }

    private static string Descrever(EstadoCasa casa, bool afundou)
    {
        if (afundou)
        {
            return "AFUNDOU um navio!";
        }
        return casa == EstadoCasa.Acerto ? "ACERTOU um navio!" : "agua.";
    }

    private void MostrarTela()
    {
        if (!Console.IsOutputRedirected)
        {
            Console.Clear();
        }

        Console.WriteLine("BATALHA NAVAL");
        Console.WriteLine("N = seu navio | X = acerto | O = erro | ~ = agua");
        MostrarTabuleiros();

        if (mensagem != "")
        {
            Console.WriteLine(mensagem);
            Console.WriteLine();
        }
    }

    private void MostrarTabuleiros()
    {
        Tabuleiro meu = partida.Jogador.Tabuleiro;
        Tabuleiro inimigo = partida.Maquina.Tabuleiro;
        int largura = 3 + TAMANHO * 2;
        string colunas = "   ";
        for (int coluna = 1; coluna <= TAMANHO; coluna++)
        {
            colunas += $"{coluna,-2}";
        }

        Console.WriteLine();
        Console.WriteLine("   SEU TABULEIRO".PadRight(largura + 6) + "   TABULEIRO INIMIGO");
        Console.WriteLine(colunas.PadRight(largura + 6) + colunas);

        for (int linha = 0; linha < TAMANHO; linha++)
        {
            Console.Write($"{linha + 1,2} ");
            for (int coluna = 0; coluna < TAMANHO; coluna++)
            {
                EscreverCasa(meu.ObterCasa(linha, coluna), false);
            }

            Console.Write($"      {linha + 1,2} ");
            for (int coluna = 0; coluna < TAMANHO; coluna++)
            {
                EscreverCasa(inimigo.ObterCasa(linha, coluna), true);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private static void EscreverCasa(EstadoCasa casa, bool ocultarNavio)
    {
        if (ocultarNavio && casa == EstadoCasa.Navio)
        {
            casa = EstadoCasa.Agua;
        }

        (char simbolo, ConsoleColor cor) = casa switch
        {
            EstadoCasa.Agua => ('~', ConsoleColor.Blue),
            EstadoCasa.Erro => ('O', ConsoleColor.Red),
            EstadoCasa.Acerto => ('X', ConsoleColor.Green),
            _ => ('N', ConsoleColor.DarkGray)
        };

        Console.BackgroundColor = cor;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(simbolo);
        Console.ResetColor();
        Console.Write(" ");
    }

}
