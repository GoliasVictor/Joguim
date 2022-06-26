using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using static Engine.Helper;
using System.Collections.Concurrent;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;
using System.Drawing;

namespace Engine
{
    [Serializable] 
    public class Mapa
    {

        Stopwatch St = new Stopwatch();
        public readonly HashSet<IEntidade> Entidades = new HashSet<IEntidade>();   
		private IEnumerable<IMovel> entidadesMoveis => Entidades.OfType<IMovel>();
        
        public readonly (double x, double y) Tamanho;
        public double PixelPorUnidade = TP;
        public Estilo Background = Color.Black;
        public Cord Spawn { get; set; } = (1, 1);
        public  double Esquerda => -Tamanho.x/2;
        public  double Direita => Tamanho.x/2;
        public  double Cima => -Tamanho.y / 2;
        public  double Baixo => Tamanho.y / 2;
        public Mapa(double TamX, double TamY)
        {
            Tamanho =  (TamX, TamY );
        }
        
        public void MorteParticula(Particula particula){
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

        const double Steps = 10;
        static double VelocidadeRelativa = 1;
        public void AtualizarMapa(KeyboardState teclado, MouseState mouse,double VelocidadeTempo = 1)
        {
            //VelocidadeTempo = VelocidadeTempo == 1 ?  VelocidadeRelativa : VelocidadeTempo;
            var TotalSteps = Steps * Math.Abs(VelocidadeTempo);
            var DeltaT = 1/(Steps * VelocidadeTempo);
            St.Start();
            for (int T = 0; T < TotalSteps; T++)
            {
                Tempo += DeltaT;
                SistemaColisao.Colidir(Entidades.OfType<IColisivel>(), DeltaT);
                foreach(var Entidade in Entidades.OfType<IInputable>())
                    Entidade.Inputs.Atualizar(teclado, mouse);
                
                foreach (var Entidade in Entidades)
                    Entidade.Atualizar(DeltaT);
            }
            St.Stop();
            //MostrarVelocidade();
            //LogTicks(St.ElapsedTicks);
            St.Restart();
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
                movel.Estilo = System.Drawing.Color.White;
                var vel= movel.Mov.Velocidade;
                if (MaisRapido.Mov.Velocidade.Tamanho <= vel.Tamanho)
                    MaisRapido = movel;
                if(!double.IsNaN(vel.x) && !double.IsNaN(vel.y))
                    VelocidadeTotal +=  movel.Mov.Velocidade;
            }
            MaisRapido.Estilo = System.Drawing.Color.Red;
            if(MaisRapido.Mov.Velocidade.Tamanho !=  VelocidadeMaximaAnterior){
                VelocidadeMaximaAnterior = MaisRapido.Mov.Velocidade.Tamanho;
                Console.WriteLine("maior velocidade: " + MaisRapido.Mov.Velocidade.Tamanho/Math.Sqrt(2)); 
            }
            
            //Console.WriteLine(VelocidadeTotal + ParedeVelocidade);
            VelocidadeRelativa = 1/(MaisRapido.Mov.Velocidade.Tamanho*10);
            if(VelocidadeRelativa < 1/21153.929811923368)
                VelocidadeRelativa = 1/21153.929811923368;

            
        }
	} 
}