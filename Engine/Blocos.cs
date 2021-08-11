using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing; 
using static Engine.Helper;

namespace Engine
{

    [Serializable]
    public struct EstiloBloco
    {
        public Color Cor { get; set; }
        public EstiloBloco(Color color) => Cor = color;
        public static Color Aleatorio() => Color.FromArgb(Rnd.Next(255), Rnd.Next(255), Rnd.Next(255));

        public static EstiloBloco parede = new EstiloBloco(Color.FromArgb(101, 67, 33));
        public static EstiloBloco Chao = new EstiloBloco(Color.Green);
        public static EstiloBloco Morte = new EstiloBloco(Color.Firebrick);
        public static EstiloBloco Ponto = new EstiloBloco(Color.Cyan);
        public static EstiloBloco Teletransporte = new EstiloBloco(Color.Cyan);
        public static EstiloBloco Player = new EstiloBloco(Color.Blue);
        public static EstiloBloco Botao = new EstiloBloco(Color.Gray);
        public static EstiloBloco Porta = new EstiloBloco(Color.SaddleBrown);
        public static EstiloBloco Empuravel = new EstiloBloco(Color.Brown);
        public static implicit operator EstiloBloco(Color c) => new EstiloBloco(c);
    }
    public interface IBloco
    {

        Cord Posicao { get; }
        Cord ProximaPos { get; }
        double Altura { get; }
        double Largura { get; }
        EstiloBloco Estilo { get; }
        double Esquerda { get; }
        double Direita { get; }
        double Cima { get; }
        double Baixo { get; }
        double ProximaEsquerda { get; }
        double ProximaDireita { get; }
        double ProximaCima { get; }
        double ProximaBaixo { get; }

        void Colidir(IBloco Colisor);


    }
    public interface IAtualizavel : IBloco 
    {
        void Atualizar();
    }
    public interface IMovel : IAtualizavel
    {
        Vetor Velocidade { get; }
        Vetor ProximaVelocidade { get; }
        void Mover(Cord Posicao);
        void AplicarForca(Vetor forca);
    }

    public interface IReceptor
    {
        void Receber(object e);
    }
    public interface IJogador : IMovel
    {
        Mapa Map { get; }
        void Dano(int n);
    }
    public abstract class Bloco : IBloco
    {
        public const double TamanhoPadrao = 20;
        public virtual Cord Posicao { get; protected set; }
        public virtual Cord ProximaPos => Posicao;
        public virtual EstiloBloco Estilo { get; set; } = new EstiloBloco(Color.Black);
        public virtual double Altura { get; set; }
        public virtual double Largura { get; set; }
        public virtual void Colidir(IBloco Colisor)
        {
            if (Colisor is IMovel movel)
                Colidir(movel);
        }
        public virtual void Colidir(IMovel Colisor)
        {
            Vetor Dir = CalcularDirecao(Colisor);
            ParedeVelocidade += Colisor.Velocidade;
            Colisor.AplicarForca(2 * new Vetor(Colisor.ProximaVelocidade.x * Dir.x, Colisor.ProximaVelocidade.y * Dir.y));
        }

        protected Vetor CalcularDirecao(IBloco Colisor)
        {
            Vetor DirecaoColisao = new Vetor(0,0);
            if(( Esquerda <  Colisor.Esquerda  && Colisor.Esquerda < Direita 
              || Esquerda <  Colisor.Direita   && Colisor.Direita  < Direita) 
              && (Cima > Colisor.Posicao.y  || Colisor.Posicao.y  > Baixo)) 
                DirecaoColisao.y = -1;
            else if(( Cima < Colisor.Cima  && Colisor.Cima  < Baixo
              || Cima < Colisor.Baixo && Colisor.Baixo < Baixo) 
              && (Esquerda > Colisor.Posicao.x  || Colisor.Posicao.x  > Direita) ) 
                DirecaoColisao.x = -1;
            return DirecaoColisao;
        }
        public virtual double Esquerda => Posicao.x - Largura/2;
        public virtual double Direita => Posicao.x + Largura/2;
        public virtual double Cima => Posicao.y - Altura/2;
        public virtual double Baixo => Posicao.y + Altura/2;

        public virtual double ProximaEsquerda => Esquerda;
        public virtual double ProximaDireita => Direita;
        public virtual double ProximaCima => Cima;
        public virtual double ProximaBaixo => Baixo;


        protected Bloco(Cord posicao, double largura= TamanhoPadrao, double altura= TamanhoPadrao)
        {
            Posicao = posicao;
            Largura = largura;
            Altura = altura; 
        }
    }
    public abstract class BlocoMovel : Bloco, IMovel
    {
        public override double ProximaEsquerda => ProximaPos.x - Largura /2;
        public override double ProximaDireita => ProximaPos.x + Largura/2;
        public override double ProximaCima => ProximaPos.y - Altura/2;
        public override double ProximaBaixo => ProximaPos.y + Altura/2;
        protected BlocoMovel(Cord posicao, double largura = TamanhoPadrao, double altura = TamanhoPadrao, Vetor VelocidadeInicial = default) : base(posicao, largura, altura)
        {
            Velocidade = VelocidadeInicial;
        }

        public virtual Vetor Velocidade { get ; protected set; }
        public virtual Vetor ProximaVelocidade => Velocidade + ForcaAplicada;
        protected virtual Vetor ForcaAplicada { get; set; }
        public override Cord ProximaPos => Posicao + ProximaVelocidade * DeltaTempo; 
        public virtual void AplicarForca(Vetor forca) => ForcaAplicada += forca;
        public virtual void Atualizar()
        {
            Velocidade = ProximaVelocidade ;
            ForcaAplicada = default;
            Posicao = ProximaPos;
        }
 
        public override void Colidir(IMovel Colisor)
        {
            //base.Colidir(Colisor);
            Helper.TranferirForca(this, Colisor, Velocidade);
            //TransferirEnergia(Colisor,  Velocidade * (DirecaoColisao * Velocidade.Normalizar()));
        }
   
        public void Mover(Cord NovaPosicao) => Posicao = NovaPosicao;

    }

    [Serializable]
    public class Parede:Bloco
    {
        public override EstiloBloco Estilo { get; set; } 
        public Parede(Cord posicao, double largura = TamanhoPadrao, double altura= TamanhoPadrao, EstiloBloco? estilo = null) : base(posicao, largura, altura)
        {
            Estilo = estilo ?? EstiloBloco.parede;
        }
         
    }
    [Serializable]
    public class Morte : Bloco
    { 
        public override void Colidir(IBloco Colisor)
        {
            base.Colidir(Colisor);
            if (Colisor is IJogador) 
                ((IJogador)Colisor).Dano(1);
        }
        public Morte(Cord posicao, double largura = TamanhoPadrao, double altura = TamanhoPadrao, EstiloBloco? estilo = null) : base(posicao, largura, altura)
        {
            Estilo = estilo ?? EstiloBloco.Morte;
        }
    }
    [Serializable]
    public class Teletransporte : Bloco 
    {
        public Cord Saida;
        public bool Tangivel => true;
        public Teletransporte(Cord posicao, Cord Saida , EstiloBloco? estilo = null, double largura = TamanhoPadrao, double altura = TamanhoPadrao) : base(posicao, largura, altura)
        {
            this.Saida = Saida;
            Estilo = estilo ?? EstiloBloco.Teletransporte;
        }
        public override void Colidir(IBloco Colisor)
        { 
            if (Colisor is IJogador jogador) 
                jogador.Mover(Saida);
        }
    }
    [Serializable]
    public class Porta : Bloco, IReceptor
    { 
        public override EstiloBloco Estilo { get => Aberta ? EstiloAberta : EstiloFechada; set => throw new NotSupportedException(); }

        private EstiloBloco EstiloFechada { get; set; }
        private EstiloBloco EstiloAberta { get; set; } 
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

        public Porta(Cord posicao, int resquisitosNescessarios, EstiloBloco? estiloFechada = null, EstiloBloco? estiloAberta = null, double largura = TamanhoPadrao, double altura = TamanhoPadrao) : base(posicao, largura, altura)
        {
            EstiloFechada = estiloFechada ?? new EstiloBloco(Color.SaddleBrown);
            EstiloAberta = estiloAberta ?? new EstiloBloco(Color.Sienna); ;
            ResquisitosNescessarios = resquisitosNescessarios;
        }
    }
    [Serializable]
    public class Botao : Bloco
    {
        public bool Tangivel => false;
        public override EstiloBloco Estilo { get; set; } = EstiloBloco.Botao;

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
        public Botao(Cord posicao, IReceptor receptor, double largura = 1, double altura = 1, EstiloBloco? estilo = null) : base(posicao, largura, altura)
        {
            Receptor = receptor;
            Estilo = estilo ?? EstiloBloco.Botao; 
        }


    }
    [Serializable]
    public class Quadradinho : BlocoMovel, IMovel
    {
        public bool HorarioAntihoriario;
        public override void Colidir(IBloco Colisor)
        {
            base.Colidir(Colisor);
            Velocidade *= HorarioAntihoriario ^ Velocidade.x == 0 ? 1 : -1; ;
        } 
        public Quadradinho(Cord posicao, bool horarioAntihoriario, Vetor direcao, double largura = 1, double altura = 1, EstiloBloco? estilo = null) : base(posicao, largura, altura )
        {
            Posicao = posicao;
            HorarioAntihoriario = horarioAntihoriario;
            Velocidade = direcao.Normalizar();
            base.Estilo = estilo ?? EstiloBloco.Aleatorio();

        }

    }
    [Serializable]
    public class BateVolta : BlocoMovel, IMovel
    {   
        public BateVolta(Cord posicao, Vetor?  Direcao = null, double largura=TamanhoPadrao, double altura= TamanhoPadrao, EstiloBloco? estilo = null):base(posicao,largura,altura)
        {
            Posicao = posicao;
            Velocidade = Direcao ?? default;
            Estilo = estilo ??  EstiloBloco.Aleatorio();
        }
    }
    public class Particula : BateVolta
    {
        public override Cord Posicao {
            get => Vivo ? base.Posicao : Cord.NaN;
            protected set {
                if(Vivo)
                    base.Posicao = value;
            }
        }
        public bool Vivo => TempoVidaMax > TempoVida ;
        public double TempoVidaMax;
        public double TempoVida => Tempo - MomentoCriacao ;
        public event Action<Particula> Morte;
        double MomentoCriacao;
        void Morrer()
        {
            Morte?.Invoke(this);
        }
        public void ZerarCriacao(){
            MomentoCriacao = Tempo;
        }
        public Particula(Cord posicao, Action<Particula> HandlerMorte = null, int? TempoVidaMax = null,Vetor? Direcao = null, EstiloBloco? estilo = null) : base(posicao, null, 5, 5, estilo)
        {
            Morte += HandlerMorte;
            ZerarCriacao();
            Estilo = estilo ?? new EstiloBloco(Color.White);
            Velocidade = Direcao ?? new Vetor(Rnd.NextDouble() * 2 - 1, Rnd.NextDouble() * 2 - 1).Normalizar();
            this.TempoVidaMax = TempoVidaMax ?? Rnd.Next(0, 1000); 
            Posicao = posicao;

        }
        public override void Atualizar()
        {
            base.Atualizar();
            if(!Vivo)
                Morrer();
        }
    }
}
