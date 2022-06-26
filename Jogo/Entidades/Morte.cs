using System;
using static Engine.Helper;
using Engine;

namespace Jogo
{
	[Serializable]
    public class Morte : Entidade, IColisivel 
    {   
        public void Colidir(IEntidade Colisor)
        {
            if (Colisor is IJogador) 
                ((IJogador)Colisor).Dano(1);
             Colisao.Estatica(this, Colisor);
        }
        public Morte(Vetor posicao, double largura = TP, double altura = TP, Estilo? estilo = null)  
            : base (posicao, largura,altura, estilo ?? Estilo.Morte)
        {  
        }
    }
}
