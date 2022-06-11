using Flee;
using Flee.PublicTypes;
using System;
using System.Runtime.CompilerServices;

namespace ExpressionTest.ExpressionParsers
{
    public class Flee : IExpParser
    {
        ExpressionContext context;
        IDynamicExpression expr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        override public double Calculate(double x, double y, double z)
        {
            expr.Context.Variables["x"] = x;
            expr.Context.Variables["y"] = y;
            expr.Context.Variables["z"] = z;
            object obj = expr.Evaluate();
            return Convert.ToDouble(obj);
        }

        public Flee(string expression) : base("Flee")
        {
            context = new ExpressionContext();
            context.Imports.AddType(typeof(Math)); // Adds functions like sin/cos etc.
            // Define some variables
            context.Variables["x"] = 1.0;
            context.Variables["y"] = 0.0;
            context.Variables["z"] = 0.0;
            context.Variables["pi"] = Math.PI;


            // Fix expression (e.g. 1e2 -> 1e+2) 
            expression = ExpressionFixer.ProcessFlee(expression);

            // Use the variables in the expression
            expr = context.CompileDynamic(expression);
            expr.Evaluate();
        }
    }
}