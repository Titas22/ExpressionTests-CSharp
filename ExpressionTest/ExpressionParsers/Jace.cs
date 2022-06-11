using Jace;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExpressionTest.ExpressionParsers
{
    public class Jace : IExpParser
    {
        static CalculationEngine engine = new CalculationEngine();
        Func<Dictionary<string, double>, double> formula;
        Dictionary<string, double> constants = new Dictionary<string, double>() { { "pi", Math.PI } };
        Dictionary<string, double> variables = new Dictionary<string, double>()
        {
            { "x", 1.0 },
            { "y", 0.0 },
            { "z", 0.0 }
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        override public double Calculate(double x, double y, double z)
        {
            variables["x"] = x;
            variables["y"] = y;
            variables["z"] = z;
            return formula(variables);
        }


        public Jace(string expression) : base("Jace")
        {
            engine.AddFunction("pow", Math.Pow);

            // Fix expression (e.g. 1e2 -> 10^2) 
            expression = ExpressionFixer.ProcessJace(expression);

            formula = engine.Build(expression, constants);
            formula(variables);
        }
    }
}