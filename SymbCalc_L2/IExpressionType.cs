using System.Collections.Generic;

namespace SymCal_2
{
    public interface IExpressionType
    {
        Types getType();

        List<IExpressionType> getOperands();
        
        IExpressionType doOperation(IOperations operation);
    }
}