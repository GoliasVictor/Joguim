using System;

namespace Engine
{
    [Serializable]
    public class Teletransporte : Bloco 
    {
        public Cord Saida;
        public bool Tangivel => true;
        public Teletransporte(Cord posicao, Cord Saida , Estilo? estilo = null, double largura = TamanhoPadrao, double altura = TamanhoPadrao) : base(posicao, largura, altura)
        {
            this.Saida = Saida;
            Estilo = estilo ?? Estilo.Teletransporte;
        }
        public override void Colidir(IBloco Colisor)
        { 
            if (Colisor is IJogador jogador) 
                jogador.Mover(Saida);
        }
    }
}
