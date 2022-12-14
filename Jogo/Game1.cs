using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Engine;
using static Engine.Helper;
using System.Linq;

namespace Jogo
{
    class RectangleSprite
    {
        static Texture2D _pointTexture;
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[]{Color.White});
            }

            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }     
    }

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
            Map = MapasPrefeitos.GerarMapaParticulas();
            
            //Map.AdicionarEntidade(new Jogador((0,0), estilo: Estilo.Teletransporte));
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
            //if (Map.Entidades.Any((e) => !SistemaColisao.Colidindo(Map.PosicaoLados, e.Lados)))
            //    Stop = true;
            if (Teclado.Apertou(Keys.Space))
                Stop = !Stop;
            else if (!Stop){
                var Inputs = new Inputs(Keyboard.GetState(),Mouse.GetState());
                if (Teclado.Apertando(Keys.T) && Teclado.Apertando(Keys.Left))
                    Map.AtualizarMapa(Inputs, -VelocidadeTempo);
                else if (Teclado.Apertando(Keys.T) && Teclado.Apertando(Keys.Right))
                    Map.AtualizarMapa(Inputs, 2*VelocidadeTempo);
                else
                    Map.AtualizarMapa(Inputs,VelocidadeTempo);
            }else
            {
                var Inputs = new Inputs(Keyboard.GetState(), Mouse.GetState());
                if (Teclado.Apertou(Keys.Left))
                    Map.AtualizarMapa(Inputs, -VelocidadeTempo);
                else if (Teclado.Apertou(Keys.Right))
                    Map.AtualizarMapa(Inputs, VelocidadeTempo);
            }

            base.Update(gameTime);
        } 
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Background);
            Desenhista.Iniciar(Camera.Transform); 

            foreach (IEntidade entidade in Map.Entidades)
                Desenhista.Desenhar(entidade);
            foreach(var chunk in SistemaColisao.GerarChunks(Map.Entidades.OfType<IColisivel>())){
                Desenhista.Desenhar(chunk, (double)2/Camera.Zoom);
            }

            Desenhista.Finalizar();
            base.Draw(gameTime);
        }
    }
}
