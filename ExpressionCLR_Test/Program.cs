using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionCLR_Test
{
    internal class Program
    {
        const int N = 10000;
        readonly static Random rnd = new Random(22);

        readonly static double[] x = RandomArray(N, 1.0, 0.0);
        readonly static double[] y = RandomArray(N, 10.0, 0.0);
        readonly static double[] z = RandomArray(N, 3.0, 0.0);
        readonly static double[] correct = Correct();

        static double[] RandomArray(int N, double max = 1.0, double min = 0.0)
        {
            double rng = max - min;
            double[] arr = new double[N];

            for (int idx = 0; idx < N; ++idx)
                arr[idx] = min + rnd.NextDouble() * rng;

            return arr;
        }

        static double[] Correct()
        {
            double[] arr = new double[N];
            for(int idx = 0; idx < N; ++idx)
            {
                arr[idx] = Correct(x[idx], y[idx], z[idx]);
            }
            return arr;
        }

        static double Correct(double x, double y, double z)
        {
            return (x + y / z);
        }
        static void Main()
        {
            //string strExpression = "clamp(-1.0,sin(2 * pi * x) + cos(x / 2 * pi),+1.0)";
            //string strExpression = "sqrt(1 - (3 / (x * x)) + abs(y)) / z";
            //string strExpression = "x + g / (3*pi)";
            string strExpression = "x + y/z";
            Dictionary<string, double> constants = new Dictionary<string, double>() { { "g", 9.8066 } };
            /*for (int idx = 0; idx < 100; idx++)
            {
                for (int jdx = 0; jdx < 100; jdx++)
                {
                    using (ExpressionCLR.ExpressionCLR newExpr = new ExpressionCLR.ExpressionCLR(strExpression))
                    {
                        test2(newExpr);
                    }
                }
            }
            Console.WriteLine(val);
            */

            ExpressionCLR.ExpressionCLR expr = new ExpressionCLR.ExpressionCLR(strExpression, constants);
            /*
            test1(expr);
            double[] inputs = new double[] { 1.0 };
            Console.WriteLine("Value = {0}", expr.EvaluateUnsafe(inputs));
            */
            double[] outputs1 = expr.Evaluate(new List<double[]>() { x, y, z });
            Console.WriteLine();
            /*
            double[] outputs2 = expr.EvaluateUnsafe(new List<double[]>() { x, y, z });
            Console.WriteLine("\n\n");

            CheckFunc(expr.EvaluateUnsafe);
            CheckFunc(expr.Test1);
            CheckFunc(expr.Test2);
            CheckFuncT(expr.Test3);
            int nT = 2;
            double[] t = new double[nT];
            for (int ii = 0; ii < nT; ++ii)
                t[ii] = 0;

            int nS = 1;
            for (int jj = 0; jj < nS; ++jj)
            {
                double[] outputs = expr.Test(new List<double[]>() { x, y, z });
                for (int ii = 0; ii < nT; ++ii)
                    t[ii] += outputs[ii];
            }

            Console.WriteLine("\n\nTimes:");
            for (int ii = 0; ii < nT; ++ii)
                Console.WriteLine("#{0}: {1}ns", ii, t[ii] / nS);
            */


            //std::cout << "\nMethod #1: " << duration(t4 - t3) << "ns\nMethod #2: " << duration(t5 - t4) << "ns\nMethod #3: " << duration(t6 - t5) << "ns\n   To Sum: " << duration(t7-t6) << "ns\n";

            Console.ReadLine();
        }
        static double CheckFuncT(Func<double[], double[]> f)
        {
            int n = 10;

            double[] inputs = new double[x.Length * 3];
            for (int idx = 0; idx < x.Length; ++idx)
            {
                inputs[idx*3] = x[idx];
                inputs[idx*3+1] = y[idx];
                inputs[idx*3+2] = z[idx];
            }






            Stopwatch stopWatch = new Stopwatch();
            double[] sum = new double[x.Length];
            for (int idx = 0; idx < n; ++idx)
            {
                stopWatch.Start();
                double[] output = f(inputs);
                stopWatch.Stop();
                for (int jdx = 0; jdx < x.Length; ++jdx)
                {
                    sum[jdx] += output[jdx];
                }
            }
            double[] res = new double[x.Length];
            for (int jdx = 0; jdx < x.Length; ++jdx)
                res[jdx] = sum[jdx] / n;

            bool isOk = CheckArr(res);
            double t = stopWatch.Elapsed.TotalMilliseconds;
            Console.Write("{0}: ", f.Method.Name);
            if (isOk)
                Console.WriteLine("Elapsed time {0}ms", t);
            else
                Console.WriteLine("WRONG---> Elapsed time {0}ms", t);

            return (isOk ? 1 : -1) * t;
        }

        static double CheckFunc(Func<List<double[]>, double[]> f)
        {
            int n = 10;
            List<double[]> inputs = new List<double[]>() { x, y, z };
            Stopwatch stopWatch = new Stopwatch();
            double[] sum = new double[x.Length];
            for (int idx = 0; idx < n; ++idx)
            {
                stopWatch.Start();
                double[] output = f(inputs);
                stopWatch.Stop();
                for (int jdx = 0; jdx < x.Length; ++jdx)
                {
                    sum[jdx] += output[jdx];
                }
            }
            double[] res = new double[x.Length];
            for (int jdx = 0; jdx < x.Length; ++jdx)
                res[jdx] = sum[jdx] / n;

            bool isOk = CheckArr(res);
            double t = stopWatch.Elapsed.TotalMilliseconds;
            Console.Write("{0}: ", f.Method.Name);
            if (isOk)
                Console.WriteLine("Elapsed time {0}ms", t);
            else
                Console.WriteLine("WRONG---> Elapsed time {0}ms", t);

            return (isOk ? 1 : -1) * t;
        }

        static bool CheckArr(double[] res)
        {
            for (int idx = 0; idx < res.Length; ++idx)
            {
                if (Math.Abs(res[idx] - correct[idx]) > 1e-9)
                    return false;
            }
            return true;
        }

        /*static void Test1(ExpressionCLR.ExpressionCLR expr)
        {
            if (!expr.IsValid())
            {
                Console.WriteLine("Something went wrong!");
                return;
            }
            Console.WriteLine(expr.ToPrintString());
        }

        static double val = 0;
        static void Test2(ExpressionCLR.ExpressionCLR expr)
        {
            if (!expr.IsValid())
            {
                Console.WriteLine("Something went wrong!");
                return;
            }
            val += expr.Evaluate();
            //Console.WriteLine(expr.ToPrintString());
        }*/
    }
}
