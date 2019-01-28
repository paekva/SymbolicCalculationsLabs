using System;
using System.Collections.Generic;
using System.Xml;

namespace SymCal_2.Formats
{
    public class XML: IFormat
    {
        private readonly string formatName = "XML";
        private readonly Types type;
        private readonly string value;
        private readonly IFormat _options;
        private readonly List<IFormat> operands;
        
        public XML(Types expType, List<IFormat> operandList, IFormat options)
        {
            if(expType==Types.Number||expType==Types.Variable) throw new Exception();
            type = expType;
            operands = operandList;
            _options = options;
        }

        public XML(string val)
        {
            type = Types.Variable;
            value = val;
        }
        
        public XML(double val)
        {
            type = Types.Number;
            value = Convert.ToString(val);
        }

        
       public string getFormatName() {return formatName;}
        public Types getType() {return type;}
        public string getValue() { return value;}
        public List<IFormat> getOperands() { return operands; }
        public IFormat getOptions() { return _options; }

    }
}