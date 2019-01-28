using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OxyPlot;
using SymbCalc_L2;
using SymCal_2;
using SymCal_2.Operands;
using SymCal_2.Operators;

namespace WpfApplication1
{
    public class PlotModel
    {
        private List<DataPoint>[] _pointsArrays;
        private MathList _mathList;
        private Variable _variable;
        private Interval _interval;
        private int stepCount = 100;
        private double step;

        public PlotModel(MathList mathList, Variable var, Interval interval)
        {
            _pointsArrays = new List<DataPoint>[mathList.getOperands().Count];
            for(int p=0;p<_pointsArrays.Length;p++) _pointsArrays[p] = new List<DataPoint>();
            _mathList = mathList;
            _variable = var;
            
            _interval = interval;
            double length = ((Number) _interval.getOperands()[1]).getValue()
                                                 - ((Number) _interval.getOperands()[0]).getValue();
            step = length / stepCount;
            
            calculatePoints();
        }

        public void drawGraphics()
        {
            Form form1 = new Form1(_pointsArrays);
            Application.Run(form1);
        }
        
        private void calculatePoints()
        {
            Computator cmp = new Computator();
            
            for(int n=0;n< _mathList.getOperands().Count;n++)
            {
                double x, y;
                string funcName = ((Variable) _mathList.getOperands()[n]).getValue();
                double start = ((Number) _interval.getOperands()[0]).getValue();
                
                for (int j = 0; j < stepCount; j++)
                {
                    List<IExpressionType> lst = new List<IExpressionType>();
                    lst.Add(new Number(start));
                    Function tm = new Function(Types.FuncExpression, lst, funcName);
                    
                    x = start;
                    y = ((Number)tm.doOperation(cmp)).getValue();
                    
                    start += step;
                    
                    _pointsArrays[n].Add(new DataPoint(x,y));
                }
            }
            
        }
    }
}