
using System;
using System.Drawing;


namespace Engine
{
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

}