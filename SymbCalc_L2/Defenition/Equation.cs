using System.Collections.Generic;

namespace SymCal_2.Operators
{
    public class Equation: IDefenition
    {
        private List<IExpressionType> operands;
        private bool _isDelayed;
        
        public Equation(List<IExpressionType> expression, bool isDelayed)
        {
            operands = new List<IExpressionType>();
            operands.AddRange(expression);
            _isDelayed = isDelayed;
        }
        
        
        public Types getType() {return Types.VarDefinition;}
        public List<IExpressionType> getOperands() {return operands;}
        public bool isDelayed(){ return _isDelayed; }
        public void setDelayed(bool delay) { _isDelayed = delay; }
        
        
        public IExpressionType doOperation(IOperations o)
        {
            return o.doVarDefenition(this);
        }
    }
}