using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using ExpressionTest.ExpressionParsers;
using System.Threading;
using System.Linq;

namespace ExpressionTest
{
    [MedianColumn, MinColumn, MaxColumn, RankColumn]
    [Config(typeof(FastAndDirtyConfig))]
    public class ExpressionTestSuite
    {
        const int N = 100000;
        static Random rnd = new Random(22);

        static double[] x = RandomArray(N, 1.0, 0.0);
        static double[] y = RandomArray(N, 10.0, 0.0);
        static double[] z = RandomArray(N, 3.0, 0.0);
        static double[] correct;
        double[] latest = new double[N];

        Dictionary<string, IExpParser> parsers = new Dictionary<string, IExpParser>();

        [Benchmark(Baseline = true)]
        public double[] Baseline()
        {
            double[] latest = new double[N];
            for (int idx = 0; idx < N; ++idx)
                latest[idx] = ExpFunc.NativeFunction(x[idx], y[idx], z[idx]);
            return latest;
        }

        public ExpressionTestSuite()
        {
            Console.WriteLine("Initialising ExpressionTestSuite");

            parsers.Add("Native", new ExpressionParsers.Native(ExpFunc.String));
            parsers.Add("mXparser", new ExpressionParsers.mXparser(ExpFunc.String));

            try { parsers.Add("Jace", new ExpressionParsers.Jace(ExpFunc.String)); }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            try { parsers.Add("Flee", new ExpressionParsers.Flee(ExpFunc.String)); }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            try { parsers.Add("NCalc", new ExpressionParsers.NCalc(ExpFunc.String)); }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            try { parsers.Add("NCalc2", new ExpressionParsers.NCalc2(ExpFunc.String)); }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            try { parsers.Add("exprtk", new ExpressionParsers.exprtk(ExpFunc.String)); }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            try { parsers.Add("exprtkUnsafe", new ExpressionParsers.exprtkUnsafe(ExpFunc.String)); }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            try { parsers.Add("exprtkVec", new ExpressionParsers.exprtkVec(ExpFunc.String)); }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            // + Native C#
            // + mXparser
            // + ncalc ----------- ^ does not work, abs() returns decimal
            // + NCalc2 ---------- ^ does not work
            // + Flee
            // + Jace

            correct = parsers["Native"].Calculate(x, y, z);
            deprec.AdjustPrecisionSafely(ref correct);

            RemoveBrokenParsers();
        }

        #region Benchmarks

        [Benchmark]
        public void Native() { latest = RunParser("Native"); }
        [Benchmark]
        public void mXparser() { latest = RunParser("mXparser"); }
        [Benchmark]
        public void NCalc() { latest = RunParser("NCalc"); }
        [Benchmark]
        public void NCalc2() { latest = RunParser("NCalc2"); }
        [Benchmark]
        public void Jace() { latest = RunParser("Jace"); }
        [Benchmark]
        public void Flee() { latest = RunParser("Flee"); }
        [Benchmark]
        public void exprtk() { latest = RunParser("exprtk"); }
        [Benchmark]
        public void exprtkUnsafe() { latest = RunParser("exprtkUnsafe"); }
        [Benchmark]
        public void exprtkVec() { latest = RunParser("exprtkVec"); }


        #endregion


        #region Helper Functions
        double[] RunParser(string parser)
        {
            return parsers[parser].Calculate(x, y, z);
        }
        public bool CheckAll(bool bPrint = true)
        {
            bool bAllOk = true;
            foreach(string parserName in parsers.Keys)
            {
                if (CheckParser(parserName, bPrint))
                    Console.WriteLine("{0} - all values correct.", parserName);
                else
                {
                    Console.WriteLine("{0} - wrong!", parserName);
                    bAllOk = false;
                }
            }
            return bAllOk;
        }
        public void RemoveBrokenParsers()
        {
            List<string> allParsers = parsers.Keys.ToList();
            foreach (string parserName in allParsers)
            {
                if (!CheckParser(parserName, false))
                    parsers.Remove(parserName);
            }
        }


        static Deprecisioner deprec = new Deprecisioner();

        bool CheckParser(string strParser, bool bPrint = true)
        {
            try
            {
                latest = parsers[strParser].Calculate(x, y, z); ;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                return false;
            }

            deprec.AdjustPrecisionSafely(ref latest);

            bool bAllOk = true;
            for (int idx = 0; idx < N; ++idx)
            {



                if ((double.IsNaN(correct[idx]) && double.IsNaN(latest[idx])) || (double.IsInfinity(correct[idx]) && double.IsInfinity(latest[idx])))
                {
                    continue;
                }    
                if (Math.Abs(latest[idx] - correct[idx]) < 1e-9)
                {
                    continue;
                }

                bAllOk = false;

                if (bPrint)
                {
                    Console.WriteLine("Wrong Value ({0}): {1}. Expected: f({2}, {3}, {4}) = {5}", strParser, latest[idx].ToString(),
                        x[idx], y[idx], z[idx], correct[idx].ToString());
                }
            }
            return bAllOk;
        }


        static double[] RandomArray(int N, double max = 1.0, double min = 0.0)
        {
            double rng = max - min;
            double[] arr = new double[N];

            for (int idx = 0; idx < N; ++idx)
                arr[idx] = min + rnd.NextDouble() * rng;

            return arr;
        }

        #endregion
    }
}