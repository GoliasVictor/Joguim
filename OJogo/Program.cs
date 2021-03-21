using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Armazenamento;
using Biblioteca;


using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
namespace OJogo
{

    public class Jogador : IJogador
    {
        public EstiloBloco Estilo { get => Tangivel ? new EstiloBloco(Color.Blue) : new EstiloBloco(Color.White); set {; } }
        public bool Tangivel => tangivel;
        public bool tangivel { get; set; }
        public Cord Pos { get; set; }
        public Cord ProximaPos { get; set; }
        public Mapa Map { get; set; }
        public Jogador(Mapa map) { Map = map; }
        public int DanoMax => 3;
        public int DanoRecebido{ get;protected set;}
        public void Dano(int n) 
        {
            if ((DanoRecebido += n) >= DanoMax)
            {
                ProximaPos = Map.Spawn.GetCord();
                DanoRecebido = 0;
            }
        }
        
        public  void Movimento()
        {
            if (Jogo.EstadoTeclado[Key.Up] || Jogo.EstadoTeclado[Key.Down] || Jogo.EstadoTeclado[Key.Left] || Jogo.EstadoTeclado[Key.Right] || Jogo.EstadoTeclado[Key.Q])
            {
                
                if (Jogo.EstadoTeclado[Key.Q] && Jogo.EstadoTeclado[Key.Q] != Jogo.EstadoAnteriorTeclado[Key.Q])
                    tangivel = !tangivel;
                ProximaPos = Pos.GetCord();
                if (Jogo.EstadoTeclado[Key.Up]) ProximaPos.y -= 1;
                else if (Jogo.EstadoTeclado[Key.Down]) ProximaPos.y += 1;
                else if (Jogo.EstadoTeclado[Key.Left]) ProximaPos.x -= 1;
                else if (Jogo.EstadoTeclado[Key.Right]) ProximaPos.x += 1;

                if (tangivel)
                {
                    Map.Interagir(this);
                    if (Map.PosTangivel(ProximaPos))
                        ProximaPos = Pos.GetCord();
                }

            }
        }

        public void AtualizarPos() => Pos = ProximaPos.GetCord();

    }
    public class Jogo : GameWindow
    {

        public Mapa Map { get; set; }
        public static KeyboardState EstadoTeclado;
        public static KeyboardState EstadoAnteriorTeclado;

        public Jogo(int TamX, int TamY) : base()
        {
            Map = MapasPrefeitos.GerarMapaDeTeste1();
            player = new Jogador(Map);
            player.Pos = Map.Spawn.GetCord();
            player.ProximaPos = Map.Spawn.GetCord();
            Map.Especiais.Insert(0, player);


        }

        public Jogador player ;
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
             EstadoTeclado = Keyboard.GetState();
            foreach (var pixelMovel in Map.Especiais)
                pixelMovel.Movimento();
            foreach (var pixelMovel in Map.Especiais)
                pixelMovel.AtualizarPos();
            if (Keyboard.GetState()[Key.Escape]) Exit();
            EstadoAnteriorTeclado = EstadoTeclado;

        }
        public float ax = 0, ay = 0;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ClientSize = new Size(Map.Tamanho.x * Tela.TamPadrao, +Map.Tamanho.y * Tela.TamPadrao);

        }
        
        protected override void OnRenderFrame(FrameEventArgs e) 
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);
            Matrix4 projection = Matrix4.CreateOrthographic(ClientSize.Width, ClientSize.Height, 0.0f, 1.0f) * Matrix4.CreateTranslation(-1f, +1f, 0);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            for (int x = 0; x < Map.Tamanho.x; x++)
                for (int y = 0; y < Map.Tamanho.y; y++)
                    Tela.DesenharRetangulo(x, y, Map[x, y].Estilo.Cor);
            
            foreach(var pixelMovel in Map.Especiais )            
                Tela.DesenharRetangulo(pixelMovel);


            SwapBuffers();
        }

    }

    internal class Program
    {
        private static void Main()
        {
            Console.Write(System.IO.Directory.GetCurrentDirectory());
            using (var game = new Jogo(21, 21))
            {
                game.VSync = VSyncMode.Adaptive;
                Tela.window = game;
                game.WindowBorder = WindowBorder.Fixed;
                game.Run(10.0, 0.0);
            }
        }
    }
}