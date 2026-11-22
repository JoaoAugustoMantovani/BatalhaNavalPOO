public class Tabuleiro
{
    public const int Tamanho = 99;
    private readonly Navio?[,] navios = new Navio?[Tamanho, Tamanho];
    private readonly bool[,] tiros = new bool[Tamanho, Tamanho];

    public bool PosicionarNavio(Navio navio, int linha, int coluna, bool horizontal)
    {
        ArgumentNullException.ThrowIfNull(navio);

        if (navio.Posicionado || !DentroDoTabuleiro(linha, coluna) ||
            navio.Tamanho > Tamanho - (horizontal ? coluna : linha))
        {
            return false;
        }

        int passoLinha = horizontal ? 0 : 1;
        int passoColuna = horizontal ? 1 : 0;

        for (int parte = 0; parte < navio.Tamanho; parte++)
        {
            int linhaDaParte = linha + parte * passoLinha;
            int colunaDaParte = coluna + parte * passoColuna;
            if (navios[linhaDaParte, colunaDaParte] is not null || tiros[linhaDaParte, colunaDaParte])
            {
                return false;
            }
        }

        for (int parte = 0; parte < navio.Tamanho; parte++)
        {
            navios[linha + parte * passoLinha, coluna + parte * passoColuna] = navio;
        }

        navio.RegistrarPosicionamento();
        return true;
    }

    // Compatibilidade com a chamada atual de Partida.PosicionarNaviosMaquina.
    public bool PoicionarNavio(Navio navio, int linha, int coluna, bool horizontal)
    {
        return PosicionarNavio(navio, linha, coluna, horizontal);
    }

    public EstadoCasa ObterCasa(int linha, int coluna)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(linha);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(linha, Tamanho);
        ArgumentOutOfRangeException.ThrowIfNegative(coluna);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(coluna, Tamanho);

        bool temNavio = navios[linha, coluna] is not null;
        if (tiros[linha, coluna])
        {
            return temNavio ? EstadoCasa.Acerto : EstadoCasa.Erro;
        }

        return temNavio ? EstadoCasa.Navio : EstadoCasa.Agua;
    }

    public ResultadoTiro ReceberTiro(int linha, int coluna)
    {
        if (!DentroDoTabuleiro(linha, coluna))
        {
            return ResultadoTiro.ForaDoTabuleiro;
        }

        if (tiros[linha, coluna])
        {
            return ResultadoTiro.TiroRepetido;
        }

        tiros[linha, coluna] = true;
        Navio? navio = navios[linha, coluna];
        if (navio is null)
        {
            return ResultadoTiro.Agua;
        }

        navio.RegistrarAcerto();
        return navio.Afundado ? ResultadoTiro.Afundou : ResultadoTiro.Acerto;
    }

    private static bool DentroDoTabuleiro(int linha, int coluna)
    {
        return linha >= 0 && linha < Tamanho && coluna >= 0 && coluna < Tamanho;
    }
}
