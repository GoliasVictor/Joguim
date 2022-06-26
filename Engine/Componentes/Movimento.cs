namespace Engine
{

	public interface IMovimento : IComponente
	{
		Vetor Velocidade { get; set; }
		PosicaoLados ProximosLados {get;set;}

		void AplicarForca(Vetor forca);
		void Atualizar(double DeltaTempo);
	}

	public sealed class Movimento : IMovimento
	{
		readonly IMovel Self;
		public Movimento(IMovel self, Vetor velocidade = default)
		{
			Self = self;
			Velocidade = velocidade;
			ForcaAplicada = Vetor.Zero;
			ProximaPosicao = Self.Pos;
		}

		public Vetor Velocidade { get; set; }
		private Vetor ProximaPosicao;
		public PosicaoLados ProximosLados {get;set;}
		public Vetor ProximaVelocidade => Velocidade + ForcaAplicada;

		Vetor CalcProximaPos(double DeltaTempo)
		{
			return Self.Pos + Velocidade * DeltaTempo;
		}

		private Vetor ForcaAplicada { get; set; }
		public void AplicarForca(Vetor forca) => ForcaAplicada += forca;
		public void Atualizar(double DeltaTempo)
		{
			Velocidade = ProximaVelocidade;
			ForcaAplicada = Vetor.Zero;
			Self.Pos = CalcProximaPos(DeltaTempo);
			ProximaPosicao = CalcProximaPos(DeltaTempo);
			ProximosLados = new(ProximaPosicao, Self.Tam);
		}
	}
}
