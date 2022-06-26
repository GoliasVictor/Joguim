using System;
using static Engine.Helper;
using Engine;

namespace Jogo
{
	public class Particula : Entidade, IMovel, IColisivel
    { 
        public IMovimento Mov {get;}  
        public override Vetor Pos {
            get => Vivo ? base.Pos : Vetor.NaN;
            set {
                if(Vivo)
                    base.Pos = value;
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
        public Particula(Vetor posicao, Action<Particula> HandlerMorte = null, double? tempoVidaMax = null,Vetor? Direcao = null,double velocidade = 1, Estilo? estilo = null) 
        : base (posicao, TP/4,TP/4)
        {
            base.Pos = posicao;
            Morte += HandlerMorte;
            Estilo = estilo ?? new Estilo(Microsoft.Xna.Framework.Color.White);
            Mov = new Movimento(this, Direcao ?? Vetor.Polar(velocidade, Rnd.NextDouble() * 2 * Math.PI));
            TempoVidaMax = tempoVidaMax ?? Rnd.Next(0, 1000);
            ZerarCriacao();
            

        }
        public override void Atualizar( double DeltaTempo)
        { 
            if(!Vivo)
                Morrer();
            Mov.Atualizar(DeltaTempo);
        }
		public override string ToString()
		{
			return $"Pos:{Pos}";
		}
		public void Colidir(IEntidade Colisor)
		{ 
            Colisao.Movel(this, Colisor);
		}
	}
}
