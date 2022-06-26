namespace Engine
{
    public interface IBloco
    {

        Cord Posicao { get; }
        Cord ProximaPos { get; }
        double Altura { get; }
        double Largura { get; }
        IEstilo Estilo { get; }
        double Esquerda { get; }
        double Direita { get; }
        double Cima { get; }
        double Baixo { get; }
        double ProximaEsquerda { get; }
        double ProximaDireita { get; }
        double ProximaCima { get; }
        double ProximaBaixo { get; }

        void Colidir(IBloco Colisor);
    }
    public interface IAtualizavel : IBloco
    {
        void Atualizar();
    }
    public interface IMovel : IAtualizavel
    {
        Vetor Velocidade { get; }
        Vetor ProximaVelocidade { get; }
        void Mover(Cord Posicao);
        void AplicarForca(Vetor forca);
    }

    public interface IReceptor
    {
        void Receber(object e);
    }
    public interface IJogador : IMovel
    {
        Mapa Map { get; }
        void Dano(int n);
    }
}
