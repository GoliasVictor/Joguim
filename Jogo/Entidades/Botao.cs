using System;
using static Engine.Helper;
using Engine;

namespace Jogo
{
	[Serializable]
    public class Botao : Entidade, IColisivel
    { 
		public bool Prescionado = false;
        public IReceptor Receptor;
        public void Emitir()
        {
            Receptor.Receber(this);
        }

        public void Colidir(IEntidade e = null)
        {
            if (!Prescionado)
            {
                Emitir();
                Prescionado = true;
            } 
        }

		public void Atualizar(long DeltaTempo){}

		public Botao(Vetor posicao, IReceptor receptor, double largura = TP, double altura = TP, Estilo? estilo = null) 
        : base(posicao, largura, altura, estilo ?? Estilo.Botao)
        {
            Receptor = receptor; 
        }


    }
}
