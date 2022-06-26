using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engine
{
	class SistemaColisao
	{
		struct WraperColisao
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
		public static void Colidir(IEnumerable<IColisivel> entidades, double DeltaT)
		{
			if (entidades is null)
				throw new ArgumentNullException(nameof(entidades));
			var EntidadesWrapers = entidades.Select(e => new WraperColisao(e)).ToArray();
			for (int i = 0; i < EntidadesWrapers.Length; i++)
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