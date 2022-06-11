using System;

namespace ExpressionTest
{
    public abstract class IExpParser
    {
        public abstract double Calculate(double x, double y, double z);

        virtual public double[] Calculate(double[] x, double[] y, double[] z)
        {
            int N = x.Length;
            double[] res = new double[N];

            for (int idx = 0; idx < N; ++idx)
                res[idx] = Calculate(x[idx], y[idx], z[idx]);

            return res;
        }

        protected IExpParser(string strClass)
        {
            Console.WriteLine("Initialising {0}...", strClass);
        }
    }
}
