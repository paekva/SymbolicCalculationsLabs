
using System.Collections.Generic;
using SymCal_2.Operators;

namespace SymCal_2.ToSort
{
    public class FunctionDefenition: IDefenition
    {
        private List<IExpressionType> _expressions;
        private List<Variable> _params;
        private string _ctx;
        private string _name;

        public FunctionDefenition(List<IExpressionType> expressions,string name, List<Variable> prms, int position)
        {
            _params = prms;
            _expressions = expressions;
            _ctx = getType().ToString() + position;
            _name = name;
        }
        
        
        public List<IExpressionType> getOperands() { return _expressions; }
        public List<Variable> getParams() { return _params; }
        public Types getType() { return Types.FuncDefinition; }
        public string getContext() { return _ctx; }
        public string getName() { return _name; }
        public bool isDelayed(){ return false; }
        public void setDelayed(bool delay){}
        
        
        public IExpressionType doOperation(IOperations o)
        {
            return o.doFuncDefenition(this);
        }
    }
}