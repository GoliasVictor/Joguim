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
		}

		public PosicaoLados Lados {get;set;}
		public List<SistemaColisao.WraperColisao> Entidades {get;set;}

	}
	public class SistemaColisao
	{

		public const int QtMaxEntidadesChunks = 10;
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

		record struct ParColisao ( IColisivel A, IColisivel B){};

		public static IEnumerable<Chunk> GerarChunks(IEnumerable <WraperColisao> entidades, PosicaoLados? posLados = null){
			if (entidades is null)
				throw new ArgumentNullException(nameof(entidades));
			var Lados =	 posLados ?? new PosicaoLados(
				esquerda:  entidades.Select((e)=> e.Lados.Esquerda).Min(),
				direita :  entidades.Select((e)=> e.Lados.Direita).Max(),
				cima 	:  entidades.Select((e)=> e.Lados.Cima).Min(),
				baixo 	:  entidades.Select((e)=> e.Lados.Baixo).Max()
			);
			var largura = Math.Abs(Lados.Esquerda - Lados.Direita);
			var altura = Math.Abs(Lados.Baixo - Lados.Cima);
			
			var qt = 2;

			var LarguraChunk =  largura / qt;
			var AlturaChunk =  altura / qt;
			var Chunks =  new List<Chunk>();
			for (int l = 0; l < qt; l++)
				for (int c = 0; c < qt; c++)
					Chunks.Add(new Chunk(new PosicaoLados(
						esquerda: Lados.Esquerda +  LarguraChunk * c,
						direita: Lados.Esquerda +  LarguraChunk * (c+1),
						cima : Lados.Cima + AlturaChunk * l,
						baixo: Lados.Cima + AlturaChunk * (l+1)
						)
					));

			foreach (var chunk in Chunks)
				foreach (var entidade in entidades)
					if(Colidindo(chunk.Lados, entidade.Lados))
						chunk.Entidades.Add(entidade);
			var Chunks2 =  new List<Chunk>();
			foreach (var chunk in Chunks)
			{
				var Count = chunk.Entidades.Count();
				if(Count == 0)
					continue; 
				if( Count > QtMaxEntidadesChunks)
					Chunks2.AddRange(GerarChunks(chunk.Entidades, chunk.Lados));
				else 
					Chunks2.Add(chunk);
			}
			return Chunks2;	
			
		}
		static IEnumerable<ParColisao> GerarPares(IEnumerable<WraperColisao> entidades){
			var Chunks = GerarChunks(entidades);
			List<ParColisao> pares =  new();
			foreach (var chunk in Chunks)
			{
				
				for (int i = 0; i < chunk.Entidades.Count(); i++)
				{
					WraperColisao A = chunk.Entidades[i];
					for (int j = i+1; j < chunk.Entidades.Count(); j++)
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
			return pares;
		}
		public static void Colidir(IEnumerable<IColisivel> entidades, double DeltaT)
		{
			if (entidades is null)
				throw new ArgumentNullException(nameof(entidades));
			var pares = GerarPares(entidades.Select(e=> new WraperColisao(e)));
			foreach(var par in pares){
				Colidir(par.A, par.B);
			}

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
	}
	static class SistemaColisaoParalela
	{
		static void ColidirParalelo(IEnumerable<IColisivel> entidades)
		{
			var Sublists = entidades.Select((B) => new List<IColisivel>() { B });
			ColidirEJuntar(new ConcurrentStack<List<IColisivel>>(Sublists));
		}
		static ConcurrentStack<List<IColisivel>> ColidirEJuntar(ConcurrentStack<List<IColisivel>> StackListsEntidade)
		{
			var ListasUnidas = new ConcurrentStack<List<IColisivel>>();
			List<Task> Tasks = new List<Task>();
			while (StackListsEntidade.Count > 1)
			{
				StackListsEntidade.TryPop(out var A);
				StackListsEntidade.TryPop(out var B);
				Tasks.Add(Task.Run(() =>
				{
					ListasUnidas.Push(ColidirEJuntarLists(A, B));
				}));
			}
			if (StackListsEntidade.Count == 1)
			{
				StackListsEntidade.TryPop(out var Lista);
				ListasUnidas.Push(Lista);
			}
			Task.WhenAll(Tasks).Wait();
			return ListasUnidas.Count < 2 ? ListasUnidas : ColidirEJuntar(ListasUnidas);
		}
		static List<IColisivel> ColidirEJuntarLists(List<IColisivel> entidades1, List<IColisivel> entidades2)
		{
			if (entidades2 is null)
				return entidades1;
			if (entidades1 is null)
				return entidades2;

			Parallel.ForEach(entidades1, (A) =>
			{
				foreach (var B in entidades2)
					if (Colidindo(A.Lados, B.Lados))
						Colidir(A, B);
			});
			return entidades1.Union(entidades2).ToList();
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
	}
}