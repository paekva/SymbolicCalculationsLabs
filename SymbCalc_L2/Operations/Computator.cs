using System;
using System.Collections.Generic;
using SymCal_2.Operands;
using SymCal_2.Operations;
using SymCal_2.Operators;
using SymCal_2.ToSort;
using SymCal_2.Unit;
using WpfApplication1;

namespace SymCal_2
{
    public class Computator: IOperations
    {
        public Computator(){}

        private IExpressionType doPredefinedFunc(Function f)
        {
            Computator comp = new Computator();
            Simplification s = new Simplification();
            
            switch ( f.getName())
            {
                case "plot":
                    MathList mathList = (MathList)f.getOperands()[0];
                    Variable var = (Variable) f.getOperands()[1];
                    Interval interval = (Interval) f.getOperands()[2];

                    PlotModel plot = new PlotModel(mathList,var,interval);
                    plot.drawGraphics();
                    return f;
                case "integrate":
                    Integrate i = new Integrate((Variable)f.getOperands()[1]);
                    IExpressionType operand = f.getOperands()[0].doOperation(comp);
                    if (operand.getType() == Types.VarDefinition) return i.integrate((IMathExpression)operand.getOperands()[1]);
                    IExpressionType integrated = i.integrate((IMathExpression)operand);
                    return integrated.doOperation(comp);
            }

            List<IExpressionType> list = new List<IExpressionType>();
            foreach (var op in f.getOperands()) list.Add(op.doOperation(comp));
            
            return s.simplify(new Function(f.getType(),list,f.getName()));
        }

        private IExpressionType doCustomFunc(Function f, FunctionDefenition fd)
        {
            Context ctx = Context.getInstance();
            Computator comp = new Computator();
            Simplification s = new Simplification();
            
            // вычисления кастомных функций
            if(f.getOperands().Count!=fd.getParams().Count) throw new Exception("Not equal number of params");

            ctx.getIn(fd.getContext()); // get into right context

            bool[] wasBefore = new bool[fd.getParams().Count];
            IExpressionType[] expressions = new IExpressionType[fd.getParams().Count];
            
            for(int p=0;p<fd.getParams().Count;p++) // add params to scope
            {
                if (ctx.exists(fd.getParams()[p].getValue(), ctx.getCurrPath()) == -1)
                {
                    wasBefore[p] = false;
                    ctx.addVariable(fd.getParams()[p].getValue(),ctx.getCurrPath(),f.getOperands()[p]);
                }
                else
                {
                    wasBefore[p] = true;
                    expressions[p] = ctx.getVariableValue(fd.getParams()[p].getValue(), ctx.getCurrPath());
                    ctx.changeVariable(fd.getParams()[p].getValue(),ctx.getCurrPath(),f.getOperands()[p]);   
                }
            }

            List<IExpressionType> nl = new List<IExpressionType>();
            foreach (var op in fd.getOperands())
                nl.Add(op.doOperation(comp));
            
            for(int p=0;p<fd.getParams().Count;p++) //remove params from scope
            {
                if(!wasBefore[p]) ctx.removeVariable(fd.getParams()[p].getValue(),ctx.getCurrPath());
                else ctx.changeVariable(fd.getParams()[p].getValue(),ctx.getCurrPath(),expressions[p]);
            }
            
            //OUT 
            ctx.getOut();
            IExpressionType n = nl[nl.Count-1];
            return (IMathExpression) (n.getType()==Types.FuncExpression ? s.simplify((Function)n) : n);
        }
        public IExpressionType doFunction(Function f)
        {
            Context ctx = Context.getInstance();
            
            // определяем вид функции: встроенная или пользовательская
            IExpressionType me = ctx.getVariableValue(f.getName(), ctx.getCurrPath());
            if (me == null) return doPredefinedFunc(f);
            return doCustomFunc(f, (FunctionDefenition)me);
        }
        
        public IExpressionType doVariable(Variable v)
        {
            Context ctx = Context.getInstance();
            Computator comp = new Computator();
            IExpressionType varDef = ctx.getVariableValue(v.getValue(), ctx.getCurrPath());

            if (varDef == null) return v;
            if (varDef.getType() == Types.Number) return varDef;
            if (varDef.getType() == Types.Variable) return varDef.doOperation(comp);
            
            varDef = (Equation)varDef.doOperation(comp);
            return varDef.getOperands()[1];
        }

        public IExpressionType doUnit(IUnit u)
        {
            Computator comp = new Computator();
            Context ctx = Context.getInstance();
            List<IExpressionType> resultList = new List<IExpressionType>();
            
            ctx.getIn(u.getContext());
            
            foreach (var exp in u.getOperands()) 
                resultList.Add(exp.doOperation(comp));
            
            ctx.getOut();
            return new GlobalUnit(resultList,u.getPosition());
        }
        
        public IExpressionType doVarDefenition(Equation eq)
        {
            Context ctx = Context.getInstance();
            Computator comp = new Computator();
            
            // если опредлено как отложенные, то пропускаем
            if (eq.isDelayed())
            {
                eq.setDelayed(false);
                return eq;
            }
            
            // иначе вычисляем
            List<IExpressionType> lst = new List<IExpressionType>();
            IMathExpression operand = (IMathExpression)eq.getOperands()[1].doOperation(comp);
            lst.Add(eq.getOperands()[0]);
            lst.Add(operand);
            Equation ne = new Equation(lst,false);
            
            // change context
            ctx.changeVariable( ((Variable)ne.getOperands()[0]).getValue() ,ctx.getCurrPath(), ne);
            
            return ne;
        }
        
        public IExpressionType doFuncDefenition(FunctionDefenition fd) { return fd; }
    }
}