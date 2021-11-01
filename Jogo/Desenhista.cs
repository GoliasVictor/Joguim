using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jogo
{
    class Desenhista
    {
        private SpriteBatch SpriteBatch;
        private GraphicsDevice GraphicsDevice;
         
        public bool Desenhando { get; private set; }

        public Desenhista(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            SpriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            GraphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
        }

        public void Iniciar(Matrix MatrixTransformacao)
        {
            SpriteBatch.Begin(transformMatrix: MatrixTransformacao);
            Desenhando = true;
        }
        public void Finalizar()
        {
            SpriteBatch.End();
            Desenhando = false;
        }
        public void Desenhar(IEntidade entidade)
        {
            if (!Desenhando)
                throw new Exception("Não Está Desenhando");
            Texture2D _texture;

            _texture = new Texture2D(GraphicsDevice, 1, 1);
            var AuxCor = entidade.Estilo.Cor;
            var Cor = new Color(AuxCor.R, AuxCor.G, AuxCor.B, AuxCor.A);
            _texture.SetData(new Color[] { Cor });
            var Pos = new Point(
                (int)(entidade.Pos.x - entidade.Tam.Largura / 2),
                (int)(entidade.Pos.y - entidade.Tam.Altura / 2));
            var Tam = new Point((int)entidade.Tam.Largura, (int)entidade.Tam.Altura);
            SpriteBatch.Draw(_texture, new Rectangle(Pos, Tam), Color.White);

        }
    }
}
