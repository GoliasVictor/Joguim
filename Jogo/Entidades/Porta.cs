using System;
using System.Drawing;

namespace Engine
{
    [Serializable]
    public class Porta : Bloco, IReceptor
    { 
        public override Estilo Estilo { get => Aberta ? EstiloAberta : EstiloFechada; set => throw new NotSupportedException(); }

        private Estilo EstiloFechada { get; set; }
        private Estilo EstiloAberta { get; set; } 
        public bool Aberta => RequesitosCompletos >= ResquisitosNescessarios;
        private int ResquisitosNescessarios { get; set; }
        private int RequesitosCompletos { get; set; }
        public void Receber(object e)
        {
            RequesitosCompletos++;
        }

        public override void Colidir(IMovel Colisor)
        {
            if (!Aberta) Colisor.AplicarForca(-Colisor.Velocidade);
        }

        public Porta(Cord posicao, int resquisitosNescessarios, Estilo? estiloFechada = null, Estilo? estiloAberta = null, double largura = TamanhoPadrao, double altura = TamanhoPadrao) : base(posicao, largura, altura)
        {
            EstiloFechada = estiloFechada ?? new Estilo(Color.SaddleBrown);
            EstiloAberta = estiloAberta ?? new Estilo(Color.Sienna); ;
            ResquisitosNescessarios = resquisitosNescessarios;
        }
    }
}
