public class Jogador
{

    public string Nome { get; set; }
    public Tabuleiro Tabuleiro { get; set; }
    public List<Navio> Frota { get; set; }

    public Jogador(string nome, Tabuleiro tabuleiro, List<Navio> frota )
    {
        Nome = nome;
        tabuleiro = tabuleiro;
        Frota = frota;
    }



}

