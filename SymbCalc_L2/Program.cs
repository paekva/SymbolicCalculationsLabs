using System;
using System.Collections.Generic;
using System.Xml;
using SymCal_2;
namespace SymbCalc_L2
{
    class Program 
    {
        static void Main(string[] args)
        {
            UI ui = UI.getInstance();
            List<string> str = ui.getUserInput();
            
            // Parsing STRING -> FORMAT -> MODEL
            ParseProcessor parseProcessor = ParseProcessor.getInstance();
            IExpressionType mathmodel = parseProcessor.parseFromInputToMathModel(str);
            
            // CALCULATIONS
            Context ct = Context.getInstance();
            Computator computator = new Computator();
            IExpressionType computed = mathmodel.doOperation(computator);

            // PARSING MODEL -> FORMAT -> STRING
            /*XmlDocument res = parseProcessor.parseToInputFromMathModel(computed);
            Console.WriteLine(res.InnerXml);*/
        }
    }
}