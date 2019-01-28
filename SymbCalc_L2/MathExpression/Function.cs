using System.Collections.Generic;
using SymCal_2.Operations;

namespace SymCal_2.Operators
{
    public class Function : IMathExpression
    {
        private List<IExpressionType> operands;
        private Types type;
        private string _name;

        public Function(Types t, List<IExpressionType> op, string name)
        {
            type = t;
            operands = op;
            _name = name;
        }
        
        
        public Types getType() {return type;}
        public string getName() {return _name;}
        public List<IExpressionType> getOperands() {return operands;}
        
        
        
        public IExpressionType doOperation(IOperations o)
        {
            return o.doFunction(this);
        }
        
        public bool compare(IMathExpression e)
        {
            if (getType() != e.getType()) return false;

            bool result=true;
            if (getType() != Types.Addition || getType() != Types.Multiplication)
            {
                for (int i=0;i<getOperands().Count;i++)
                {
                    result = result && ((IMathExpression)getOperands()[i]).compare((IMathExpression)e.getOperands()[i]);
                }
                
                return result;
            }
            
            if (getOperands().Count != e.getOperands().Count) return false;
            bool[] flags = new bool[getOperands().Count];
            foreach (var outer in e.getOperands())
            {
                for (int k = 0; k < getOperands().Count; k++)
                {
                    if (!flags[k])
                    {
                        flags[k] = ((IMathExpression)outer).compare((IMathExpression)getOperands()[k]);
                        if(flags[k]) break;
                    }

                    if (k == getOperands().Count - 1) return false;
                }
            }

            foreach (var b in flags)
            {
                if (!b) return false;
            }
            return true;
        }
        
        
    }
}