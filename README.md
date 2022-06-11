# ExpressionTest
C# Expression Parser Tests:
  - Native C# (Baseline)
  - mxParser
  - NCalc
  - NCalc2
  - Jace
  - Flee
  - exprtk (C++/CLI)


# Example results for expression "(x + y) * z":

|       Method |         Mean |        Error |       StdDev |       Median |          Min |          Max |  Ratio | RatioSD | Rank |
|------------- |-------------:|-------------:|-------------:|-------------:|-------------:|-------------:|-------:|--------:|-----:|
|     Baseline |     257.8 us |     19.22 us |     22.13 us |     268.6 us |     217.4 us |     284.6 us |   1.00 |    0.00 |    1 |
|       Native |     281.8 us |     19.32 us |     22.24 us |     289.4 us |     240.6 us |     318.9 us |   1.10 |    0.15 |    2 |
|     mXparser | 240,051.2 us | 17,015.53 us | 19,595.10 us | 247,572.8 us | 203,298.2 us | 265,697.1 us | 936.60 |  102.10 |    8 |
|        NCalc |  58,337.4 us |  3,499.54 us |  3,889.73 us |  58,808.0 us |  49,799.3 us |  63,512.2 us | 228.78 |   27.87 |    7 |
|       NCalc2 |  49,520.1 us |  5,723.03 us |  6,590.65 us |  49,232.9 us |  40,367.7 us |  64,370.9 us | 193.63 |   32.81 |    6 |
|         Jace |  52,104.8 us |  3,916.55 us |  4,353.24 us |  52,940.1 us |  41,964.8 us |  61,627.6 us | 204.25 |   25.45 |    6 |
|         Flee |  29,093.9 us |  2,349.09 us |  2,611.01 us |  29,696.9 us |  24,558.1 us |  33,443.1 us | 114.02 |   14.88 |    5 |
|       exprtk |   3,176.4 us |    228.95 us |    263.65 us |   3,296.6 us |   2,420.5 us |   3,414.7 us |  12.44 |    1.77 |    4 |
| exprtkUnsafe |   3,124.6 us |    361.75 us |    416.59 us |   3,301.6 us |   2,309.0 us |   3,864.5 us |  12.25 |    2.22 |    4 |
|    exprtkVec |     577.6 us |     16.69 us |     17.86 us |     579.1 us |     541.8 us |     605.5 us |   2.27 |    0.22 |    3 |
