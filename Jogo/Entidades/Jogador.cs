using System;
using static Engine.Helper;
using Microsoft.Xna.Framework.Input;
using Engine;
using Microsoft.Xna.Framework;

namespace Jogo
{
	public interface IJogador : IInputable, IMovel, IColisivel
	{

		void Dano(int n);
		void Mover(Vetor NovaPosicao);
	}

	public class Jogador : Entidade, IJogador
	{
		public Inputs Inputs { get; set; }
		public IMovimento Mov { get; init; }
		IInputs IInputable.Inputs
		{
			get => Inputs;
			set => Inputs = value is Inputs inp ? inp : throw new InvalidCastException();
		}

		public double magnetudeVelocidade;

		public Jogador(Vetor posicao, double velocidade = 1, double largura = TP, double altura = TP, Estilo? estilo = default) : base(posicao, largura, altura, estilo ?? Color.Red)
		{
			Mov = new Movimento(this, Vetor.Zero);
			magnetudeVelocidade = velocidade;

		}
		public void Colidir(IEntidade colisor)
		{
			Colisao.Movel(this, colisor);
		}

		public override void Atualizar(double DeltaTempo)
		{
			Mov.AplicarForca(-0.01*Mov.Velocidade); ;
			
			Vetor Direcao = Vetor.Zero;

			Direcao += Inputs.EstaPresionada(Keys.Down) ?  Vetor.Baixo : Vetor.Zero;
			Direcao += Inputs.EstaPresionada(Keys.Up) 	?  Vetor.Cima : Vetor.Zero;
			Direcao += Inputs.EstaPresionada(Keys.Right)?  Vetor.Direita : Vetor.Zero;
			Direcao += Inputs.EstaPresionada(Keys.Left)	?  Vetor.Esquerda : Vetor.Zero;
			if((Mov.Velocidade + magnetudeVelocidade * Direcao).Tamanho < 10){
				Mov.AplicarForca(magnetudeVelocidade * ( Direcao != Vetor.Zero ? Direcao.Normalizar() : Vetor.Zero)); ;
			}

			Mov.Atualizar(DeltaTempo);

		}

		public void Dano(int n)
		{
			throw new NotImplementedException();
		}

		public void Mover(Vetor NovaPosicao)
		{
			Pos = NovaPosicao;
		}
	}
}
