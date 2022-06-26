namespace Engine
{
	public struct PosicaoLados : IComponente { 
		public PosicaoLados(Vetor Pos,Tamanho Tam) {
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

}