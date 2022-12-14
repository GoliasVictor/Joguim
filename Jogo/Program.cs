using Engine;
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
            //for (int i = 0; i < 10000; i++)
            //    Map.AtualizarMapa(new Inputs(KeyBoard, Mouse));
            //Console.WriteLine("Ver Colisoes: Total = "+SistemaColisao.qtVer);
            //Console.WriteLine("Ver Colisoes: Ultimo frame = "+ SistemaColisao.deltaQtVer);
            //Console.WriteLine("Chunks: Quantidade total gerada " + Chunk.QtCriada);
            //Console.WriteLine("Chunks: Quantidade gerada Ültimo:"+Chunk.DeltaQtCriada);
            using (var game = new Game1())
                game.Run();
        } 
    }
}
