using System;
using System.Collections.Generic;
using static System.Math;

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
        }
        public Vetor(Vetor cord)
        {
            x = cord.x;
            y = cord.y;
        }
        public static Vetor Polar(double Magnetude, double Angulo){
            var x = Magnetude * Cos(Angulo);
            var y = Magnetude * Sin(Angulo);
            return new Vetor(x,y);
        }
        public static readonly Vetor Zero = new Vetor(0, 0);
        public static readonly Vetor NaN = new Vetor(double.NaN, double.NaN);
        public static readonly Vetor Esquerda = new Vetor(-1, 0);
        public static readonly Vetor Direita = new Vetor(1, 0);
        public static readonly Vetor Cima = new Vetor(0, -1);
        public static readonly Vetor Baixo = new Vetor(0, 1);
        public static Vetor operator *(Vetor A, double Escalar) => new Vetor(A.x * Escalar, A.y * Escalar);
        public static Vetor operator *(double Escalar,Vetor A) => new Vetor(A.x * Escalar, A.y * Escalar);
        public static Vetor operator /(Vetor A, double Escalar) => new Vetor(A.x / Escalar, A.y / Escalar);
        public static Vetor operator +(Vetor A, Vetor B) => new Vetor(A.x + B.x, A.y + B.y);
        public static Vetor operator -(Vetor A, Vetor B) => new Vetor(A.x - B.x, A.y - B.y);
        public static double operator *(Vetor A, Vetor B) => A.x * B.x + A.y * B.y;
        public static Vetor operator -(Vetor v) => new Vetor(-v.x, -v.y);
        public static bool operator == (Vetor A, Vetor B) => A.x == B.x && A.y == B.y;
        public static bool operator != (Vetor A, Vetor B) => A.x != B.x || A.y != B.y;
        public static implicit operator Vetor((double x, double y) t) => new Vetor(t.x, t.y);
        public double Tamanho => Sqrt(Pow(x, 2) + Pow(y, 2));
        public override string ToString() => $"{{x:{x}, y:{y}}}";
        public Vetor Normalizar()
        {
            return this / Tamanho;
        }

		public override bool Equals(object obj)
		{
			return obj is Vetor vetor &&
				   x == vetor.x &&
				   y == vetor.y &&
				   Tamanho == vetor.Tamanho &&
				   EqualityComparer<Vetor>.Default.Equals(Normalizado, vetor.Normalizado);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(x, y, Tamanho, Normalizado);
		}

		public Vetor Normalizado
        {
            get
            {
                var NormX = x > 0 ? 1 : x < 0 ? -1 : 0;
                var NormY = y > 0 ? 1 : y < 0 ? -1 : 0;
                return new Vetor((double)NormX, (double)NormY);
            }
        }


    }

}