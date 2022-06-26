using System;

namespace Engine
{
    [Serializable]
    public class Botao : Bloco
    {
        public bool Tangivel => false;
        public override Estilo Estilo { get; set; } = Estilo.Botao;

        public bool Prescionado = false;
        public IReceptor Receptor;
        public void Emitir()
        {
            Receptor.Receber(this);
        }

        public override void Colidir(IBloco e = null)
        {
            if (!Prescionado)
            {
                Emitir();
                Prescionado = true;
            }
        }
        public Botao(Cord posicao, IReceptor receptor, double largura = 1, double altura = 1, Estilo? estilo = null) : base(posicao, largura, altura)
        {
            Receptor = receptor;
            Estilo = estilo ?? Estilo.Botao; 
        }


    }
}
