using System.Collections.Generic;
using System.Xml;
using SymCal_2.Formats;

namespace SymCal_2
{
    public class ParseProcessor
    {
        private static ParseProcessor instance;
        private ParseProcessor(){}

        public static ParseProcessor getInstance()
        {
            if(instance==null)
                instance=new ParseProcessor();
            return instance;
        }
        public IExpressionType parseFromInputToMathModel(List<string> input)
        {
            LatexParser ll = new LatexParser();
            IFormat result = ll.ParseInputToFormat(input,Types.GlobalU, "");
            IExpressionType mathModel = ll.UnitImportFromFormatToModel(result, 0);
            return mathModel;
        }
        
        public XmlDocument parseToInputFromMathModel(IExpressionType model)
        {
            XmlParser xp = new XmlParser();
            IFormat nf = xp.UnitImportFromModelToFormat((IUnit)model,0);
            return xp.ParseFormatToInput(nf);
        }
    }
}