using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using static Engine.Helper;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Engine
{
    [Serializable] 
    public class Mapa
    {

        Stopwatch St = new Stopwatch();
        public readonly HashSet<IEntidade> Entidades = new HashSet<IEntidade>();   
		private IEnumerable<IMovel> entidadesMoveis => Entidades.OfType<IMovel>();
        
        public readonly Tamanho Tamanho;
        readonly public PosicaoLados PosicaoLados ;
        public double PixelPorUnidade = TP;
        public IEstilo Estilo;
        public Mapa(double TamX, double TamY)
        {
            Tamanho = new Tamanho(TamX, TamY );
            PosicaoLados =  new PosicaoLados((0,0), Tamanho);
        }
        
        public void RemoverEntidade(IEntidade particula){
            Entidades.Remove(particula);
        }
        public void AdicionarEntidade(IEntidade entidade)
        {
            Entidades.Add(entidade);
        }
        public void AdicionarEntidades(IEnumerable<IEntidade> entidades)
        {
            foreach (IEntidade entidade in entidades)
                Entidades.Add(entidade);
        }

        const double Steps = 20;
        static double VelocidadeRelativa = 1;
        public void AtualizarMapa(IInputs inputs,double VelocidadeTempo = 1)
        {
            VelocidadeTempo =   VelocidadeRelativa;
            var TotalSteps = Steps ;//* Math.Abs(VelocidadeTempo);
            var DeltaT = VelocidadeTempo/Steps;
            St.Start();
            for (int T = 0; T < TotalSteps; T++)
				SubAualizacao(inputs, DeltaT);
			St.Stop();
            //MostrarMagnetudeVelocidade();
            //LogTicks(St.ElapsedTicks);
            St.Restart();
        }

		protected void SubAualizacao(IInputs inputs, double DeltaT)
		{
            Tempo += DeltaT;
			SistemaColisao.Colidir(Entidades.OfType<IColisivel>(), DeltaT);
			foreach (var Entidade in Entidades.OfType<IInputable>())
				Entidade.Inputs = inputs;
            foreach (var Entidade in Entidades)
				Entidade.Atualizar(DeltaT);

		}

		List<double> ListTicks = new List<double>();
        void LogTicks(double Ticks){
            
            ListTicks.Add(Ticks);
            var Media = ListTicks.Sum((T) => T/ListTicks.Count());
            Console.WriteLine($"{Media:000000000} ~ {Ticks:000000000}");
        }
        static double VelocidadeMaximaAnterior; 
        private void MostrarVelocidade(){
            Vetor VelocidadeTotal = default;
            IMovel MaisRapido = entidadesMoveis.First();
            foreach (IMovel movel in entidadesMoveis)
            {
                var vel= movel.Mov.Velocidade;
                if (MaisRapido.Mov.Velocidade.Tamanho <= vel.Tamanho)
                    MaisRapido = movel;
                if(!double.IsNaN(vel.x) && !double.IsNaN(vel.y))
                    VelocidadeTotal +=  movel.Mov.Velocidade;
            }
            if(MaisRapido.Mov.Velocidade.Tamanho !=  VelocidadeMaximaAnterior){
                VelocidadeMaximaAnterior = MaisRapido.Mov.Velocidade.Tamanho;
                Console.WriteLine("maior velocidade: " + MaisRapido.Mov.Velocidade.Tamanho/Math.Sqrt(2)); 
            }
            
            VelocidadeRelativa = (MaisRapido.Mov.Velocidade.Tamanho);
            if(VelocidadeRelativa > 500)
                VelocidadeRelativa = 500;
            if(VelocidadeRelativa < 1)
                VelocidadeRelativa = 1;
        }
        private void MostrarMagnetudeVelocidade(){
           double MagntudeTotal = entidadesMoveis.Sum(e => {
                var tamanho = e.Mov.Velocidade.Tamanho;
                return double.IsNaN(tamanho) ? 0 : tamanho;
            });
            var MagnetudeMedia = MagntudeTotal / entidadesMoveis.Count();
            Console.WriteLine("Velocidade Media Entidades: " + MagnetudeMedia); 

            VelocidadeRelativa = 1/MagnetudeMedia ;
            if(VelocidadeRelativa > 1)
                VelocidadeRelativa = 1;
            if(VelocidadeRelativa < 0.001)
                VelocidadeRelativa = 0.001;
        }
	} 
}