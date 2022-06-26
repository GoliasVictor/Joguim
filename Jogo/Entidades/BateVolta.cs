using System;
using static Engine.Helper;
using Engine;

namespace Jogo
{
	[Serializable]
    public class BateVolta :Entidade,  IMovel, IColisivel
    {   
		public IMovimento Mov {get;set;} 
        public BateVolta(Vetor posicao, Vetor?  Direcao = null, double largura=TP, double altura= TP, Estilo? estilo = null)
            :base(posicao,largura,altura, estilo ??  Estilo.Aleatorio())
        {
            Mov = new Movimento(this,Direcao ?? default); 
        }
		public override void Atualizar(double DeltaTempo){
            Mov.Atualizar(DeltaTempo);
        }
		public void Colidir(IEntidade Colisor)
		{
            Colisao.Movel(this, Colisor);
		}
	}
}
