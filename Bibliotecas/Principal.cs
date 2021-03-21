using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Input;

namespace Biblioteca
{


[Serializable]
    public class Cord
    {
        public int x { get; set; }
        public int y { get; set; }
        public Cord(int cx, int cy)
        {
            x = cx;
            y = cy;
        }
        public Cord GetCord() => (Cord)MemberwiseClone();
        public override string ToString() => $"{{x:{x}, y:{y}}}";

        public static bool operator ==(Cord a, Cord b) => a is null? (b is null ? true : false): a.x == b.x && a.y == b.y;
        public static bool operator !=(Cord a, Cord b) => !(a == b);
        public static implicit operator Cord((int x,int y) p) => new Cord(p.x,p.y);


        public override bool Equals(Object obj)
        {
            if (obj is Point)
            {
                Cord p = (Cord)obj;
                return x == p.x & y == p.y;
            }
            else return false;
            
        }

        public override int GetHashCode()=> (x, y).GetHashCode();
        

        public Cord() { }
    }

    [Serializable]
    public class EstiloBloco
    {
        public Color Cor { get; set; }
        public EstiloBloco() { }
        public EstiloBloco(Color color) =>  Cor = Color.FromArgb(127, color);
        public static implicit operator EstiloBloco(Color c) => new EstiloBloco(c);

        public static EstiloBloco parede  = new EstiloBloco(Color.FromArgb(101, 67, 33));
        public static EstiloBloco Chao = new EstiloBloco(Color.Green);
        public static EstiloBloco Morte = new EstiloBloco(Color.Firebrick);
        public static EstiloBloco Ponto = new EstiloBloco(Color.Cyan);
        public static EstiloBloco Teletransporte = new EstiloBloco(Color.Cyan);
        public static EstiloBloco Player = new EstiloBloco(Color.Blue);
        public static EstiloBloco Botao = new EstiloBloco(Color.Gray);
        public static EstiloBloco Porta = new EstiloBloco(Color.SaddleBrown);
    }
    #region Tipos De Blocos
    public interface Bloco
    {
         bool Tangivel { get; }
         EstiloBloco Estilo { get;  set; }
    }
    public interface IMovel:Bloco
    {
        Cord Pos { get; set; }
        Cord ProximaPos { get; set; }
        void Movimento();
        void AtualizarPos();
    }
    public interface IInteragivel:Bloco
    {
        void Interagir( IMovel e);
    }
    public interface IReceptor
    {
        void Receber(object e);
    }
    public interface IJogador:IMovel
    {
        Mapa Map { get; set; }
        void Dano(int n);
    }


    #endregion

    
    [Serializable]
    public class Mapa
    {
        public readonly (int x, int y) Tamanho;
        public Cord Spawn { get; set; } = (1, 1);
        public Bloco[,] Grid { get; set; }
        public List<IMovel> Especiais{ get; set; } = new List<IMovel>();
        

        public Bloco this[int i, int j]
        {
            get => Grid[i, j];
            set => Grid[i, j] = value;
        }
        public Bloco this[Cord cord]
        {
            get => Grid[cord.x, cord.y];
            set => Grid[cord.x, cord.y] = value;
        }
        public Mapa() { }
        public Mapa(int TamX, int TamY)
        {

            Tamanho = (TamX, TamY);
            Grid = new Bloco[TamX, TamY];
            for (int x = 0; x < TamX; x++)
                for (int y = 0; y < TamX; y++)
                    Grid[x, y] = new Chao();

            for (int x = 0; x < TamX; x++)
            {
                Grid[x, TamX - 1] = new Parede();
                Grid[x, 0] = new Parede();
            }

            for (int y = 0; y < TamX; y++)
            {
                Grid[TamX - 1, y] = new Parede();
                Grid[0, y] = new Parede();
            }

        }
        public void Interagir( IMovel movel )
        {
            if (this[movel.ProximaPos] is IInteragivel)
                ((IInteragivel)this[movel.ProximaPos]).Interagir(movel);
            foreach (var BlocoEspecial in Especiais)
                if (BlocoEspecial.Pos == movel.ProximaPos && BlocoEspecial is IInteragivel && !ReferenceEquals(movel, BlocoEspecial)) 
                        ((IInteragivel)BlocoEspecial).Interagir(movel);
        }
        public bool PosTangivel(Cord cord) => this[cord].Tangivel || EspecialTangivel(cord);
        public void PintarGrid(Cord Pos, Bloco bloco)
        {
            Grid[Pos.x, Pos.y] = bloco;
        }
        public bool EspecialTangivel(Cord Pos)
        {
            foreach(var b in Especiais)
                if ((b.Pos == Pos || b.ProximaPos == Pos) && b.Tangivel && !ReferenceEquals(b.ProximaPos, Pos))
                    return true;
            return false;
        }
    }
    public static class Tela
    {
        public static Random Rand = new Random();

        public static int TamPadrao = 20;
        public static GameWindow window;
        public static void DesenharRetangulo(int x, int y, Color cor)
        {
            x = (x * +TamPadrao ) ;
            y = (y * -TamPadrao );
           
            GL.Color3( cor);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(x, y);
            GL.Vertex2(TamPadrao + x, y);
            GL.Vertex2(TamPadrao + x, -TamPadrao + y);
            GL.Vertex2(x, -TamPadrao + y);
            //GL.Vertex2(x - TamPadrao / 2, y - TamPadrao / 2);
            //GL.Vertex2(x + TamPadrao / 2, y - TamPadrao / 2);
            //GL.Vertex2(x + TamPadrao / 2, y + TamPadrao / 2);
            //GL.Vertex2(x - TamPadrao / 2, y + TamPadrao / 2);
            GL.End();
        }
        public static void DesenharRetangulo(IMovel movel)
        {
            var x = (movel.Pos.x * +TamPadrao);
            var y = (movel.Pos.y * -TamPadrao);
            GL.Color3(movel.Estilo.Cor);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(x, y);
            GL.Vertex2(TamPadrao + x, y);
            GL.Vertex2(TamPadrao + x, -TamPadrao + y);
            GL.Vertex2(x, -TamPadrao + y);
            GL.End();
        }
    }

    public static class Pintor
    {
        public static Mapa Map;
        public static  void Ponto(Cord A, Bloco bloco) => Map.PintarGrid(A, bloco);
        public static  void Linha(Cord A, Cord B, Bloco bloco)
        {
            double Distancia = Math.Sqrt( Math.Pow(A.x-B.x,2) + Math.Pow(A.y - B.y, 2));
            double cox = (B.x - A.x) / Distancia;
            double coy = (B.y - A.y) / Distancia;
            Cord aux = A.GetCord();
            Map.PintarGrid(A, bloco);
            Map.PintarGrid(B, bloco);
            for (int i = 1; aux.x != B.x || aux.y != B.y; i++)
            {
                aux.x = A.x + (int)(i * cox);
                aux.y = A.y + (int)(i * coy);
                Map.PintarGrid(aux, bloco);
            }
        }
       public  static  void Retangulo(Cord A, Cord B, Bloco bloco, bool Prenchido = false)
        {
            Cord Men = (
                  A.x < B.x ? A.x : B.y,
                  A.y < B.y ? A.y : B.y
                );
            Cord Mai = (
                  A.x > B.x ? A.x : B.y, 
                  A.y > B.y ? A.y : B.y
                );
            if (Prenchido)
            {
                for (int x = Men.x; x < Mai.x; x++)
                    for (int y = Men.y; y < Mai.y; y++)
                        Map.PintarGrid((x, y), bloco);
            }
            else
            {
                for (int x = Men.x; x <= Mai.x; x++)
                {
                    Map.PintarGrid((x, A.y), bloco);
                    Map.PintarGrid((x, B.y), bloco);
                }
                for (int y = Men.y; y <= Mai.y; y++)
                {
                    Map.PintarGrid((A.x, y), bloco);
                    Map.PintarGrid((B.x, y), bloco);
                }
            }
        }
    }


}