using System;
using System.Collections.Generic;
using SymCal_2.Operators;

namespace SymCal_2.Operations
{
    public class Simplification
    {
        public Simplification(){}

        public IMathExpression simplify(Function f)
        {
            List<IExpressionType> list = simplifyOperands(f);
            
            Function fn = new Function(f.getType(),list,f.getName());
            
            switch (fn.getType())
            {
                case Types.Addition:
                    fn = removeSameFoldedType(fn, Types.Addition);
                    IMathExpression add = similar(fn, Types.Multiplication, Types.Addition);
                    return add;
                case Types.Multiplication:
                    fn = removeSameFoldedType(fn, Types.Multiplication);
                    fn = bracketsOpener(fn);
                    return similar(fn, Types.Power, Types.Multiplication);
                case Types.Substraction:
                    bool result = true;
                    double res;
                    foreach (var o in fn.getOperands()) result = result && o.getType() == Types.Number;
                    if (result)
                    {
                        res = ((Number) fn.getOperands()[0]).getValue();
                        for (int i = 1; i < fn.getOperands().Count; i++) res -= ((Number)fn.getOperands()[i]).getValue();
                        return new Number(res);
                    }
                    return fn;
                default:
                    return simplifyFunction(fn);
            }
        }
        
        
        private List<IExpressionType> simplifyOperands(IMathExpression m)
        {
            List<IExpressionType> lm = new List<IExpressionType>();
            foreach (var o in m.getOperands())
            {
                if(o.getType()==Types.FuncExpression) lm.Add(simplify((Function)o));
                else lm.Add(o);
            }

            return lm;
        }
        
        private IMathExpression simplifyFunction(Function e)
        {
            if (e.getOperands()[0].getType()==Types.Number && e.getOperands()[1].getType()==Types.Number)
                {
                    Double first = ((Number) e.getOperands()[0]).getValue();
                    Double second = ((Number) e.getOperands()[1]).getValue();
                    switch (e.getType())
                    {
                        case Types.Fraction:
                            Console.WriteLine("Found a fraction");
                            return new Number(first / second);
                        case Types.Root:
                            Console.WriteLine("Found a root");
                            return new Number(Math.Sqrt(second)); // ИЗВЛЕКАЕТ КВАДРАТНЫЙ КОРЕНЬ А НЕ НУЖНЫЙ
                        case Types.Power:
                            Console.WriteLine("Found a power");
                            return new Number(Math.Pow(first,second));
                    }
                }
                else if(e.getType()==Types.Fraction) return new Number(1);
            return e;
        }
        
        private Function removeSameFoldedType(Function e, Types type)
        {
            List<IExpressionType> op = new List<IExpressionType>();
            IExpressionType add=null;
            for(int a=0;a<e.getOperands().Count;a++)
            {
                if (e.getOperands()[a].getType() == type)
                {
                    add = (IMathExpression)e.getOperands()[a];
                    break;
                }

                if (a == e.getOperands().Count - 1) return e;
            }

            e.getOperands().Remove(add);
            op.AddRange(add.getOperands());
            op.AddRange(e.getOperands());
            return new Function(Types.Addition, op, "");
        }
        private Function bracketsOpener(Function e){
            IExpressionType add=null;
            foreach (var op in e.getOperands())
            {
                if (op.getType() == Types.Addition)
                {
                    add = op;
                    break;
                }
            }

            if (add == null) return e;
            
            e.getOperands().Remove(add);
            List<IExpressionType> mulList = new List<IExpressionType>();
            Simplification simplification = new Simplification();
            
            foreach (var a in add.getOperands())
            {
                List<IExpressionType> list = new List<IExpressionType>();
                list.AddRange(e.getOperands());
                list.Add(a);
                IMathExpression mul = new Function(Types.Multiplication,list,"");
                mul = simplification.simplify((Function)mul);
                mulList.Add(mul);
            }
            
            return new Function(Types.Addition, mulList,"");
        }
        private IMathExpression similar(IMathExpression e, Types innerType, Types outerType) {
            bool isAdd = outerType == Types.Addition;
            List<Helper> tmp = repeatatives(e, isAdd);

            if (tmp.Count == e.getOperands().Count) return e;
            
            List<IExpressionType> list = new List<IExpressionType>();
            foreach (var h in tmp)
            {
                if (h.count==1)
                {
                    list.Add(h.expression);
                    continue;
                }
                
                List<IExpressionType> operands = new List<IExpressionType>();
                operands.Add(h.expression);
                operands.Add(new Number(h.count));
                list.Add(new Function(innerType,operands,""));
            }

            if (list.Count == 1) return (IMathExpression)list[0];
            return new Function(outerType,list,"");
        }
        private List<Helper> repeatatives(IMathExpression e, bool isAdd){
            List<Helper> lh = new List<Helper>();
            foreach (var op in e.getOperands())
            {
                IMathExpression tmpExpression=(IMathExpression)op;
                int tmpCount=1;
                
                for(int h=0;h<lh.Count;h++)
                {
                    IMathExpression first = (IMathExpression)op;
                    IMathExpression second = lh[h].expression;
                    
                    if (first.compare(second))
                    {
                        if (op.getType() == Types.Number && isAdd)
                        {
                            tmpCount = 1;
                            tmpExpression = new Number(Convert.ToDouble( ((Number)first).getValue() )
                                                       + Convert.ToDouble( ((Number)second).getValue() ));
                        }
                        else if (op.getType() == Types.Number && !isAdd)
                        {
                            tmpCount = 1;
                            tmpExpression = new Number(Convert.ToDouble( ((Number)first).getValue() )
                                                       * Convert.ToDouble( ((Number)second).getValue() ));
                        }
                        else
                        {
                            tmpCount++;
                            tmpExpression = first;
                        }
                        
                        lh.RemoveAt(h);
                        break;
                    }
                }

                Helper n = new Helper();
                n.expression = tmpExpression;
                n.count = tmpCount;
                lh.Add(n);
            }

            return lh;
        }
    }

    struct Helper
    {
        public IMathExpression expression;
        public double count;

    }
}