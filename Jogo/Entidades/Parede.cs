using System;
using static Engine.Helper;
using Engine;
using Microsoft.Xna.Framework;

namespace Jogo
{
	[Serializable]
    public class Parede : Entidade, IColisivel
    {

        public void Colidir(IEntidade Colisor) => Colisao.Estatica(this, Colisor);
        public Parede(Vetor posicao, double largura = TP, double altura= TP, Estilo? estilo = default) 
        : base (posicao, largura, altura, estilo ?? new Estilo(new Color(101, 67, 33)))
        {
        }
         
    }
}
