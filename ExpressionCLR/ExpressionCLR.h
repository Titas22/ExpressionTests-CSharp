#pragma once
#include "pch.h"
#include "..\cppExpression\Expression.h"

using namespace System::Collections::Generic;

#include <chrono>
#include <utility>

typedef std::chrono::high_resolution_clock::time_point TimeVar;

#define duration(a) std::chrono::duration_cast<std::chrono::nanoseconds>(a).count()
#define timeNow() std::chrono::high_resolution_clock::now()


namespace ExpressionCLR {
	public ref class ExpressionCLR
	{
    public:
        ExpressionCLR(System::String^ clrExpression, System::Collections::Generic::Dictionary<System::String^, double>^ constants); // Constructor
        ~ExpressionCLR() { this->!ExpressionCLR(); }; //Destructor
        !ExpressionCLR(); // Finaliser

        System::Collections::Generic::List<System::String^>^ GetInputList() { return inputList; }
        System::String^ ToPrintString() { return strPrint; }
        bool IsValid() { return bValid; }

        double Evaluate() { return expr->Evaluate(); };
        double Evaluate(array<double>^ inputs);
        double EvaluateUnsafe(array<double>^ clrInputs);

        array<double>^ Evaluate(List<array<double>^>^ vecInputs);

    private:
        Expression* expr = nullptr;
        bool bValid = false;
        System::String^ strPrint; 
        System::Collections::Generic::List<System::String^>^ inputList;

        size_t nVars = 0;
        double** pInputs;
	};

}

std::unordered_map<std::string, double> ConvertConstants(Dictionary<System::String^, double>^ clrConsts);