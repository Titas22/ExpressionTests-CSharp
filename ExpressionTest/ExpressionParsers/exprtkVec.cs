﻿using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace ExpressionTest.ExpressionParsers
{
    public class exprtkVec : IExpParser
    {
        ExpressionCLR.ExpressionCLR expr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        override public double Calculate(double x, double y, double z)
        {
            //return expr.Evaluate(new double[] { });
            return expr.Evaluate(new double[] { x, y, z });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        override public double[] Calculate(double[] x, double[] y, double[] z)
        {
            return expr.Evaluate(new List<double[]>{ x, y, z });
        }

        public exprtkVec(string expression) : base("exprtkVec")
        {
            Dictionary<string, double> constants = new Dictionary<string, double>() { { "g", 9.8066 } };

            expr = new ExpressionCLR.ExpressionCLR(expression, constants);

            // Fix expression (e.g. 1e2 -> 1e+2) 
            expression = ExpressionFixer.ProcessExprtk(expression);

            // Use the variables in the expression
            expr.Evaluate();
        }
    }
}