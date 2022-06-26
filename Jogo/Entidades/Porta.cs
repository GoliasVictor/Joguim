using System;
using static Engine.Helper;
using Engine;
using Microsoft.Xna.Framework;

namespace Jogo
{
	[Serializable]
    public class Porta : Entidade, IColisivel, IReceptor
    {  
        private Estilo EstiloFechada { get; set; }
        private Estilo EstiloAberta { get; set; } 
        public bool Aberta => RequesitosCompletos >= ResquisitosNescessarios;
        private int ResquisitosNescessarios { get; set; }
        private int RequesitosCompletos { get; set; }
        public override Estilo Estilo => Aberta ? EstiloAberta :  EstiloFechada;
        public void Receber(object e)
        {
            RequesitosCompletos++;
        }

        public void Colidir(IEntidade Colisor) {
            if(Aberta)             
                Colisao.Estatica(this, Colisor);

        } 
        

        public Porta(Vetor posicao, int resquisitosNescessarios, Estilo? estiloFechada = null, Estilo? estiloAberta = null, double largura = TP, double altura = TP) 
            :base(posicao, altura, largura,  default)
        {
            EstiloFechada = estiloFechada ?? new Estilo(Color.SaddleBrown);
            EstiloAberta = estiloAberta ?? new Estilo(Color.Sienna); ;
            ResquisitosNescessarios = resquisitosNescessarios;
        }
    }
}
