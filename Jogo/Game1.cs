using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Engine;
using static Engine.Helper;
namespace Jogo
{
    public class Game1 : Game
    {

        Mapa Map { get; set; }
        const double VelocidadeTempo = 1;
        Desenhista Desenhista;
        bool Stop = true;
        Camera Camera;
        Color Background ;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1() : base()
        {

            GerarMapa();
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        public void GerarMapa()
        {
            Tempo = 0; 
            Map = MapasPrefeitos.GerarMapaDeTeste1();
            
           // Map.AdicionarEntidade(Jogador = new Jogador((50,50), estilo: Estilo.Player));
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = (int)(Map.Tamanho.Largura*Map.PixelPorUnidade);
            _graphics.PreferredBackBufferHeight = (int)(Map.Tamanho.Altura *Map.PixelPorUnidade);
            _graphics.ApplyChanges();
            Camera = new Camera(_graphics.GraphicsDevice.Viewport);
            Camera.Zoom = (float)(Map.PixelPorUnidade);
            var bg = Map.Estilo  is null ? default : ((Estilo)Map.Estilo).Cor;
            Background =  new Color(bg.R,bg.G, bg.B);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Desenhista = new Desenhista(_spriteBatch, GraphicsDevice); 
        }
        protected void AtualizarZoom()
        {
            var DeltaWheel = ControladorMouse.DeltaWheelValue / 100f;
            Camera.Zoom *= (float)Math.Pow(1.1,DeltaWheel); 
        }
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Teclado.Atualizar();
            ControladorMouse.Atualizar();

            
            AtualizarZoom();
            if (Teclado.Apertou(Keys.Space))
                Stop = !Stop;
            else if (!Stop){
                var Inputs = new Inputs(Keyboard.GetState(),Mouse.GetState());

                if ( Teclado.Apertando(Keys.T) && Teclado.Apertando(Keys.Left))
                    Map.AtualizarMapa(Inputs,-VelocidadeTempo);
                else
                    Map.AtualizarMapa(Inputs,VelocidadeTempo);
            }

            base.Update(gameTime);
        } 
        protected override void Draw(GameTime gameTime)
        { 

            GraphicsDevice.Clear(Background);
            Desenhista.Iniciar(Camera.Transform); 

            foreach (IEntidade entidade in Map.Entidades)
                Desenhista.Desenhar(entidade);

            Desenhista.Finalizar();
            base.Draw(gameTime);
        }
    }
}
