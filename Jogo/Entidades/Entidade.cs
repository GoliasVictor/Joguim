using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static Engine.Helper;

namespace Engine
{

    public abstract class Bloco : IBloco
    {
        public const double TamanhoPadrao = 20;
        public virtual Cord Posicao { get; protected set; }
        public virtual Cord ProximaPos => Posicao;
        public virtual Estilo Estilo { get; set; } = new Estilo(Color.Black);
        public virtual double Altura { get; set; }
        public virtual double Largura { get; set; }
        public virtual void Colidir(IBloco Colisor)
        {
            if (Colisor is IMovel movel)
                Colidir(movel);
        }
        public virtual void Colidir(IMovel Colisor)
        {
            Vetor Dir = CalcularDirecao(Colisor);
            ParedeVelocidade += Colisor.Velocidade;
            Colisor.AplicarForca(2 * new Vetor(Colisor.ProximaVelocidade.x * Dir.x, Colisor.ProximaVelocidade.y * Dir.y));
        }

        protected Vetor CalcularDirecao(IBloco Colisor)
        {
            Vetor DirecaoColisao = new Vetor(0,0);
            if(( Esquerda <  Colisor.Esquerda  && Colisor.Esquerda < Direita 
              || Esquerda <  Colisor.Direita   && Colisor.Direita  < Direita) 
              && (Cima > Colisor.Posicao.y  || Colisor.Posicao.y  > Baixo)) 
                DirecaoColisao.y = -1;
            else if(( Cima < Colisor.Cima  && Colisor.Cima  < Baixo
              || Cima < Colisor.Baixo && Colisor.Baixo < Baixo) 
              && (Esquerda > Colisor.Posicao.x  || Colisor.Posicao.x  > Direita) ) 
                DirecaoColisao.x = -1;
            return DirecaoColisao;
        }
        public virtual double Esquerda => Posicao.x - Largura/2;
        public virtual double Direita => Posicao.x + Largura/2;
        public virtual double Cima => Posicao.y - Altura/2;
        public virtual double Baixo => Posicao.y + Altura/2;

        public virtual double ProximaEsquerda => Esquerda;
        public virtual double ProximaDireita => Direita;
        public virtual double ProximaCima => Cima;
        public virtual double ProximaBaixo => Baixo;

        IEstilo IBloco.Estilo => Estilo;

        protected Bloco(Cord posicao, double largura= TamanhoPadrao, double altura= TamanhoPadrao)
        {
            Posicao = posicao;
            Largura = largura;
            Altura = altura; 
        }
    }
}
