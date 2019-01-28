using System.Collections.Generic;

namespace SymCal_2.Operators
{
    public class Number : IMathExpression
    {
        private double value;
        
        
        public Number(double val) { value = val;}
        public Types getType() {return Types.Number;}
        public double getValue() {return value;}
        public List<IExpressionType> getOperands() {return null;}
        
        

        public IExpressionType doOperation(IOperations o){ return this; }

        public bool compare(IMathExpression e)
        {
            if (getType() != e.getType()) return false;
            return true;
        }
    }
}