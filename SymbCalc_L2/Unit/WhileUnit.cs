using System.Collections.Generic;

namespace SymCal_2.ToSort
{
    public class WhileUnit: IUnit
    {
        private List<IExpressionType> _expressions;
        private IExpressionType _condition;
        private string _ctx;
        private int _position;
        
        public WhileUnit(List<IExpressionType> expressions, IExpressionType condition, int position)
        {
            _expressions = expressions;
            _ctx = getType().ToString() + position;
            _condition = condition;
            _position = position;
        }
        
        public List<IExpressionType> getOperands()
        {
            return _expressions;
        }
        public Types getType()
        {
            return Types.WhileU;
        }
        public IExpressionType doOperation(IOperations o)
        {
            return o.doUnit(this);
        }

        public string getContext()
        {
            return _ctx;
        }
        
        public IExpressionType getCondition()
        {
            return _condition;
        }
        
        public int getPosition()
        {
            return _position;
        }
    }
}