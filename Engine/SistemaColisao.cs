using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engine
{
	public struct Chunk {
		public Chunk(PosicaoLados posicaoLados)
		{
			Lados = posicaoLados;
			Entidades = new();
            DeltaQtCriada++;

        }
        public static int QtCriada = 0;
		public static int DeltaQtCriada = 0;

		public Chunk(PosicaoLados lados, List<WraperColisao> entidades) : this(lados)
		{
			Entidades = entidades;
            DeltaQtCriada++;

        }

		public PosicaoLados Lados {get;init;}
		public List<WraperColisao> Entidades {get;init;}

	}
    public struct WraperColisao
    {
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
    public class SistemaColisao
	{

		public const int QtMaxEntidadesChunks = 37;
		

		record struct ParColisao ( IColisivel A, IColisivel B){};
        public static IEnumerable<Chunk> GerarChunks(IEnumerable<IColisivel> entidades)
        {
            return GerarChunks(new Chunk(new PosicaoLados(
                   esquerda: entidades.Select((e) => e.Lados.Esquerda).Min(),
                   direita: entidades.Select((e) => e.Lados.Direita).Max(),
                   cima: entidades.Select((e) => e.Lados.Cima).Min(),
                   baixo: entidades.Select((e) => e.Lados.Baixo).Max()
             ), entidades.Select(e=> new WraperColisao(e)).ToList()));
        }
        public static int deltaQtVer = 0;
        public static int qtVer = 0;

		static IEnumerable<Chunk> GerarChunks(Chunk chunkInicial)
		{
            if (chunkInicial.Entidades is null)
                throw new ArgumentNullException(nameof(chunkInicial));
            List<Chunk> subChunks = new();
			Stack<Chunk> chunks = new Stack<Chunk>();
			chunks.Push(chunkInicial);
			while (chunks.Count > 0)
			{
				chunks.TryPop(out Chunk chunk);


				var Lados = chunk.Lados;
				var largura = Math.Abs(Lados.Esquerda - Lados.Direita);
				var altura = Math.Abs(Lados.Baixo - Lados.Cima);

				const int QT_DIVISOES = 2;

				var larguraChunk = largura / QT_DIVISOES;
				var alturaChunk = altura / QT_DIVISOES;
				if (largura == 0 || altura == 0)
					continue;

                for (double y = Lados.Cima; y <= largura + alturaChunk; y += alturaChunk)
				{
					for (double x = Lados.Esquerda; x <= altura + larguraChunk; x += larguraChunk)
					{

						var subChunk = new Chunk(new PosicaoLados(
							esquerda: x,
							direita: x + larguraChunk,
							cima: y,
							baixo: y + alturaChunk
							)
						);

						foreach (var entidade in chunk.Entidades)
							if (Colidindo(subChunk.Lados, entidade.Lados))
								subChunk.Entidades.Add(entidade);
						if (subChunk.Entidades.Count < 2)
							continue;

						//subChunk.Lados = new PosicaoLados(
						//	   esquerda: Math.Max(subChunk.Lados.Esquerda,subChunk.Entidades.Select((e) => e.Lados.Esquerda).Min()),
						//	   direita: Math.Min(subChunk.Lados.Direita, subChunk.Entidades.Select((e) => e.Lados.Direita).Max()),
						//	   cima: Math.Max(subChunk.Lados.Cima, subChunk.Entidades.Select((e) => e.Lados.Cima).Min()),
						//	   baixo: Math.Min(subChunk.Lados.Baixo, subChunk.Entidades.Select((e) => e.Lados.Baixo).Max())
						//);


						if (subChunk.Entidades.Count() > QtMaxEntidadesChunks)
							chunks.Push(subChunk);
						else
							subChunks.Add(subChunk);
					}
				}

			}


			return subChunks;

		}
		static IEnumerable<ParColisao> GerarPares(IEnumerable<IColisivel> entidades){
			var Chunks = GerarChunks(entidades);
			List<ParColisao> pares =  new();
			foreach (var chunk in Chunks)
			{
				
				for (int i = 0; i < chunk.Entidades.Count; i++)
				{
					WraperColisao A = chunk.Entidades[i];
					for (int j = i+1; j < chunk.Entidades.Count; j++)
					{
						WraperColisao B = chunk.Entidades[j];
						if (A.Estatico && B.Estatico)
							continue;

						var EstaColidindo = Colidindo(A.Lados, B.Lados);
						if (EstaColidindo)
							pares.Add(new ParColisao(A.Entidade, B.Entidade));
					}
				}
			}
			return  new HashSet<ParColisao>(pares);
		}
		public static void Colidir(IEnumerable<IColisivel> entidades, double DeltaT)
		{
            deltaQtVer = 0;
			Chunk.DeltaQtCriada = 0;
            if (entidades is null)
				throw new ArgumentNullException(nameof(entidades));
			var pares = GerarPares(entidades);
			foreach(var par in pares){
				Colidir(par.A, par.B);
			}
			qtVer += deltaQtVer;
			Chunk.QtCriada += Chunk.DeltaQtCriada;

        }
		public static bool Colidindo(PosicaoLados A, PosicaoLados B)
        {
            deltaQtVer++;
            return A.Direita > B.Esquerda && A.Cima < B.Baixo
                && B.Direita > A.Esquerda && B.Cima < A.Baixo;
        } 
        static void Colidir(IColisivel A, IColisivel B)
        {  
            B.Colidir(A);
            A.Colidir(B);
        }
	}
}
/*
\\ t = cm
\\ T_q = mc^2 + \frac{1}{2}(m^2-m)
\\ T_q = tc + \frac{1}{2}(\frac{t^2}{c^2} -\frac{t}{c})
\\ T_q = \frac{t^2}{m} + \frac{1}{2}(m^2-m)
\\ 
\\T_q \in \Theta(c^2m + m^2) 
\\T_q \in \Theta(tc + \frac{t^2}{c^2}) = \Theta(t(c + \frac{t}{c^2})) 
\\T_q \in \Theta(\frac{t^2}{m} + m^2)
*/ 