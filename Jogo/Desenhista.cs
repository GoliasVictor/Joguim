using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jogo
{
    class Desenhista
    {
        private SpriteBatch SpriteBatch;
        private GraphicsDevice GraphicsDevice;
        public Texture2D WhiteTexture { get; protected set; }
        public bool Desenhando { get; private set; }
        public Desenhista(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice )
        {
            SpriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            GraphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
            WhiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            WhiteTexture.SetData(new Color[] { Color.White });
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
 

          
            var Cor = ((IEstilizavel)entidade).Estilo.Cor;
            
            var Pos = new Vector2(
                (float)(entidade.Pos.x - entidade.Tam.Largura / 2),
                (float)(entidade.Pos.y - entidade.Tam.Altura / 2));

            var Tam = new Point((int)entidade.Tam.Largura , (int)entidade.Tam.Altura);
            SpriteBatch.Draw(WhiteTexture, Pos,new Rectangle(new Point(0,0),Tam), new Color(Cor.R, Cor.G, Cor.B));

        }

        public void Desenhar(Chunk chunk)
        {
                var tam = new Point(
                    (int)(Math.Abs(chunk.Lados.Esquerda - chunk.Lados.Direita))-1,
                    (int)(Math.Abs(chunk.Lados.Cima - chunk.Lados.Baixo))-1
                );
                var pos =  new Point((int)chunk.Lados.Esquerda,(int)chunk.Lados.Cima);
                Color cor  = new Color();
                var Intensidade = ((double)chunk.Entidades.Count())/(double)SistemaColisao.QtMaxEntidadesChunks;
                cor = new Color(255 , (float)(255*(1-Intensidade)), (float)(255*(1-Intensidade)));
                RectangleSprite.DrawRectangle(SpriteBatch, new Rectangle(pos,tam), cor,2);        

        }

  
    }
}
