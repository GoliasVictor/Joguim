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

    public class Jogador : BlocoMovel,IJogador
    {
        public override EstiloBloco Estilo { get; set; }
        (Key Up, Key Down, Key Left, Key Right) KeyDirection;
        private bool Tangivel = true;
        public Mapa Map { get; set; }
        public Jogo Jogo { get; set; }
        public Jogador(Cord posicao, (Key Up, Key Down, Key Left, Key Right) KeyDirection, Jogo jogo, EstiloBloco? estilo = null) : base(posicao,TamanhoPadrao, TamanhoPadrao)
        {
            Estilo = estilo ?? new EstiloBloco(Color.Blue);
            Jogo = jogo;
            this.KeyDirection = KeyDirection;
        }
        public int DanoMax => 3;
        public int DanoRecebido{ get;protected set;}
        public void Dano(int n) 
        {
            if ((DanoRecebido += n) >= DanoMax)
            {
                Mover(Jogo.Map.Spawn);
                DanoRecebido = 0;
            }
        } 

        public override void Movimentar()
        {
            
            //Velocidade = default;
            base.Movimentar();
            if (Teclado.Apertando(KeyDirection.Up)) 
                Velocidade += Vetor.Cima*0.01;
            if (Teclado.Apertando(KeyDirection.Down)) 
                Velocidade += Vetor.Baixo * 0.01;
            if (Teclado.Apertando(KeyDirection.Left)) 
                Velocidade += Vetor.Esquerda * 0.01;
            if (Teclado.Apertando(KeyDirection.Right)) 
                Velocidade += Vetor.Direita * 0.01;
            if (Teclado.Apertou(Key.Q))
                Tangivel = !Tangivel; 
        }
        public override void AplicarForca(Vetor forca)
        {
            if (Tangivel)
                base.AplicarForca(forca);
        }
 

    }
    public static class Teclado
    {

        public static KeyboardState EstadoTeclado;
        public static KeyboardState EstadoAnteriorTeclado;
        public static bool Apertando(Key Key) => EstadoTeclado[Key];
        public static bool Apertou(Key Key) => EstadoTeclado[Key] && !EstadoAnteriorTeclado[Key];
        public static void Atualizar( )
        {
            EstadoAnteriorTeclado = EstadoTeclado;
            EstadoTeclado = Keyboard.GetState() ;
        }
        public static void Atualizar(object sender, KeyboardKeyEventArgs e)
        {
            Atualizar();
        }
        public static void Atualizar(object sender, KeyPressEventArgs e)
        {
            Atualizar();
        }

    }
    public class Jogo : GameWindow
    {

        public Mapa Map { get; set; }
        public static MouseState EstadoMouse;
         float Zoom = 1;
         bool Stop = true;
         Vector3 Position;

        //Jogador Jogador;
        Vector3 Centro => new Vector3(ClientSize.Width/2,ClientSize.Height/2,0);
        public Jogo(int TamX, int TamY) : base()
        {
             
            KeyDown += Teclado.Atualizar;
            KeyUp += Teclado.Atualizar;
            KeyPress += Teclado.Atualizar;
            GerarMapa();
        }



        public Jogador player ;

        public void GerarMapa()
        {
            Helper.Tempo = 0;

            Map = MapasPrefeitos.GerarMapaTestFIsica();
            //Map.AdicionarBloco(Jogador = new Jogador((0,0), (Key.Up, Key.Down, Key.Left, Key.Right), this));
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            //Console.WriteLine($"x:{(-Position.X +ClientSize.Width/2- e.X ) / Zoom}, y:{-(-Position.Y+ClientSize.Height/2- e.Y ) / Zoom}");
            if (Mouse.GetState().IsButtonDown(MouseButton.Middle))
            {
                Position = Position + new Vector3(e.XDelta,-e.YDelta,0) / Zoom;
            } 
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            Console.WriteLine($"Wheeel:{e.Delta}");
            var PlusZoom = (float)Math.Pow(e.Delta, 5) / 100.0f;
            if(PlusZoom + Zoom< 10 && PlusZoom + Zoom > 0.01)
                Zoom += PlusZoom;
        }
        public float ax = 0, ay = 0;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ClientSize = new Size((int)Map.Tamanho.x, (int)Map.Tamanho.y); 
        }
        readonly int VelocidadeTempo = 1;
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            EstadoMouse = Mouse.GetState(); 
            //if (Helper.Tempo > 150) GerarMapa();
            if (Teclado.Apertou(Key.Escape)) Exit();

            if (Teclado.Apertou(Key.Space)) Stop = !Stop;
            if (Teclado.Apertando(Key.Right))
                Map.AtualizarMapa(VelocidadeTempo);
            else if (Teclado.Apertando(Key.Left))
                Map.AtualizarMapa(-VelocidadeTempo);
            else if (!Stop)
                Map.AtualizarMapa(); 
        }
        protected override void OnRenderFrame(FrameEventArgs e) 
        {
            
            base.OnRenderFrame(e);
                //Console.WriteLine($"Zoom:{Zoom}, Position:{Position}, Position*Zoom:{Position*Zoom}");
 
            GL.MatrixMode(MatrixMode.Projection);
            GL.Viewport(new Point(0, 0), ClientSize);
            GL.LoadIdentity();
            GL.Ortho(-Centro.X-Bloco.TamanhoPadrao,Centro.X+Bloco.TamanhoPadrao, -Centro.Y- Bloco.TamanhoPadrao, Centro.Y+ Bloco.TamanhoPadrao, 0, 1);
            GL.Scale(Zoom, Zoom, 1); 
            GL.Translate(Position );
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Tela.DesenharRetangulo(-Centro.X,-Centro.Y,ClientSize.Width,ClientSize.Height , Color.Green);

            foreach ( IBloco Bloco in  Map.Blocos)
                Tela.DesenharRetangulo(Bloco); 

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
                game.Run(60,120);
            }
        }
    }
}