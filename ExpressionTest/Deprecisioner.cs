using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTest
{
    public class Deprecisioner
    {
        const int DEFAULT_DEPRECISION = 28;
        private double[] PowersOfTwoPlusOne;

        public Deprecisioner()
        {
            PowersOfTwoPlusOne = new double[54];
            for (var i = 0; i < PowersOfTwoPlusOne.Length; i++)
            {
                if (i == 0)
                    PowersOfTwoPlusOne[i] = 1; // Special case.
                else
                {
                    long two_to_i_plus_one = (1L << i) + 1L;
                    PowersOfTwoPlusOne[i] = (double)two_to_i_plus_one;
                }
            }
        }

        public double AdjustPrecisionSafely(double d, int digits = DEFAULT_DEPRECISION)
        {
            double t = d * PowersOfTwoPlusOne[53 - digits];
            double adjusted = t - (t - d);
            return adjusted;
        }

        public void AdjustPrecisionSafely(ref double[] d, int digits = DEFAULT_DEPRECISION)
        {
            for (int idx = 0; idx < d.Length; idx++)
                d[idx] = AdjustPrecisionSafely(d[idx], digits);
        }

        public void AdjustPrecisionSafely(ref double[,] d, int digits = DEFAULT_DEPRECISION)
        {
            for (int ii = 0; ii < d.GetLength(0); ii++)
                for (int jj = 0; jj < d.GetLength(1); jj++)
                    d[ii, jj] = AdjustPrecisionSafely(d[ii, jj], digits);
        }
    }
}
