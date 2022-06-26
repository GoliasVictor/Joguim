using System;
using System.Drawing;
using static Engine.Helper;

namespace Engine
{
    [Serializable]
    public struct Estilo : IEstilo
    {
        public Color Cor { get; set; }
        public Estilo(Color color) => Cor = color;
        public static Color Aleatorio() => Color.FromArgb(Rnd.Next(255), Rnd.Next(255), Rnd.Next(255));

        public static Estilo parede = new Estilo(Color.FromArgb(101, 67, 33));
        public static Estilo Chao = new Estilo(Color.Green);
        public static Estilo Morte = new Estilo(Color.Firebrick);
        public static Estilo Ponto = new Estilo(Color.Cyan);
        public static Estilo Teletransporte = new Estilo(Color.Cyan);
        public static Estilo Player = new Estilo(Color.Blue);
        public static Estilo Botao = new Estilo(Color.Gray);
        public static Estilo Porta = new Estilo(Color.SaddleBrown);
        public static Estilo Empuravel = new Estilo(Color.Brown);
        public static implicit operator Estilo(Color c) => new Estilo(c);
    }
}
