using System;

namespace Engine
{
    [Serializable]
    public class Parede:Bloco
    {
        public override Estilo Estilo { get; set; } 
        public Parede(Cord posicao, double largura = TamanhoPadrao, double altura= TamanhoPadrao, Estilo? estilo = null) : base(posicao, largura, altura)
        {
            Estilo = estilo ?? Estilo.parede;
        }
         
    }
}
