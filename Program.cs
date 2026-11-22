Jogador jogador = new Jogador("Voce", new Tabuleiro(), CriarFrota());
Jogador maquina = new Jogador("Maquina", new Tabuleiro(), CriarFrota());
new JogoConsole(new Partida(jogador, maquina)).Executar();

static List<Navio> CriarFrota()
{
    return new List<Navio> { new Submarino(), new Submarino(), new Fragata(), new PortaAvioes(), new PortaAvioes() };
}
