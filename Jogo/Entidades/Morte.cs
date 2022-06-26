using System;

namespace Engine
{
    [Serializable]
    public class Morte : Bloco
    { 
        public override void Colidir(IBloco Colisor)
        {
            base.Colidir(Colisor);
            if (Colisor is IJogador) 
                ((IJogador)Colisor).Dano(1);
        }
        public Morte(Cord posicao, double largura = TamanhoPadrao, double altura = TamanhoPadrao, Estilo? estilo = null) : base(posicao, largura, altura)
        {
            Estilo = estilo ?? Estilo.Morte;
        }
    }
}
