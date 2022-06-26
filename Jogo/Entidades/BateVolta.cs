using System;

namespace Engine
{
    [Serializable]
    public class BateVolta : BlocoMovel, IMovel
    {   
        public BateVolta(Cord posicao, Vetor?  Direcao = null, double largura=TamanhoPadrao, double altura= TamanhoPadrao, Estilo? estilo = null):base(posicao,largura,altura)
        {
            Posicao = posicao;
            Velocidade = Direcao ?? default;
            Estilo = estilo ??  Estilo.Aleatorio();
        }
    }
}
