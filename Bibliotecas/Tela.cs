using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;

namespace Biblioteca
{
    public static class Tela
    {
        public static Random Rand = new Random();

        public static int TamPadrao = 20;
        public static GameWindow window;
        public static void DesenharRetangulo(float x, float y, float Largura, float Altura, Color cor)
        { 

            GL.Color3(cor);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(x, -y);
            GL.Vertex2(x+Largura, -y);
            GL.Vertex2(x+Largura, -(y+Altura));
            GL.Vertex2(x, -(y+Altura));
            GL.End(); 
        }

        public static void DesenharRetangulo(IBloco Bloco)
        {

            if (ReferenceEquals(Bloco, Armazenamento.MapasPrefeitos.Zezinho)){ };
            GL.Color3(Bloco.Estilo.Cor);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(Bloco.Esquerda, Bloco.Cima );
            GL.Vertex2(Bloco.Direita , Bloco.Cima );
            GL.Vertex2(Bloco.Direita , Bloco.Baixo);
            GL.Vertex2(Bloco.Esquerda, Bloco.Baixo);
            GL.End();
            //if (Bloco is IMovel movel)
            //{
            //    GL.Color3(Color.White);
            //    GL.Begin(PrimitiveType.Lines);
            //    GL.Vertex2(movel.Posicao.x, movel.Posicao.y);
            //    GL.Vertex2(movel.Posicao.x  + movel.ProximaVelocidade.x*10, movel.Posicao.y + movel.ProximaVelocidade.y*10);
            //    GL.End();
            //}
        }
    }

}