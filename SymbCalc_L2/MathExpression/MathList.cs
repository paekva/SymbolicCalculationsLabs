using System.Collections.Generic;

namespace SymCal_2.Operands
{
    public class MathList : IMathExpression
    {
        private List<IExpressionType> _operands;
        
        
        public MathList(List<IExpressionType> operands) {_operands = operands; }
        public Types getType() {return Types.List;}
        public List<IExpressionType> getOperands() {return _operands;}
        
        
        
        public IExpressionType doOperation(IOperations o){ return this; }
        
        public bool compare(IMathExpression e){ return false; }
        
    }
}