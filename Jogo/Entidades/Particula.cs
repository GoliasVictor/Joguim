using System;
using System.Drawing;
using static Engine.Helper;

namespace Engine
{
    public class Particula : BateVolta
    {
        public override Cord Posicao {
            get => Vivo ? base.Posicao : Cord.NaN;
            protected set {
                if(Vivo)
                    base.Posicao = value;
            }
        }
        public bool Vivo => TempoVidaMax > TempoVida ;
        public double TempoVidaMax;
        public double TempoVida => Tempo - MomentoCriacao ;
        public event Action<Particula> Morte;
        double MomentoCriacao;
        void Morrer()
        {
            Morte?.Invoke(this);
        }
        public void ZerarCriacao(){
            MomentoCriacao = Tempo;
        }
        public Particula(Cord posicao, Action<Particula> HandlerMorte = null, int? TempoVidaMax = null,Vetor? Direcao = null, Estilo? estilo = null) : base(posicao, null, 5, 5, estilo)
        {
            Morte += HandlerMorte;
            ZerarCriacao();
            Estilo = estilo ?? new Estilo(Color.White);
            Velocidade = Direcao ?? new Vetor(Rnd.NextDouble() * 2 - 1, Rnd.NextDouble() * 2 - 1).Normalizar();
            this.TempoVidaMax = TempoVidaMax ?? Rnd.Next(0, 1000); 
            Posicao = posicao;

        }
        public override void Atualizar()
        {
            base.Atualizar();
            if(!Vivo)
                Morrer();
        }
    }
}
