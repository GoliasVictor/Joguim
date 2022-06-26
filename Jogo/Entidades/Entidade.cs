using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Helper;
using Engine;

namespace Jogo
{

	public abstract class Entidade : IEntidade, IEstilizavel{

        public virtual Estilo Estilo { get; set; }
		public virtual Vetor Pos { get ; set; }
		public virtual Tamanho Tam { get ; set; }
        public virtual PosicaoLados Lados => new PosicaoLados(Pos, Tam);
        public virtual void Atualizar(double DeltaTempo){}
        public Entidade(Vetor posicao, double largura = TP, double altura= TP, Estilo estilo =  default )
        {
            Pos = posicao;
            Tam = new Tamanho(largura, altura);
            Estilo = estilo;
        }
    }
}
