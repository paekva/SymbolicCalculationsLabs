using System.Collections.Generic;
using SymCal_2.Operators;

namespace SymCal_2.Operations
{
    public class Integrate
    {
        private delegate IMathExpression IntegratedFunc(IMathExpression coeff);
        
        private List<IMathExpression> before;
        private List<IntegratedFunc> after;
        private Variable _param;
        public Integrate(Variable param)
        {
            before = new List<IMathExpression>();
            after = new List<IntegratedFunc>();
            _param = param;
            
            createBeforeArray();
            createAfterArray();
        }
        public IMathExpression integrate(IMathExpression e)
        {
            // Интеграл от суммы или вычитания
            if (e.getType() == Types.Addition || e.getType() == Types.Substraction)
            {
                List<IExpressionType> list = new List<IExpressionType>();
                foreach (var op in e.getOperands())
                {
                    list.Add(integrate((IMathExpression)op));
                }
                return new Function(e.getType(),list,"");
            }
            
            for (int i = 0; i < before.Count; i++)
            {
                if (e.compare(before[i])) return after[i](e);
            }
            return null;
        }
        private void createAfterArray()
        {
            after.Add(integateAbsoluteTerm); // => C * x
            after.Add(integateParamInPower); // => x^(n+1) / (n+1)
            after.Add(integateCos); // => sin(x)
        }

        private void createBeforeArray()
        {
            List<IExpressionType> list = new List<IExpressionType>();
            
            // exp = C
            before.Add(new Number(1));
            
            // exp = x^n
            list.Add(_param);
            list.Add(new Number(1));
            before.Add(new Function(Types.Power,list,""));
            
            // exp = cos(x)
            list = new List<IExpressionType>();
            list.Add(_param);
            before.Add(new Function(Types.FuncExpression,list,"cos"));
        }

        private IMathExpression integateAbsoluteTerm(IMathExpression exp)
        {
            List<IExpressionType> list = new List<IExpressionType>();
            list.Add((Number)exp);
            list.Add(_param);
            return new Function(Types.Multiplication, list, "");
        }
        
        private IMathExpression integateParamInPower(IMathExpression exp)
        {
            Number coeff = (Number)((Function) exp).getOperands()[1];
            
            List<IExpressionType> list = new List<IExpressionType>();
            list.Add(_param);
            list.Add(new Number(coeff.getValue()+1));
            Function nf = new Function(Types.Power,list,"");
            list = new List<IExpressionType>();
            list.Add(nf);
            list.Add(new Number(coeff.getValue()+1));
            
            return new Function(Types.Fraction,list,"");
        }
        
        private IMathExpression integateCos(IMathExpression coeff)
        {
            List<IExpressionType> list = new List<IExpressionType>();
            list.Add(_param);
            return new Function(Types.FuncExpression,list,"sin");
        }
    }
}