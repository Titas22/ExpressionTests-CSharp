using System.Runtime.CompilerServices;
using org.mariuszgromada.math.mxparser;

namespace ExpressionTest.ExpressionParsers
{
    public class mXparser : IExpParser
    {
        Expression mxExpr;
        Argument[] mxArgs;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        override public double Calculate(double x, double y, double z)
        {
            mxArgs[0].setArgumentValue(x);
            mxArgs[1].setArgumentValue(y);
            mxArgs[2].setArgumentValue(z);

            return mxExpr.calculate();
        }

        public mXparser(string expression) : base("mXparser")
        {
            mxArgs = new Argument[3] { new Argument("x = 0"), new Argument("y = 0"), new Argument("z = 0") };
            mxExpr = new Expression(expression);

            mxExpr.addArguments(mxArgs);
            mxExpr.checkSyntax();
            mxExpr.calculate();
        }
    }
}