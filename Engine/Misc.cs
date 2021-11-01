
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Drawing;
using static Engine.Helper;


namespace Engine
{
    public struct Vetor
    {
        public double x;
        public double y;

        public Vetor(double x, double y)
        {
            this.x = x;
            this.y = y;
            //this.x += (Rnd.NextDouble() - 0.5) * 0.00000001;
            //this.y += (Rnd.NextDouble() - 0.5) * 0.00000001;
        }
        public Vetor(Cord cord)
        {
            x = cord.x;
            y = cord.y;
        }
        public static readonly Vetor Zero = new Vetor(0, 0);
        public static readonly Vetor Esquerda = new Vetor(-1, 0);
        public static readonly Vetor Direita = new Vetor(1, 0);
        public static readonly Vetor Cima = new Vetor(0, 1);
        public static readonly Vetor Baixo = new Vetor(0, -1);
        public static Vetor operator *(Vetor A, double Escalar) => new Vetor(A.x * Escalar, A.y * Escalar);
        public static Vetor operator *(double Escalar,Vetor A) => new Vetor(A.x * Escalar, A.y * Escalar);
        public static Vetor operator /(Vetor A, double Escalar) => new Vetor(A.x / Escalar, A.y / Escalar);
        public static Vetor operator +(Vetor A, Vetor B) => new Vetor(A.x + B.x, A.y + B.y);
        public static Vetor operator -(Vetor A, Vetor B) => new Vetor(A.x - B.x, A.y - B.y);
        public static double operator *(Vetor A, Vetor B) => A.x * B.x + A.y * B.y;
        public static Vetor operator -(Vetor v) => new Vetor(-v.x, -v.y);
        public static bool operator <(Vetor A, Vetor B) => A.Tamanho < B.Tamanho;
        public static bool operator >(Vetor A, Vetor B) => A.Tamanho > B.Tamanho;
        public static explicit operator Vetor(Cord p) => new Vetor(p.x, p.y);
        public double Tamanho => Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        public override string ToString() => $"{{x:{x}, y:{y}}}";
        public Vetor Normalizar()
        {
            return this / Tamanho;
        }
        public Vetor Normalizado
        {
            get
            {
                var NormX = x > 0 ? 1 : x < 0 ? -1 : 0;
                var NormY = y > 0 ? 1 : y < 0 ? -1 : 0;
                return new Vetor(NormX, NormY);
            }
        }


    }
    [Serializable]
    public struct Cord
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public Cord(double cx, double cy, double cz = 1)
        {
            x = cx;
            y = cy;
            z = cz;
        }
        public override string ToString() => $"{{x:{x}, y:{y}}}";
        public static Cord NaN = new Cord(double.NaN, double.NaN, double.NaN);
        public static bool operator ==(Cord a, Cord b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Cord a, Cord b) => !(a == b);
        public static bool operator >(Cord a, Cord b) => a.x > b.x && a.y > b.y;
        public static bool operator <(Cord a, Cord b) => a.x < b.x && a.y < b.y;
        public static bool operator >=(Cord a, Cord b) => a > b || b == a;
        public static bool operator <=(Cord a, Cord b) => a < b || b == a;
        public static Cord operator -(Cord a, Cord b) => new Cord(a.x - b.x, a.y - b.y);
        public static Cord operator +(Cord a, Vetor b) => new Cord(a.x + b.x, a.y + b.y);
        public static Cord operator /(Cord a, double b) => new Cord(a.x / b, a.y / b);
        public static implicit operator Cord((double x, double y) p) => new Cord(p.x, p.y);

        public override bool Equals(Object obj)
        {
            if (obj is Point)
            {
                Cord p = (Cord)obj;
                return x == p.x & y == p.y;
            }
            else return false;
        }

        public override int GetHashCode() => (x, y).GetHashCode();


    }




    public static class Helper
    {
        public const double TamanhoPadrao = 20;

        private static double tempo = 0;
        public static double TempoAnterior { get; private set; }
        public static double DeltaTempo => Tempo - TempoAnterior;
        public static Vetor ParedeVelocidade { get; set; }
        public static double Tempo
        {
            get => tempo;
            set
            {
                TempoAnterior = tempo;

                tempo = value;
            }
        }
        public static void TranferirForca(this IMovel A, IMovel B, Vetor ForcaTransferida)
        {
            A.Mov.AplicarForca(-ForcaTransferida);
            B.Mov.AplicarForca(ForcaTransferida);
        }

        public static Random Rnd = new Random();


    }

}