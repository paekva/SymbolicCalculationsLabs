using System;
using System.Collections.Generic;

namespace SymCal_2.Formats
{
    public class Latex: IFormat
    {
        private readonly string formatName = "Latex";
        private readonly Types type;
        private readonly string value;
        private readonly IFormat _options;
        private readonly List<IFormat> operands;

        public IExpressionType UnitImportFromFormatToModel(IFormat input, int listPosition) { return null; }
        public IExpressionType ExpressionImportFromFormatToModel(IFormat expression){ return null; }
        
        
        public Latex(Types expType, List<IFormat> operandList, IFormat options) // Constructor for complex exp
        {
            if(expType==Types.Number||expType==Types.Variable) throw new Exception();
            type = expType;
            operands = operandList;
            _options = options;
        }

        public Latex(string val) // Constructor for variables
        {
            type = Types.Variable;
            value = val;
        }
        
        public Latex(double val) // Constructor for numbers
        {
            type = Types.Number;
            value = Convert.ToString(val);
        }
        
        public string getFormatName() {return formatName;}

        public Types getType() {return type;}
        
        public string getValue() { return value;}
        public List<IFormat> getOperands() { return operands; }

        public IFormat getOptions() { return _options;}
        
    }
}