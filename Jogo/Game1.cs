using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Engine;
using Armazenamento;
using static Engine.Helper;
namespace Jogo
{
    public class Game1 : Game
    {

        Mapa Map { get; set; }
        readonly double VelocidadeTempo = 0.1;
        Desenhista Desenhista;
        bool Stop = true;
        Camera Camera;


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
            Map = MapasPrefeitos.GerarMapaTestFIsica();
            //Map.AdicionarBloco(Jogador = new Jogador((0,0), (Key.Up, Key.Down, Key.Left, Key.Right), this));
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = (int)(Map.Tamanho.x+ 2* Bloco.TamanhoPadrao);
            _graphics.PreferredBackBufferHeight = (int)(Map.Tamanho.y + 2 * Bloco.TamanhoPadrao);
            _graphics.ApplyChanges();
            Camera = new Camera(_graphics.GraphicsDevice.Viewport);

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
            Camera.Zoom += (float)Math.Pow(DeltaWheel , 3) / 100.0f; 
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
            if (Teclado.Apertando(Keys.Right))
                Map.AtualizarMapa(VelocidadeTempo);
            else if (Teclado.Apertando(Keys.Left))
                Map.AtualizarMapa(-VelocidadeTempo);
            else if (!Stop)
                Map.AtualizarMapa(1);

            base.Update(gameTime);
        } 
        protected override void Draw(GameTime gameTime)
        { 
            GraphicsDevice.Clear(Color.Black);
            Desenhista.Iniciar(Camera.Transform);

            foreach (IBloco Bloco in Map.Blocos)
                Desenhista.DesenharBloco(Bloco);

            Desenhista.Finalizar();
            base.Draw(gameTime);
        }
    }
}
