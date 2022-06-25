using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using static Engine.Helper;
using System.Collections.Concurrent;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;

namespace Engine
{
    [Serializable] 
    public class Mapa
    {

        Stopwatch St = new Stopwatch();
        public readonly HashSet<IEntidade> Entidades = new HashSet<IEntidade>();   
        public readonly (double x, double y) Tamanho;
        public Cord Spawn { get; set; } = (1, 1);
        public  double Esquerda => -Tamanho.x/2;
        public  double Direita => Tamanho.x/2;
        public  double Cima => -Tamanho.y / 2;
        public  double Baixo => Tamanho.y / 2;
        public Mapa(int TamX, int TamY)
        {
            Tamanho =  (TamX - TamanhoPadrao, TamY - TamanhoPadrao);
            var Grossura = TamanhoPadrao;
            Entidades.Add(new Parede(new Cord(Esquerda-Grossura/2, 0), Grossura, TamY + Grossura ));
            Entidades.Add(new Parede(new Cord(Direita + Grossura / 2, 0), Grossura, TamY + Grossura ));
            Entidades.Add(new Parede(new Cord(0,Cima - Grossura / 2), TamX + Grossura , Grossura));
            Entidades.Add(new Parede(new Cord(0,Baixo + Grossura / 2), TamX + Grossura, Grossura));
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
        static bool Colidindo(PosicaoLados A, PosicaoLados B)
        {
            return A.Direita > B.Esquerda && A.Cima < B.Baixo
                && B.Direita > A.Esquerda && B.Cima < A.Baixo;
        } 
        static void Colidir(IColisivel A, IColisivel B)
        {  
            B.Colidir(A);
            A.Colidir(B);
        }
        #region ColisaoParalela 
        static void ColidirParalelo(IEnumerable<IColisivel> entidades){
            var Sublists = entidades.Select((B) => new List<IColisivel>(){B});
            ColidirEJuntar(new ConcurrentStack<List<IColisivel>>(Sublists));
        }
        static ConcurrentStack<List<IColisivel>> ColidirEJuntar(ConcurrentStack<List<IColisivel>> StackListsEntidade){
            var ListasUnidas =  new ConcurrentStack<List<IColisivel>>();
            List<Task> Tasks =  new List<Task>();
            while(StackListsEntidade.Count > 1){
                StackListsEntidade.TryPop(out var A);
                StackListsEntidade.TryPop(out var B);
                Tasks.Add( Task.Run(()=>{
                    ListasUnidas.Push(ColidirEJuntarLists(A,B));
                }));
            }
            if(StackListsEntidade.Count == 1){
                StackListsEntidade.TryPop(out var Lista);
                ListasUnidas.Push(Lista);
            }
            Task.WhenAll(Tasks).Wait();
            return ListasUnidas.Count < 2 ? ListasUnidas : ColidirEJuntar(ListasUnidas);
        }
        static List<IColisivel> ColidirEJuntarLists(List<IColisivel> entidades1,List<IColisivel> entidades2){
            if(entidades2 is null)
                return entidades1;
            if(entidades1 is null)
                return entidades2;
            
            Parallel.ForEach(entidades1,(A)=>{
                foreach (var B in entidades2)
                    if(Colidindo(A.Lados, B.Lados))
                        Colidir(A, B);
            });            
            return entidades1.Union(entidades2).ToList();
        }
        #endregion

        struct WraperColisao {
            public readonly PosicaoLados Lados;
            public readonly IColisivel Entidade;
            public readonly bool Estatico;

            public WraperColisao(IColisivel entidade)
            {
                Entidade = entidade;
                if (entidade is IMovel entidadeMovel)
                {
                    Lados = entidadeMovel.Mov.ProximosLados;
                    Estatico = false;
                }
                else
                {
                    Lados = entidade.Lados;
                    Estatico = true;
                }
            }
        }
        static void Colidir(IEnumerable<IColisivel> entidades, double DeltaT)
        {
            if (entidades is null)
                throw new ArgumentNullException(nameof(entidades));
            var EntidadesWrapers = entidades.Select(e => new WraperColisao(e)).ToArray();
            for(int i = 0; i < EntidadesWrapers.Length; i ++)
            {
                WraperColisao A = EntidadesWrapers[i];
                for (int j = i; j < EntidadesWrapers.Length; j++)
                {
                    WraperColisao B = EntidadesWrapers[j];
                    if (A.Estatico && B.Estatico)
                        continue;
                    
                    var EstaColidindo = Colidindo(A.Lados, B.Lados);
                    if (EstaColidindo)
                        Colidir(A.Entidade, B.Entidade);
                }
            }
        }

        const double Steps = 2;
        public void AtualizarMapa(KeyboardState teclado, MouseState mouse,double VelocidadeTempo = 1)
        {
            
            var TotalSteps = Steps * Math.Abs(VelocidadeTempo);
            var DeltaT = 1 /(Steps * VelocidadeTempo);
            St.Start();
            for (int T = 0; T < TotalSteps; T++)
            {
                Tempo += DeltaT;
                var Colisiveis = Entidades.OfType<IColisivel>();
                Colidir(Colisiveis, DeltaT);
                Parallel.ForEach(Entidades.OfType<IInputable>(), (Entidade) =>
                {
                    Entidade.Inputs.Atualizar(teclado, mouse);
                });
                
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
            Console.WriteLine("maior velocidade: " + MaisRapido); 
            Console.WriteLine(VelocidadeTotal + ParedeVelocidade);
        }
		private IEnumerable<IMovel> entidadesMoveis => Entidades.OfType<IMovel>();
	} 
}