using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExpressionTest
{
    public static class ExpressionFixer
    {
        static Regex rxScientific = new Regex(@"([eE])(?=\d)", RegexOptions.Compiled);
        static Regex rxScientificPos = new Regex(@"([eE][+])", RegexOptions.Compiled);
        static Regex rxScientificNeg = new Regex(@"([eE][-])", RegexOptions.Compiled);
        static Regex rxMultiplication = new Regex(@"(?<=\d)(?=(?:(?![eE])[a-zA-Z]))", RegexOptions.Compiled);

        static string FixScientificNotation(string input)
        {
            return rxScientific.Replace(input, "e+");
        }
        static string FixScientificNotationJace(string input)
        {
            input = rxScientific.Replace(input, "*10^");
            input = rxScientificPos.Replace(input, "*10^");
            return rxScientificNeg.Replace(input, "*0.1^");
        }

        static string FixImplicitMultiplication(string input)
        {
            return rxMultiplication.Replace(input, "*");
        }

        public static string ProcessFlee(string expression)
        {
            string strFixed = expression;

            strFixed = FixScientificNotation(strFixed);
            strFixed = FixImplicitMultiplication(strFixed);

            return strFixed;
        }

        public static string ProcessExprtk(string expression)
        {
            string strFixed = expression;

            strFixed = FixScientificNotation(strFixed);

            return strFixed;
        }


        public static string ProcessJace(string expression)
        {
            string strFixed = expression;

            strFixed = FixScientificNotationJace(strFixed);
            strFixed = FixImplicitMultiplication(strFixed);

            return strFixed;
        }

        public static string ProcessNCalc(string expression)
        {
            string strFixed = expression;

            strFixed = FixImplicitMultiplication(strFixed);

            return strFixed;
        }
    }
}
