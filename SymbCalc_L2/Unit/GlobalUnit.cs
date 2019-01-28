using System.Collections.Generic;

namespace SymCal_2.Unit
{
    public class GlobalUnit: IUnit
    {
        private List<IExpressionType> _expressions;
        private string _ctx;
        private int _position;
        
        public GlobalUnit(List<IExpressionType> expressions, int position)
        {
            _expressions = expressions;
            _ctx = getType().ToString() + position;
            _position = position;
        }
        
        
        public List<IExpressionType> getOperands() { return _expressions; }
        public Types getType(){ return Types.GlobalU; }
        public string getContext() { return _ctx; }
        public IExpressionType getCondition() { return null; }
        public int getPosition(){ return _position; }
        
        
        public IExpressionType doOperation(IOperations o) { return o.doUnit(this); }
    }
}