public class Partida
{
    public Jogador Jogador { get; set; }
    public Jogador Maquina { get; set; }
    public bool PartidaFinalizada { get; set; }
    public Jogador Vencedor { get; set; }


    public Partida(Jogador jogador, Jogador maquina)
    {
        Jogador = jogador;
        Maquina = maquina;
        PartidaFinalizada = false;
        Vencedor = null;
    }

    public void PosicionarNaviosMaquina()
    {
        foreach(Navio navio in Maquina.Frota) //percorre todos os navios da frota
        {
            bool posicionado = false;

            while (!posicionado) 
            {
                int linha = Random.Shared.Next(0, 10);
                int coluna = Random.Shared.Next(0, 10);

                bool horizontal = Random.Shared.Next(0, 2) == 0; //retorn true se for igual a zero(horizontal)
                                                                 //e false se for igual a 1(vertical)

                posicionado = Maquina.Tabuleiro.PoicionarNavio(navio, linha, coluna, horizontal);
                //posicionarNavio é uma função booleana que retorna false caso não consiga posicionar o navio

            }

        }
    }


    public void AtaqueMaquina()
    {
        ResultadoTiro resultado;

        int linha;
        int coluna;

        do
        {
            linha = Random.Shared.Next(0, 10);
            coluna = Random.Shared.Next(0, 10);

            resultado = Jogador.Tabuleiro.ReceberTiro(linha, coluna);
        } while (resultado = ResultadoTiro.TiroRepetido); //continua até o tiro não ser em um lugar
                                                          //que tenha sido antes


    }


    public bool VerificarVitoria() //retorna false se um navio ainda não foi atingido
    {
        foreach(Navio navio in jogador.Frota)
        {
            if (!navio.Afundado)
            {
                return false;
            }
        }
        return true;
    }


    public void AtaqueJogador(int linha, int coluna)
    {

        ResultadoTiro resultado = Maquina.Tabuleiro.ReceberTiro(linha, coluna);

        if(resultado = ResultadoTiro.TiroRepetido || resultado == ResultadoTiro.ForaDoTabuleiro)
        {
            return; //se o tiro for onde ja foi ou fora do tabuleiro o jogador continua coma vez
        }

        if (VerificarVitoria(Maquina)){ // verifica se a maquina perdeu
            Vencedor = Jogador;
            PartidaFinalizada = true;
            return;
        }

        AtaqueMaquina();

        if (VerificarVitoria(Jogador)) //verifica se o jogador perdeu
        {
            Vencedor = Maquina;
            PartidaFinalizada = true;
        }

    }



}

