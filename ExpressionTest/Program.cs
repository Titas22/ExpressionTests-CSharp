
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Configs;
using Perfolizer.Horology;
using mXParser = org.mariuszgromada.math.mxparser;
using System.Text.RegularExpressions;

namespace ExpressionTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nExpression: {0}\n", ExpFunc.String);

            Regex rx = new Regex(@"([eE])(?=\d)", RegexOptions.Compiled);
            Console.WriteLine("Fixed: {0}", rx.Replace(ExpFunc.String, "e+"));





            ExpressionTestSuite test = new ExpressionTestSuite();
            if (!test.CheckAll(true))
            {
                Console.WriteLine("\nSomething went wrong. Press Enter to continue to benchmarks...");
                Console.ReadLine();
            }

#if !DEBUG
            BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<ExpressionTestSuite>();
#endif

            Console.WriteLine("\nExpression: {0}\n", ExpFunc.String);
            if (!test.CheckAll(false))
            {
                Console.WriteLine("\nSomething went wrong!");
            }

            Console.WriteLine("\nAll Done!");
            Console.ReadLine();
        }


    }
}
