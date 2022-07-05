namespace Engine
{

	public interface IMovimento : IComponente
	{
		Vetor Velocidade { get; set; }
		PosicaoLados ProximosLados {get;set;}

		void AplicarForca(Vetor forca);
		void Atualizar(double DeltaTempo);
		void CancelarMovimento(Vetor direcao);
	}
	
	public class Movimento : IMovimento
	{
		readonly IMovel Self;
		public Movimento(IMovel self, Vetor velocidade = default)
		{
			Self = self;
			Velocidade = velocidade;
			ForcaAplicada = Vetor.Zero;
		}

		public Vetor Velocidade { get; set; }
		Vetor DirecaoCancelada ;

		public PosicaoLados ProximosLados {get;set;}
		public Vetor CalcProximaVelocidade(){
			var vel = Velocidade + ForcaAplicada;
			var Dir = DirecaoCancelada;
			if(Dir != Vetor.Zero){
				var aux = vel -  Dir * vel * Dir;
				if(Dir.x * vel.x > 0)
					vel.x = aux.x;
				if(Dir.y * vel.y > 0)
					vel.y =  aux.y;
			}
			return vel;
		}

		Vetor CalcProximaPos(double DeltaTempo)
		{
			return Self.Pos + Velocidade * DeltaTempo;
		}

		private Vetor ForcaAplicada { get; set; }
		public void AplicarForca(Vetor forca) => ForcaAplicada += forca;
		public void Atualizar(double DeltaTempo)
		{
			Velocidade = CalcProximaVelocidade();
			ForcaAplicada = Vetor.Zero;
			Self.Pos = CalcProximaPos(DeltaTempo);
			var ProximaPosicao = CalcProximaPos(DeltaTempo);
			ProximosLados = new(ProximaPosicao, Self.Tam);
			DirecaoCancelada = Vetor.Zero;
		}
		
		public void CancelarMovimento(Vetor direcao){
			DirecaoCancelada += direcao.Normalizar();
		}
	}
}
