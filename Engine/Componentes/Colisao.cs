using static Engine.Helper;


namespace Engine
{
	public static class Colisao
	{
		static public void Estatica(IEntidade Colidido, IEntidade Colisor)
		{
			if (Colisor is IMovel ColisorMov)
				Estatica(Colidido, ColisorMov);
		}

		static public void Estatica(IEntidade Colidido, IMovel Colisor)
		{
			Vetor Dir = CalcularDirecao(Colidido, Colisor);
			ParedeVelocidade += Colisor.Mov.Velocidade;
			Colisor.Mov.CancelarMovimento(Dir);
			//Colisor.Mov.AplicarForca(2 * new Vetor(Colisor.Mov.ProximaVelocidade.x * Dir.x, Colisor.Mov.ProximaVelocidade.y * Dir.y));
		}

		static Vetor CalcularDirecao(IEntidade Colidido, IEntidade Colisor)
		{
			var A = Colidido.Lados;
			var B = Colisor.Lados;
			Vetor DirecaoColisao = new Vetor(0, 0);
			if (A.Esquerda < B.Esquerda && B.Esquerda < A.Direita)
				DirecaoColisao +=  Vetor.Esquerda;
			
			if(A.Esquerda < B.Direita  && B.Direita < A.Direita)
				DirecaoColisao +=  Vetor.Direita;
			
			if (A.Cima < B.Cima  && B.Cima  < A.Baixo)
				DirecaoColisao  +=  Vetor.Cima;
			
			if(A.Cima < B.Baixo && B.Baixo < A.Baixo)
				DirecaoColisao += Vetor.Baixo;
			return DirecaoColisao;
		}
		static public void Movel(IMovel Colidido, IEntidade Colisor)
		{
			if (Colisor is IMovel ColisorMov)
				Movel(Colidido, ColisorMov);
		}
		static public void Movel(IMovel Colidido, IMovel Colisor)
		{
			Helper.TranferirForca(Colidido, Colisor, Colidido.Mov.Velocidade);
			//var DirecaoColisao = CalcularDirecao(Colidido,Colisor).Normalizar();
			//Helper.TranferirForca(Colidido,Colisor , DirecaoColisao * Colidido.Mov.Velocidade * DirecaoColisao);
		}
	}

}