using System;
using static Engine.Helper;
using Engine;

namespace Jogo
{
	[Serializable]
    public class Teletransporte : Entidade, IColisivel
    { 
        public Vetor Saida; 
        public Teletransporte(Vetor posicao, Vetor saida , Estilo? estilo = null, double largura = TP, double altura = TP) 
            : base (posicao, largura,altura, estilo ?? Estilo.Teletransporte)
        { 
            Saida = saida;
        }
        public void Colidir(IEntidade Colisor)
        { 
            if (Colisor is IJogador jogador) 
                jogador.Mover(Saida);
            Colisao.Estatica(this, Colisor);
        }
    }
}
