#include "pch.h"
#include "Expression.h"

exprtk::parser<double> parserD;

Expression::Expression(const std::string strExpr, const std::unordered_map<std::string, double> constants)
{
    bool bAllOk = InitialiseConstants(constants);

    bAllOk &= CompileExpression(strExpr);

    if (!bAllOk)
    {
        delete expr; // Clear the old object
        CompileExpression("null");
    }
    else
    {
        bIsValid = true;
    }
}

bool Expression::InitialiseConstants(const std::unordered_map<std::string, double> constants)
{
    symbolTable.clear_local_constants();
    bool bAllOk = symbolTable.add_constants();
    bAllOk &= AddConstants(symbolTable, DEFAULT_CONSTANTS);
    bAllOk &= AddConstants(symbolTable, constants);
    return bAllOk;
}

void ExtractUsedConstants(const exprtk::symbol_table<double>& symbolTable, const std::vector<std::string>& vars, std::vector<std::string>& consts)
{
    for (std::string var : vars)
    {
        if (!symbolTable.is_constant_node(var))
            continue;
        consts.push_back(var);
    }
}

bool AddConstants(exprtk::symbol_table<double>& symbolTable, const std::unordered_map<std::string, double> constants)
{
    bool bAllOk = true;
    for (auto const& [key, val] : constants)
    {
        if (symbolTable.is_constant_node(key))
            symbolTable.remove_variable(key);
        bAllOk &= symbolTable.add_constant(key, val);
    }
    return bAllOk;
}

void RemoveConstantsFromVariableList(const exprtk::symbol_table<double>& symbolTable, std::vector<std::string>& vars)
{
    // Delete defined constants from variable list
    vars.erase(
        std::remove_if(
            vars.begin(),
            vars.end(),
            [symbolTable](std::string const& var) { return symbolTable.is_constant_node(var);
            }),
        vars.end()
                );
}

void Expression::InitialiseInputVariables()
{
    // Initialise variable list and assign it to the symbolTable
    nVars = vars.size();
    inputs = std::vector<double>(nVars);
    for (size_t idx = 0; idx < nVars; ++idx)
    {
        symbolTable.add_variable(vars[idx], inputs[idx] = NaN);
    }
}

bool Expression::CompileExpression(const std::string strExpr)
{
    this->strExpression = strExpr;
    vars.clear();
    exprtk::collect_variables(strExpression, vars);

    ExtractUsedConstants(symbolTable, vars, consts);
    RemoveConstantsFromVariableList(symbolTable, vars);

    InitialiseInputVariables();

    // Initialise the expression
    expr = new exprtk::expression<double>();
    expr->register_symbol_table(symbolTable);


    // Compile the expression
    parserD.init_precompilation();
    bool isOk = parserD.compile(strExpression, *expr);
    return isOk;
}

double Expression::Evaluate(const std::vector<double> newInputs)
{
    // If wrong number of inputs - return NaN
    if (nVars != newInputs.size())
        return NaN;

    // Update the inputs
    if (nVars == 0)
        return expr->value();

    // Calculate and return
    return this->EvaluateUnsafe(&newInputs[0]);
}

inline double Expression::EvaluateUnsafe(const double* const newInputs)
{   
    // Update the inputs
    memcpy(&inputs[0], newInputs, nVars * sizeof(double));

    // Calculate and return
    return expr->value();
}

std::vector<double> Expression::Evaluate(const std::vector<std::vector<double>> newInputVecs)
{
    // Allocate empty output - default if something is wrong
    std::vector<double> result(0);

    // Check if the number of input arrays is correct
    size_t nInputs = newInputVecs.size();
    if (nInputs == 0 || nInputs != nVars)
        return result;

    // Check if all input arrays are of equal length
    size_t n = newInputVecs[0].size();
    for (std::vector<double> vec : newInputVecs)
        if (vec.size() != n)
            return result;
    
    // Allocate the output array
    result = std::vector<double>(n);

    // Go through all samples
    for (size_t idx = 0; idx < n; ++idx)
    {
        // Assign each variable
        for (size_t iVar = 0; iVar < nVars; ++iVar)
            inputs[iVar] = newInputVecs[iVar][idx];
        
        // Calculate and store the output
        result[idx] = expr->value();
    }

    return result;
}

void Expression::EvaluateUnsafe(const double* const* const pIn, double* pOut, const int sz)
{
    for (int idx = 0; idx < sz; ++idx)
    {
        for (int iVar = 0; iVar < nVars; ++iVar)
        {
            inputs[iVar] = pIn[iVar][idx];
        }
        pOut[idx] = expr->value();
    }
}


std::string Expression::ToPrintString()
{
    std::ostringstream oss;
    oss << this;
    return oss.str();
}

std::ostream& operator<<(std::ostream& os, Expression const& expr)
{
    // Print out the expression
    os << "===== Expression =====\n" << expr.strExpression << "\n";
    os << (expr.bIsValid ? "Is Valid :)" : "IS NOT VALID!!!") << "\n";

    // Print out constants
    os << "\n===== Constants =====\n";
    for (std::string c : expr.consts)
        os << c << " = " << expr.symbolTable.get_variable(c)->value() << "\n";

    // Print out the required input variables
    os << "\n===== Variables =====\n";
    for (std::string v : expr.vars)
        os << v << "\n";

    return os;
}