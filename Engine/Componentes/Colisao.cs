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
			Colisor.Mov.AplicarForca(2 * new Vetor(Colisor.Mov.Velocidade.x * Dir.x, Colisor.Mov.Velocidade.y * Dir.y));
		}

		static Vetor CalcularDirecao(IEntidade Colidido, IEntidade Colisor)
		{
			var A = Colidido.Lados;
			var B = Colisor.Lados;
			Vetor DirecaoColisao = new Vetor(0, 0);
			if ((A.Esquerda < B.Esquerda && B.Esquerda < A.Direita
				|| A.Esquerda < B.Direita && B.Direita < A.Direita)
				&& (A.Cima > Colisor.Pos.y || Colisor.Pos.y > A.Baixo))
				DirecaoColisao.y = -1;
			else if ((A.Cima < B.Cima && B.Cima < A.Baixo
				|| A.Cima < B.Baixo && B.Baixo < A.Baixo)
				&& (A.Esquerda > Colisor.Pos.x || Colisor.Pos.x > A.Direita))
				DirecaoColisao.x = -1;
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
			//TransferirEnergia(Colisor,  Velocidade * (DirecaoColisao * Velocidade.Normalizar()));
		}
	}

}