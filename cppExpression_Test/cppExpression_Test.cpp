#include "../cppExpression/pch.h"
#include "../cppExpression/Expression.h"
#include <iostream>

Expression* expression;

int main()
{
    std::unordered_map<std::string, double> myconsts = 
    {
        {"g", 9.80665},
        {"rho_air", 1.225}
    };

    //std::string strExpression = "clamp(-1.0,sin(2 * pi * x) + cos(x / 2 * pi),+1.0)";
    std::string strExpression = "sqrt(1 - (3 / (x * x)) + abs(y)) / z";
    
    expression = new Expression(strExpression, myconsts);

    std::cout << expression << std::endl;

    double res = expression->Evaluate({ 6.0, 1.0, 2.0 });
    std::cout << "\nValue = " << res << std::endl;


    std::vector<double> x{ 1.0, 2.0, 3.0, 4.0, 5.0 };
    std::vector<double> y{ 1.0, 2.0, 3.0, 4.0, 5.0 };
    std::vector<double> z{ 0.2, 0.25, 0.5, 1.0, 2.0 };

    std::vector<double> vres = expression->Evaluate({ x, y, z });

    for (size_t idx = 0; idx < x.size(); ++idx)
    {
        std::cout << "x: " << x[idx] << "     y: " << y[idx] << "     z: " << z[idx] << "     =     " << vres[idx] << "\n";
    }

    std::string str = expression->ToPrintString();
    std::cout << str << std::endl;

    delete expression;
    return 0;
}