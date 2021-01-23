namespace FixMath.NET
{
    public readonly struct FixVector2
    {
        public static FixVector2 Zero => new FixVector2(Fix64.Zero, Fix64.Zero);
        public static FixVector2 Up => new FixVector2(Fix64.Zero, Fix64.One);
        public static FixVector2 Right => new FixVector2(Fix64.One, Fix64.Zero);
        public static FixVector2 Down => new FixVector2(Fix64.Zero, -Fix64.One);
        public static FixVector2 Left => new FixVector2(-Fix64.One, Fix64.Zero);
        
        public readonly Fix64 x;
        public readonly Fix64 y;

        public Fix64 Magnitude => Fix64.Sqrt(SqrMagnitude);
        public Fix64 SqrMagnitude => x * x + y * y;
        public FixVector2 Normalized => this / Magnitude;
        
        public FixVector2(Fix64 x, Fix64 y)
        {
            this.x = x;
            this.y = y;
        }

        public static FixVector2 operator +(FixVector2 a, FixVector2 b) => new FixVector2(a.x + b.x, a.y + b.y);
        public static FixVector2 operator -(FixVector2 a, FixVector2 b) => new FixVector2(a.x - b.x, a.y - b.y);
        public static FixVector2 operator *(FixVector2 v, Fix64 s) => new FixVector2(v.x * s, v.y * s);
        public static FixVector2 operator /(FixVector2 v, Fix64 s) => v * (Fix64.One / s);

        public override bool Equals(object obj)
        {
            return obj is FixVector2 vector2 && Equals(vector2);
        }

        public bool Equals(FixVector2 other)
        {
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }
    }
}