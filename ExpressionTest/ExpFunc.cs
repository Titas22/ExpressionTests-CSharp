#define EXP1
using System;
using System.Runtime.CompilerServices;

namespace ExpressionTest
{
    public static class ExpFunc
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double NativeFunction(double x, double y, double z)
#if EXP1
        { return (x + y) * z; }
        public const string String = "(x + y) * z";
#elif EXP2
            { return Math.Max(x, Math.Min(y, z)); }
        public const string String = "max(x, min(y, z))";
#elif EXP3
        { return Math.Sqrt(1 - (3 / (x * x)) + Math.Abs(y)) / z; }
        public const string String = "sqrt(1 - (3 / (x*x)) + abs(y)) / z";
        /*
        { return Math.Sqrt(1 - (3 / (x* x)) + Math.Abs(y)) / z; }
        public const string String = "sqrt(1 - (3 / x^2) + abs(y)) / z";*/
        
#elif EXP4
        { return (Math.Sin(2 * Math.PI * x) + Math.Cos(y / 2 * Math.PI)) * z; }
        public const string String = "(sin(2 * pi * x) + cos(y / 2 * pi)) * z";
#elif EXP5
        { return 75*Math.Pow(x, 17) + 25.1*Math.Pow(y, 5) - 35*Math.Pow(z, 4) - 15.2*x*x*x + 40*y*y - 15.3*z + 1; }
        //public const string String = "75x^17 + 25.1y^5 - 35z^4 - 15.2x^3 + 40y^2 - 15.3z + 1";
        public const string String = "75*pow(x, 17) + 25.1*pow(y, 5) - 35*pow(z, 4) - 15.2*x*x*x + 40*y*y - 15.3*z + 1";
        //public const string String = "75*x^17 + 25.1*y^5 - 35*z^4 - 15.2*x^3 + 40*y^2 - 15.3*z + 1";
#elif EXP6
        { return Math.Pow(2, 3); }
        public const string String = "pow(2, 3)";
#else
        { return 1.2e2 + 1.2e+2 + 1.2e-2; }
        public const string String = "1.2e2 + 1.2e+2 + 1.2e-2";
#endif
    }
}