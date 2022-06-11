using System.Runtime.CompilerServices;

namespace ExpressionTest.ExpressionParsers
{
    public class Native : IExpParser
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        override public double Calculate(double x, double y, double z)
        {
            return ExpFunc.NativeFunction(x, y, z);
        }
        public Native(string expression) : base("Native")
        {

        }
    }
}
