using System.Collections.Generic;

namespace SymCal_2.Operators
{
    public class Variable : IMathExpression
    {
        private string value;
        
        
        public Variable(string val) { value = val; }
        public Types getType() {return Types.Variable;} 
        public string getValue() {return value;} 
        public List<IExpressionType> getOperands() {return null;}
        
        
        public IExpressionType doOperation(IOperations o) { return o.doVariable(this); }
        
        public bool compare(IMathExpression e)
        {
            if (getType() != e.getType()) return false;
            if (getValue() == ((Variable) e).getValue()) return true;
            return false;
        }
    }
}