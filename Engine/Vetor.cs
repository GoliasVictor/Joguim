
using System;


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

}