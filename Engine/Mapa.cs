using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using static Engine.Helper;
using System.Collections.Concurrent;

namespace Engine
{
    [Serializable] 
    public class Mapa
    {

        Stopwatch St = new Stopwatch();
        public readonly HashSet<IBloco> Blocos = new HashSet<IBloco>();   
        public readonly (double x, double y) Tamanho;
        public Cord Spawn { get; set; } = (1, 1);
        public  double Esquerda => -Tamanho.x/2;
        public  double Direita => Tamanho.x/2;
        public  double Cima => -Tamanho.y / 2;
        public  double Baixo => Tamanho.y / 2;
        public Mapa(int TamX, int TamY)
        {
            Tamanho =  (TamX - Bloco.TamanhoPadrao, TamY - Bloco.TamanhoPadrao);
            var Grossura = Bloco.TamanhoPadrao;
            Blocos.Add(new Parede(new Cord(Esquerda-Grossura/2, 0), Grossura, TamY + Grossura ));
            Blocos.Add(new Parede(new Cord(Direita + Grossura / 2, 0), Grossura, TamY + Grossura ));
            Blocos.Add(new Parede(new Cord(0,Cima - Grossura / 2), TamX + Grossura , Grossura));
            Blocos.Add(new Parede(new Cord(0,Baixo + Grossura / 2), TamX + Grossura, Grossura));
        }
        public void MorteParticula(Particula particula){
            Blocos.Remove(particula);
        }
        public void AdicionarBloco(IBloco bloco)
        {
            Blocos.Add(bloco);
        }
        public void AdicionarBlocos(IEnumerable<IBloco> blocos)
        {
            foreach (IBloco Bloco in blocos)
                Blocos.Add(Bloco);
        }
        static bool Colidindo(IBloco A, IBloco B)
        {
            return A.ProximaDireita > B.ProximaEsquerda && A.ProximaCima < B.ProximaBaixo
                && B.ProximaDireita > A.ProximaEsquerda && B.ProximaCima < A.ProximaBaixo;
        } 
        static void Colidir(IBloco A, IBloco B)
        {  
            B.Colidir(A);
            A.Colidir(B);
        }
        #region ColisaoParalela 
        static void ColidirParalelo(IEnumerable<IBloco> blocos){
            var Sublists = blocos.Select((B) => new List<IBloco>(){B});
            ColidirEJuntar(new ConcurrentStack<List<IBloco>>(Sublists));
        }
        static ConcurrentStack<List<IBloco>> ColidirEJuntar(ConcurrentStack<List<IBloco>> StackListsBloco){
            var ListasUnidas =  new ConcurrentStack<List<IBloco>>();
            List<Task> Tasks =  new List<Task>();
            while(StackListsBloco.Count > 1){
                StackListsBloco.TryPop(out var A);
                StackListsBloco.TryPop(out var B);
                Tasks.Add( Task.Run(()=>{
                    ListasUnidas.Push(ColidirEJuntarLists(A,B));
                }));
            }
            if(StackListsBloco.Count == 1){
                StackListsBloco.TryPop(out var Lista);
                ListasUnidas.Push(Lista);
            }
            Task.WhenAll(Tasks).Wait();
            return ListasUnidas.Count < 2 ? ListasUnidas : ColidirEJuntar(ListasUnidas);
        }
        static List<IBloco> ColidirEJuntarLists(List<IBloco> Blocos1,List<IBloco> Blocos2){
            if(Blocos2 is null)
                return Blocos1;
            if(Blocos1 is null)
                return Blocos2;
            Parallel.ForEach(Blocos1,(A)=>{
                foreach (var B in Blocos2)
                    if(Colidindo(A, B))
                        Colidir(A, B);
            });            
            return Blocos1.Union(Blocos2).ToList();
        }
        #endregion
        static void Colidir(IEnumerable<IBloco> blocos)
        {
            if (blocos is null)
                throw new ArgumentNullException(nameof(blocos));
            var Blocos = new Stack<IBloco>(blocos);
            while(Blocos.Count > 0)
            {
                var A = Blocos.Pop();
                foreach (var B in Blocos)
                    if(Colidindo(A, B))
                        Colidir(A, B);
            }
        }
        readonly double Steps = 2;
        public void AtualizarMapa(double VelocidadeTempo = 1)
        {
            
            var TotalSteps = Steps * Math.Abs(VelocidadeTempo);
            var DeltaT = 1 /(Steps * VelocidadeTempo);
            St.Start();
            for (int T = 0; T < TotalSteps; T++)
            {
                Tempo += DeltaT;
                Colidir(Blocos);
                Parallel.ForEach(  BlocosAtualizaveis, 
                    (BlocoAtualizavel) =>  BlocoAtualizavel.Atualizar() 
                );
            }
            St.Stop();
            LogTicks(St.ElapsedTicks);
            St.Restart();
        }   
        List<double> ListTicks = new List<double>();
        void LogTicks(double Ticks){
            
            ListTicks.Add(Ticks);
            var Media = ListTicks.Sum((T) => T/ListTicks.Count());
            Console.WriteLine($"{Math.Log2(Media):000000000} ~ {Math.Log2(Ticks):000000000}");
        }
        private void MostrarVelocidade(){
            Vetor VelocidadeTotal = default;
            foreach (IMovel movel in BlocosMoveis)
                VelocidadeTotal += movel.Velocidade;
            Console.WriteLine(VelocidadeTotal + ParedeVelocidade);
        }
        private IEnumerable<IAtualizavel> BlocosAtualizaveis => Blocos.OfType<IAtualizavel>();
		private IEnumerable<IMovel> BlocosMoveis => Blocos.OfType<IMovel>();
	} 
}