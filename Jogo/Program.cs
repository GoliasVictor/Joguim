using Armazenamento;
using System;

namespace Jogo
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            //var Map = MapasPrefeitos.GerarMapaParticulas();
            //var KeyBoard = new Microsoft.Xna.Framework.Input.KeyboardState();
            //var Mouse = new Microsoft.Xna.Framework.Input.MouseState();
            //for (int i = 0; i < 1000000; i++)
            //    Map.AtualizarMapa(KeyBoard,Mouse);
            using (var game = new Game1())
                game.Run();
        }
    }
}
