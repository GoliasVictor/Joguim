using System;

namespace Engine
{
    [Serializable]
    public class Quadradinho : BlocoMovel, IMovel
    {
        public bool HorarioAntihoriario;
        public override void Colidir(IBloco Colisor)
        {
            base.Colidir(Colisor);
            Velocidade *= HorarioAntihoriario ^ Velocidade.x == 0 ? 1 : -1; ;
        } 
        public Quadradinho(Cord posicao, bool horarioAntihoriario, Vetor direcao, double largura = 1, double altura = 1, Estilo? estilo = null) : base(posicao, largura, altura )
        {
            Posicao = posicao;
            HorarioAntihoriario = horarioAntihoriario;
            Velocidade = direcao.Normalizar();
            base.Estilo = estilo ?? Estilo.Aleatorio();

        }

    }
}
