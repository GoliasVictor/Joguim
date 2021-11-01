namespace Engine
{
	public interface IEntidade 
	{
		Estilo Estilo{get;set;}
		Cord Pos { get; set;}
		Tamanho Tam {get;set;}
		PosicaoLados Lados {get;}
		void Atualizar(double DeltaTempo);
	}
	public interface IColisivel : IEntidade {
		void Colidir(IEntidade Colisor);
	}
	public interface IInputable
	{
		Inputs Inputs {get;}
	}
	public interface IMovel : IEntidade
	{
		Movimento Mov { get;}
	}
	public interface IReceptor : IEntidade
    {
        void Receber(object e);
    }
	public interface IJogador : IMovel,IInputable
    {
        Mapa Map { get; }
        void Dano(int n);
        void Mover(Cord NovaPosicao);
    }
}