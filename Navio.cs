public abstract class Navio
{
    private int acertos;

    protected Navio(string nome, int tamanho)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(tamanho, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(tamanho, 4);
        Nome = nome;
        Tamanho = tamanho;
    }

    public string Nome { get; }
    public int Tamanho { get; }
    public bool Afundado => acertos == Tamanho;

    internal bool Posicionado { get; private set; }

    internal void RegistrarPosicionamento()
    {
        Posicionado = true;
    }

    internal void RegistrarAcerto()
    {
        acertos++;
    }
}
