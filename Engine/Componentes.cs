using System;
using System.Drawing;
using static Engine.Helper;
using Microsoft.Xna.Framework.Input;


namespace Engine
{
	public interface IComponente{

	};


	public struct PosicaoLados{ 
		public PosicaoLados(Cord Pos,Tamanho Tam) {
			Esquerda = Pos.x - Tam.Largura/2;
			Direita  = Pos.x + Tam.Largura/2;
        	Cima  = Pos.y - Tam.Altura/2;
        	Baixo = Pos.y + Tam.Altura/2;
		}
		readonly public double Esquerda ;
        readonly public double Direita;
        readonly public double Cima;
        readonly public double Baixo;
	}
	public class Movimento : IComponente
	{
		readonly IMovel Self;
		private Vetor velocidade;
		

		public Movimento(IMovel self, Vetor velocidade = default)
		{
			Self = self;
			Velocidade = velocidade;
			ForcaAplicada = Vetor.Zero;
		}

		public Vetor Velocidade { get;set; }
		public Vetor ProximaVelocidade => Velocidade + ForcaAplicada;
		public Cord ProximaPos(double DeltaTempo)
		{
			return Self.Pos + Velocidade * DeltaTempo;
		}
		public PosicaoLados ProximosLados(double DeltaTempo)
		{
			return new(ProximaPos(DeltaTempo), Self.Tam);
		}
		private Vetor ForcaAplicada { get; set; }
		public void AplicarForca(Vetor forca) => ForcaAplicada += forca;
		public void Atualizar(double DeltaTempo)
		{
			Velocidade = ProximaVelocidade;
			ForcaAplicada = default;
			Self.Pos = ProximaPos(DeltaTempo);
		}
	}
	public struct Tamanho : IComponente {
		public IEntidade Self ;

		public Tamanho(IEntidade self, double largura, double altura)
		{
			Self = self;
			Altura = altura;
			Largura = largura;
		}

		public double Altura { get;  set;} 
		public double Largura { get;set; }

 
	} 
	public class Inputs : IComponente {
		public KeyboardState Teclado {get; private set;}
		public MouseState Mouse {get;private set;}
		public void Atualizar(KeyboardState novoEstadoTeclado, MouseState novoEstadoMouse ){
			Teclado = novoEstadoTeclado;
			Mouse =  novoEstadoMouse;
		}
	}
	public static class Colisao { 
		static public void Estatica( IEntidade Colidido,IEntidade	 Colisor)
        {
			if (Colisor is IMovel ColisorMov)
				Estatica(Colidido, ColisorMov);
        }

		static public void Estatica( IEntidade Colidido,IMovel Colisor)
        {
			Vetor Dir = CalcularDirecao(Colidido,Colisor);
			ParedeVelocidade += Colisor.Mov.Velocidade;
			Colisor.Mov.AplicarForca(2* new Vetor(Colisor.Mov.ProximaVelocidade.x * Dir.x, Colisor.Mov.ProximaVelocidade.y * Dir.y));
        }
 
        static  Vetor CalcularDirecao(IEntidade Colidido,IEntidade Colisor)
        {
			var A = Colidido.Lados;
			var B = Colisor.Lados;
            Vetor DirecaoColisao = new Vetor(0,0);
            if(( A.Esquerda <  B.Esquerda  && B.Esquerda < A.Direita 
              || A.Esquerda <  B.Direita   && B.Direita  < A.Direita) 
              && (A.Cima > Colisor.Pos.y  || Colisor.Pos.y  > A.Baixo)) 
                DirecaoColisao.y = -1;
            else if(( A.Cima < B.Cima  && B.Cima  < A.Baixo
              || A.Cima < B.Baixo && B.Baixo < A.Baixo) 
              && (A.Esquerda > Colisor.Pos.x  || Colisor.Pos.x  > A.Direita) ) 
                DirecaoColisao.x = -1;
            return DirecaoColisao;
        }
		static public void Movel( IMovel Colidido,IEntidade	 Colisor)
        {
			if ( Colisor is IMovel ColisorMov )
				Movel(Colidido, ColisorMov);
        }
		static  public void Movel(IMovel Colidido, IMovel Colisor)
        {
			Helper.TranferirForca(Colidido, Colisor, Colidido.Mov.Velocidade);
			//TransferirEnergia(Colisor,  Velocidade * (DirecaoColisao * Velocidade.Normalizar()));
		}
	} 

    public struct Estilo
    {
        public Color Cor { get; set; }
        public Estilo(Color color) => Cor = color;
        public static Color Aleatorio() => Color.FromArgb(Rnd.Next(255), Rnd.Next(255), Rnd.Next(255));

        public static Estilo parede = new Estilo(Color.FromArgb(101, 67, 33));
        public static Estilo Chao = new Estilo(Color.Green);
        public static Estilo Morte = new Estilo(Color.Firebrick);
        public static Estilo Ponto = new Estilo(Color.Cyan);
        public static Estilo Teletransporte = new Estilo(Color.Cyan);
        public static Estilo Player = new Estilo(Color.Blue);
        public static Estilo Botao = new Estilo(Color.Gray);
        public static Estilo Porta = new Estilo(Color.SaddleBrown);
        public static Estilo Empuravel = new Estilo(Color.Brown);
        public static implicit operator Estilo(Color c) => new Estilo(c);
    }
    
}