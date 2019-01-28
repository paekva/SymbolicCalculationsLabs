using SymCal_2.Operators;
using SymCal_2.ToSort;

namespace SymCal_2
{
    public interface IOperations
    {
        IExpressionType doUnit(IUnit u);
        
        IExpressionType doFunction(Function fun);
        
        IExpressionType doVariable(Variable v);

        IExpressionType doVarDefenition(Equation eq);

        IExpressionType doFuncDefenition(FunctionDefenition fd);

    }
}