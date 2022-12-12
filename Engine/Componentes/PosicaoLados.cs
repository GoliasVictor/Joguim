namespace Engine
{
	public record struct PosicaoLados : IComponente { 
		public PosicaoLados(Vetor Pos,Tamanho Tam) {
			Esquerda = Pos.x - Tam.Largura/2;
			Direita  = Pos.x + Tam.Largura/2;
        	Cima  = Pos.y - Tam.Altura/2;
        	Baixo = Pos.y + Tam.Altura/2;
		}
		public PosicaoLados(double esquerda, double direita, double cima, double baixo){
			Esquerda = esquerda;
			Direita = direita;
			Cima = cima;
			Baixo = baixo;
		}
		public readonly double Esquerda { get; init; }
        public readonly double Direita { get; init; }
        public readonly double Cima { get; init; }
        public readonly double Baixo { get; init; }
	}

}