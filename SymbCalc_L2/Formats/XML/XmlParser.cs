using System;
using System.Collections.Generic;
using System.Xml;
using SymCal_2.Operators;
using SymCal_2.ToSort;

namespace SymCal_2.Formats
{
    public class XmlParser: IParser
    {
        public IFormat UnitImportFromModelToFormat(IExpressionType output, int listPosition)
        {
            List<IFormat> lst = new List<IFormat>();
            if(output.getType()==Types.FuncDefinition) lst.Add( new XML( ((FunctionDefenition)output).getName()) );
            foreach (var outp in output.getOperands())
            {
                if (outp.getType() == Types.FuncDefinition
                    || outp.getType() == Types.WhileU
                    || outp.getType() == Types.ConditionU) lst.Add(UnitImportFromModelToFormat(outp,0));
                else lst.Add(ExpressionImportFromModelToFormat(outp));
            }

            //IFormat options;
            return new XML(output.getType(),lst,null);
        }

        public IFormat ExpressionImportFromModelToFormat(IExpressionType outp)
        {
            switch (outp.getType())
            {
                case Types.Variable:
                    return new XML( ((Variable)outp).getValue());
                case Types.Number:
                    return new XML(Convert.ToDouble( ((Number)outp).getValue()));
                case Types.FuncExpression:
                    List<IFormat> list = new List<IFormat>();
                    foreach (var o in outp.getOperands()) list.Add(ExpressionImportFromModelToFormat(o));
                    return new XML(outp.getType(),list,new XML( ((Function)outp).getName() ));
                default:
                    List<IFormat> lst = new List<IFormat>();
                    foreach (var o in outp.getOperands()) lst.Add(ExpressionImportFromModelToFormat(o));
                    return new XML(outp.getType(),lst,null);
            }
        }


        public IExpressionType UnitImportFromFormatToModel(IFormat input, int listPosition)
        {
            return null;
        }
        public IExpressionType ExpressionImportFromFormatToModel(IFormat expression)
        {
            return null;
        }
        
        public XmlDocument ParseFormatToInput(IFormat exp)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement elem = doc.CreateElement("Algoritm");
            for( int i=0; i< exp.getOperands().Count; i++)
            {
                toXml(elem,doc,exp.getOperands()[i]);
            }
            doc.AppendChild(elem);
            return doc;
        }
        
        private void toXml(XmlElement root, XmlDocument doc, IFormat el)
        {
            switch (el.getType())
            {
                case Types.VarDefinition:
                {
                    XmlElement elem = doc.CreateElement("Equation");
                    
                    for( int i=0; i< el.getOperands().Count; i++)
                    {
                        toXml(elem,doc,el.getOperands()[i]);
                    }
                    
                    root.AppendChild(elem);
                    break;
                }
                case Types.Addition:
                {
                    XmlElement elem = doc.CreateElement("Sum");
                    
                    for( int i=0; i< el.getOperands().Count; i++)
                    {
                        toXml(elem,doc,el.getOperands()[i]);
                    }
                    
                    root.AppendChild(elem);
                    break;
                }
                case Types.Multiplication:
                {
                    XmlElement elem = doc.CreateElement("Mul");
                    
                    for( int i=0; i< el.getOperands().Count; i++)
                    {
                        toXml(elem,doc,el.getOperands()[i]);
                    }
                    
                    root.AppendChild(elem);
                    break;
                }
                case Types.Fraction:
                {
                    XmlElement elem = doc.CreateElement("Fraction");
                    XmlElement numerator = doc.CreateElement("Numerator");
                    toXml(numerator,doc,el.getOperands()[0]);
                    XmlElement denumerator = doc.CreateElement("Denumerator");
                    toXml(denumerator,doc,el.getOperands()[1]);
                    
                    elem.AppendChild(numerator);
                    elem.AppendChild(denumerator);
                    root.AppendChild(elem);
                    break;
                }
                case Types.Power:
                {
                    XmlElement elem = doc.CreateElement("Power");
                    XmlElement numerator = doc.CreateElement("Base");
                    toXml(numerator,doc,el.getOperands()[0]);
                    XmlElement denumerator = doc.CreateElement("Value");
                    toXml(denumerator,doc,el.getOperands()[1]);
                    
                    elem.AppendChild(numerator);
                    elem.AppendChild(denumerator);
                    root.AppendChild(elem);
                    break;
                }
                case Types.Root: 
                {
                    XmlElement elem = doc.CreateElement("Sqrt");
                    XmlElement numerator = doc.CreateElement("Base");
                    toXml(numerator,doc,el.getOperands()[0]);
                    XmlElement denumerator = doc.CreateElement("Value");
                    toXml(denumerator,doc,el.getOperands()[1]);
                    
                    elem.AppendChild(numerator);
                    elem.AppendChild(denumerator);
                    root.AppendChild(elem);
                    break;
                }
                case Types.Number:
                case Types.Variable:
                {
                    XmlElement elem = doc.CreateElement("Operand");
                    elem.InnerText = el.getValue();
                    root.AppendChild(elem);
                    break;
                }
                case Types.FuncDefinition:
                    /*XmlElement elem1 = doc.CreateElement(el.getOperands()[0].getValue());
                    for( int i=1; i< el.getOperands().Count; i++)
                    {
                        toXml(elem1,doc,el.getOperands()[i]);
                    }
                    root.AppendChild(elem1);*/
                    break;
                case Types.FuncExpression:
                    XmlElement elem2 = doc.CreateElement(el.getOperands()[0].getValue());
                    for( int i=1; i< el.getOperands().Count; i++)
                    {
                        toXml(elem2,doc,el.getOperands()[i]);
                    }
                    root.AppendChild(elem2);
                    break;
                default: throw new Exception();
            }
        }
    }
}