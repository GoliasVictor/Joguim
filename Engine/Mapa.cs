using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using static Engine.Helper;
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
  
        public void AdicionarBloco(IBloco bloco)
        {
            Blocos.Add(bloco);
        }
        public void AdicionarBlocos(IEnumerable<IBloco> blocos)
        {
            foreach (IBloco Bloco in blocos)
                Blocos.Add(Bloco);
        }
        private void ColidirBlocos(IMovel BlocoMovel)
        {
            if (ReferenceEquals(BlocoMovel, Armazenamento.MapasPrefeitos.Zezinho)) { };
            foreach (var OutroBloco in Blocos)
                if (!ReferenceEquals(BlocoMovel, OutroBloco))
                    Colidir(BlocoMovel, OutroBloco);
        }
        private static bool Colidindo(IBloco A, IBloco B)
        {
            return A.ProximaDireita > B.ProximaEsquerda && A.ProximaCima < B.ProximaBaixo
                && B.ProximaDireita > A.ProximaEsquerda && B.ProximaCima < A.ProximaBaixo;
        }
        private static bool EstaDentro(IBloco B, Cord C)
        {
            return B.ProximaDireita > C.x && B.ProximaCima < C.y && C.x > B.ProximaEsquerda && C.y < B.ProximaBaixo;
        }
        public void AplicarForcaBloco(Cord cord, Vetor Forca)
        {
            var Bloco =  GetBlocosMoveis().Where((BlocoMovel) => EstaDentro(BlocoMovel,cord)).FirstOrDefault();
            Bloco.AplicarForca(Forca);
        }
        private static void Colidir(IBloco A, IBloco B)
        {  
            B.Colidir(A);
            A.Colidir(B);
        }
        private bool Colidindo(IBloco bloco)
        {
            foreach (var OutroBloco in Blocos)
                if (!ReferenceEquals(bloco, OutroBloco))
                    if (Colidindo(bloco, OutroBloco))
                        return true;
            return false;
        }
        private void Colidir(IBloco bloco)
        {
            if (bloco is null)
                throw new ArgumentNullException(nameof(bloco));
            foreach (var OutroBloco in Blocos)
                if(!ReferenceEquals(bloco,OutroBloco) && Colidindo(bloco, OutroBloco))
                    Colidir(bloco, OutroBloco);
        }
        readonly double Steps = 1;
        public void AtualizarMapa(double VelocidadeTempo = 1)
        {
            St.Start();
            for (int T = 0; T < Steps; T++)
            {
                Tempo += VelocidadeTempo * (1 / Steps);
                IEnumerable<IMovel> BlocosMoveis = GetBlocosMoveis();

                Parallel.ForEach(BlocosMoveis, (IMovel BlocoMovel) =>
                {
                    if (ReferenceEquals(BlocoMovel, Armazenamento.MapasPrefeitos.Zezinho)) { };
                    BlocoMovel.Movimentar();
                });
                foreach(var Bloco in BlocosMoveis)
                    Colidir(Bloco); 
                //for (int i = 0; i < 10; i++)
                //{
                //    IEnumerable<IBloco> BlocosMovidos = from Bloco in Blocos where Bloco.Posicao != Bloco.Posicao && Colidindo(Bloco) select Bloco;
                //    if (BlocosMovidos.Count() == 0)
                //        break;
                //    Parallel.ForEach(BlocosMovidos, Colidir);
                //}
                foreach (IMovel BlocoMovel in GetBlocosMoveis().Where((M) => M.ProximaPos != M.Posicao))
                    BlocoMovel.AtualizarPos();
                
            }
            St.Stop();
            //Console.WriteLine(St.ElapsedTicks);
            St.Restart();
            //Vetor VelocidadeTotal = default;
            //foreach (IMovel movel in GetBlocosMoveis())
            //    VelocidadeTotal += movel.Velocidade;
            //Console.WriteLine(VelocidadeTotal + ParedeVelocidade);
        }

        private IEnumerable<IMovel> GetBlocosMoveis()
        {
            return Blocos.OfType<IMovel>();
        }
    }
}