using static Engine.Helper;
using Microsoft.Xna.Framework;
using Engine;
namespace Jogo
{
    interface IEstilizavel{
		Estilo Estilo {get;}
	}	
	public struct Estilo : IEstilo
    {
        public Color Cor { get; set; }
        public Estilo(Color color) => Cor = color;
        public static Color Aleatorio() => new Color(Rnd.Next(255), Rnd.Next(255), Rnd.Next(255));

        public static Estilo parede = new Estilo(new Color(101, 67, 33));
        public static Estilo Morte = new Estilo(Color.Firebrick);
        public static Estilo Teletransporte = new Estilo(Color.Cyan);
        public static Estilo Botao = new Estilo(Color.Gray);
        public static implicit operator Estilo(Color c) => new Estilo(c);
    }
    
}
