namespace FixMath.NET
{
    public readonly struct FixVector3
    {
        public readonly Fix64 x;
        public readonly Fix64 y;
        public readonly Fix64 z;

        public FixVector2 XY => new FixVector2(x, y);
        public FixVector2 XZ => new FixVector2(x, z);

        public Fix64 Magnitude => Fix64.Sqrt(SqrMagnitude);
        public Fix64 SqrMagnitude => x * x + y * y + z * z;
        public FixVector3 Normalized => this / Magnitude;
        
        public FixVector3(Fix64 x, Fix64 y, Fix64 z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static FixVector3 operator +(FixVector3 a, FixVector3 b) => new FixVector3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static FixVector3 operator -(FixVector3 a, FixVector3 b) => new FixVector3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static FixVector3 operator *(FixVector3 v, Fix64 s) => new FixVector3(v.x * s, v.y * s, v.z * s);
        public static FixVector3 operator /(FixVector3 v, Fix64 s) => v * (Fix64.One / s);
    }
}