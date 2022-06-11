using NCalc;
using System;
using System.Runtime.CompilerServices;

namespace ExpressionTest.ExpressionParsers
{
    public class NCalc : IExpParser
    {
        Expression expr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        override public double Calculate(double x, double y, double z)
        {

            expr.Parameters["x"] = x;
            expr.Parameters["y"] = y;
            expr.Parameters["z"] = z;
            object obj = expr.Evaluate();
            return Convert.ToDouble(obj);
        }

        public NCalc(string expression) : base("NCalc")
        {

            expression = ExpressionFixer.ProcessNCalc(expression);

            expr = new Expression(expression, EvaluateOptions.IgnoreCase);

            expr.Parameters["x"] = 1.0;
            expr.Parameters["y"] = 0.0;
            expr.Parameters["z"] = 0.0;
            expr.Parameters["pi"] = Math.PI;

            expr.Evaluate();
        }
    }
}