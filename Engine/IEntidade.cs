namespace Engine
{
	public interface IEntidade 
	{
		Vetor Pos { get; set;}
		Tamanho Tam {get;set;}
		PosicaoLados Lados {get;}
		void Atualizar(double DeltaTempo);
	}
	public interface IColisivel : IEntidade {
		void Colidir(IEntidade Colisor);
	}
	public interface IInputable
	{
		IInputs Inputs {get;set;}
	}
	public interface IMovel : IEntidade
	{
		IMovimento Mov { get;}
	}
	public interface IReceptor : IEntidade
    {
        void Receber(object e);
    }
}