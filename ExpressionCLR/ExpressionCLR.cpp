#include "pch.h"

#include "ExpressionCLR.h"
#include <msclr\marshal_cppstd.h>
#include <stdlib.h>
#include <string.h>

using namespace System::Collections::Generic;



// Constructor
//Expression(const std::string strExpression, const std::unordered_map<std::string, double> constants = std::unordered_map<std::string, double>());
ExpressionCLR::ExpressionCLR::ExpressionCLR(System::String^ clrExpression, Dictionary<System::String^, double>^ clrConsts)
{
	// Parse the inputs
	std::string strExpression = msclr::interop::marshal_as<std::string>(clrExpression);
	std::unordered_map<std::string, double> myconsts = ConvertConstants(clrConsts);

	// Create the expression
	expr = new Expression(strExpression, myconsts);

	// Need to assign all these to be valid from C#
	this->bValid = expr->IsValid();
	std::vector<std::string> cppInputs = expr->GetInputList();
	this->strPrint = gcnew System::String(expr->ToPrintString().c_str());

	this->inputList = gcnew List<System::String^>((int)cppInputs.size());
	for (std::string str : cppInputs)
		this->inputList->Add(gcnew System::String(str.c_str()));

	nVars = expr->GetNumberOfInputs();
	pInputs = new double*[nVars];

	//pInputs = expr->GetInputsPtr();
}

// Finalizer  
ExpressionCLR::ExpressionCLR::!ExpressionCLR()
{
	if (this->expr != nullptr)
	{
		delete this->expr;
		this->expr = nullptr;
		delete[] pInputs;
	}
}

double ExpressionCLR::ExpressionCLR::Evaluate(array<double>^ clrInputs)
{
	int nInputs = clrInputs->Length;
	if (nInputs != nVars)
		return NaN;
	if (nInputs == 0)
		return expr->Evaluate();

	pin_ptr<double> pinnedArr = &clrInputs[0];  // entire array is now pinned

	return expr->EvaluateUnsafe(pinnedArr);
}

double ExpressionCLR::ExpressionCLR::EvaluateUnsafe(array<double>^ clrInputs)
{
	pin_ptr<double> pinnedArr = &clrInputs[0];   // entire array is now pinned
	
	return expr->EvaluateUnsafe(pinnedArr);
}


array<double>^ ExpressionCLR::ExpressionCLR::Evaluate(List<array<double>^>^ clrInputsVec)
{
	if (clrInputsVec->Count != nVars || nVars == 0)
		return gcnew array<double>(0);

	int nSamples = clrInputsVec[0]->Length;
	for (int idx = 1; idx < nVars; ++idx)
	{
		if ((clrInputsVec[idx]->Length) != nSamples)
		{
			return gcnew array<double>(0);
		}
	}

	for (int idx = 0; idx < nVars; ++idx)
	{
		array<double>^ arr = clrInputsVec[idx];
		pin_ptr<double> pinnedInput = &arr[0];
		pInputs[idx] = pinnedInput;
	}

	array<double>^ output = gcnew array<double>(nSamples);
	pin_ptr<double> pinnedArr = &output[0];
	double* pOut = pinnedArr;

	expr->EvaluateUnsafe(&pInputs[0], pOut, nSamples);

	return output;
}


std::unordered_map<std::string, double> ConvertConstants(Dictionary<System::String^, double>^ clrConsts)
{
	// Allocate the output map
	std::unordered_map<std::string, double> myconsts(clrConsts->Count);

	// Enumerate through all entries
	Dictionary<System::String^, double>::Enumerator enumConsts = clrConsts->GetEnumerator();
	while (enumConsts.MoveNext())
	{
		// Get Key-Value pair
		KeyValuePair<System::String^, double> kvp = enumConsts.Current;

		// Marshal data
		std::string key = msclr::interop::marshal_as<std::string>(kvp.Key);
		double val = kvp.Value;

		// Store in the map
		myconsts[key] = val;
	}

	return myconsts;
}