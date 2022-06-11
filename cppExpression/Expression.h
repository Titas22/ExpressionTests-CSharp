#pragma once

#include "pch.h"
#include <iostream>
#include <unordered_map>

const std::unordered_map<std::string, double> DEFAULT_CONSTANTS = {
    {"nan", NaN},
    {"NAN", NaN},
    {"NaN", NaN},
    {"PI", M_PI}
};

void ExtractUsedConstants(const exprtk::symbol_table<double>& symbolTable, const std::vector<std::string>& vars, std::vector<std::string>& consts);
bool AddConstants(exprtk::symbol_table<double>& symbolTable, const std::unordered_map<std::string, double> constants);
void RemoveConstantsFromVariableList(const exprtk::symbol_table<double>& symbolTable, std::vector<std::string>& vars);

class Expression
{
public:
    Expression(const std::string strExpression, const std::unordered_map<std::string, double> constants = std::unordered_map<std::string, double>());
    ~Expression() { delete expr; }  // Clear the expression object

    std::vector<std::string> GetInputList() { return vars; }
    bool IsValid() { return bIsValid; }

    double Evaluate(const std::vector<double> newInputs = std::vector<double>(0));
    std::vector<double> Evaluate(const std::vector<std::vector<double>> newInputVecs);

    double EvaluateUnsafe(const double* const newInputs);
    void EvaluateUnsafe(const double* const* const pIn, double* pOut, const int sz);
    void EvaluateUnsafeT(const double* const pIn, double* pOut, const int sz);

    double GetValue() { return expr->value(); }

    std::string ToPrintString();
    friend std::ostream& operator<<(std::ostream& os, Expression const& expr);
    friend std::ostream& operator<<(std::ostream& os, Expression* const expr) { return os << *expr; }

    double* GetInputsPtr() { return &inputs[0]; }
    size_t GetNumberOfInputs() { return nVars; }

private:
    bool InitialiseConstants(std::unordered_map<std::string, double> constants);
    bool CompileExpression(const std::string strExpr);
    void InitialiseInputVariables();

    std::string strExpression;
    exprtk::expression<double>* expr;
    exprtk::symbol_table<double> symbolTable;
    std::vector<std::string> vars;
    std::vector<std::string> consts;
    std::vector<double> inputs;
    size_t nVars;
    bool bIsValid = false;
};