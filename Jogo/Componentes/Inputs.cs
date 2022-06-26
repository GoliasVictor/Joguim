using Engine;
using Microsoft.Xna.Framework.Input;

namespace Jogo
{
	public class Inputs : IComponente, IInputs
	{
		public KeyboardState Teclado { get; init; }
		public MouseState Mouse { get; init; }
		public Inputs(KeyboardState novoEstadoTeclado, MouseState novoEstadoMouse)
		{
			Teclado = novoEstadoTeclado;
			Mouse = novoEstadoMouse;
		}
		public bool EstaPresionada(Keys key)
		{
			return Teclado.IsKeyDown(key);
		}
	}

}