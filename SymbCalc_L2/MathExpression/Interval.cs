using System.Collections.Generic;

namespace SymCal_2.Operands
{
    public class Interval: IMathExpression
    {
        private List<IExpressionType> _operands;
        
        
        public Interval(List<IExpressionType> operands) {_operands = operands; }
        public Types getType() {return Types.Interval;}
        public List<IExpressionType> getOperands() {return _operands;}
        
        
        
        public IExpressionType doOperation(IOperations p){ return this; }
        
        public bool compare(IMathExpression e){ return false; }

    }
}