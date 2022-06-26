using static Engine.Helper;

namespace Engine
{
    public abstract class BlocoMovel : Bloco, IMovel
    {
        public override double ProximaEsquerda => ProximaPos.x - Largura /2;
        public override double ProximaDireita => ProximaPos.x + Largura/2;
        public override double ProximaCima => ProximaPos.y - Altura/2;
        public override double ProximaBaixo => ProximaPos.y + Altura/2;
        protected BlocoMovel(Cord posicao, double largura = TamanhoPadrao, double altura = TamanhoPadrao, Vetor VelocidadeInicial = default) : base(posicao, largura, altura)
        {
            Velocidade = VelocidadeInicial;
        }

        public virtual Vetor Velocidade { get ; protected set; }
        public virtual Vetor ProximaVelocidade => Velocidade + ForcaAplicada;
        protected virtual Vetor ForcaAplicada { get; set; }
        public override Cord ProximaPos => Posicao + ProximaVelocidade * DeltaTempo; 
        public virtual void AplicarForca(Vetor forca) => ForcaAplicada += forca;
        public virtual void Atualizar()
        {
            Velocidade = ProximaVelocidade ;
            ForcaAplicada = default;
            Posicao = ProximaPos;
        }
 
        public override void Colidir(IMovel Colisor)
        {
            //base.Colidir(Colisor);
            Helper.TranferirForca(this, Colisor, Velocidade);
            //TransferirEnergia(Colisor,  Velocidade * (DirecaoColisao * Velocidade.Normalizar()));
        }
   
        public void Mover(Cord NovaPosicao) => Posicao = NovaPosicao;

    }
}
