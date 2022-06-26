using System;
using static Engine.Helper;
using Engine;

namespace Jogo
{
	[Serializable]
    public class Quadradinho :  Entidade, IMovel, IColisivel
    {
        public IMovimento Mov {get;}  
        public bool HorarioAntihoriario;
        public void Colidir(IEntidade Colisor)
        {
            Mov.Velocidade *= HorarioAntihoriario ^ Mov.Velocidade.x == 0 ? 1 : -1; ;
            Colisao.Estatica(this, Colisor);

        }

		public Quadradinho(Vetor posicao, bool horarioAntihoriario, Vetor direcao, double largura = TP, double altura = TP, Estilo? estilo = null) 
         :base(posicao, largura, altura, estilo ?? Estilo.Aleatorio())
        {
            HorarioAntihoriario = horarioAntihoriario;
            Mov = new Movimento(this, direcao.Normalizar()); 
        }

    }
}
