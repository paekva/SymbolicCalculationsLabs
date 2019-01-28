using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SymCal_2.Formats;
using SymCal_2.Formats.parser;
using SymCal_2.Operands;
using SymCal_2.Operators;
using SymCal_2.ToSort;
using SymCal_2.Unit;

namespace SymCal_2.Formats
{
    public class LatexParser : IParser
    {
        // FORMAT TO MATH MODEL PARSER
        public IExpressionType UnitImportFromFormatToModel(IFormat input, int listPosition)
        {
            Context ctx = Context.getInstance();
            ctx.getIn(input.getType().ToString() + listPosition); //спускаемся в контекст блока

            List<IExpressionType> unitExpressions = new List<IExpressionType>();

            for (int o = 0; o < input.getOperands().Count; o++) // преобразуем все выражения внутри блока
            {
                switch (input.getOperands()[o].getType())
                {
                    case Types.FuncDefinition:
                    case Types.WhileU:
                    case Types.ConditionU:
                        unitExpressions.Add(UnitImportFromFormatToModel(input.getOperands()[o], o));
                        break;
                    default:
                        unitExpressions.Add(ExpressionImportFromFormatToModel(input.getOperands()[o]));
                        break;
                }
            }

            // преобразуем выражение - условие
            IExpressionType options=null;
            if(input.getType()!=Types.FuncDefinition)
                options = input.getOptions()==null ? null : ExpressionImportFromFormatToModel(input.getOptions());

            ctx.getOut();
            
           // построение самого блока
            switch (input.getType())
            {
                case Types.GlobalU:
                    return new GlobalUnit(unitExpressions, listPosition);
                case Types.WhileU:
                    return new WhileUnit(unitExpressions, options, listPosition);
                case Types.FuncDefinition:
                    string name = input.getOptions().getOptions().getValue();
                
                    List<Variable> lst = new List<Variable>();
                    foreach (var v in input.getOptions().getOperands()) lst.Add(new Variable(v.getValue()));
                
                    FunctionDefenition fd = new FunctionDefenition(unitExpressions, name, lst, listPosition);
                    ctx.addFunction(name,ctx.getCurrPath(),fd);
                    return fd;
                default:
                    throw new Exception("Error in importing" + input.getType());
            }
        }

        public IExpressionType ExpressionImportFromFormatToModel(IFormat expression)
        {
            List<IExpressionType> operands = new List<IExpressionType>();

            Context ctx = Context.getInstance();
            if (expression.getOperands() != null)
            {
                foreach (var operand in expression.getOperands()) 
                    operands.Add(ExpressionImportFromFormatToModel(operand));
            }
            
            switch (expression.getType())
            {
                case Types.Variable:
                    Variable newVar = new Variable(expression.getValue());
                    ctx.addVariable(newVar.getValue(), ctx.getCurrPath(), null);
                    return newVar;
                case Types.Number:
                    return new Number(Convert.ToDouble(expression.getValue()));
                case Types.VarDefinition:
                    Equation ne = new Equation(operands, false);
                    ctx.changeVariable(((Variable) operands[0]).getValue(), ctx.getCurrPath(), ne);
                    return ne;
                case Types.SuspendedVarDefinition:
                    Equation nde = new Equation(operands, true);
                    ctx.changeVariable(((Variable) operands[0]).getValue(), ctx.getCurrPath(), nde);
                    return nde;
                case Types.List:
                    return new MathList(operands);
                case Types.Interval:
                    return new Interval(operands);
                case Types.FuncExpression:
                    return new Function(expression.getType(),operands, expression.getOptions().getValue());
                default:
                    return new Function(expression.getType(),operands, "");
                
            }
        }
        
        
        
        
        // STRING TO FORMAT PARSER
        public IFormat ParseInputToFormat(List<string> input, Types unitType, string outOptions)
        {
            LatexLexems ll = LatexLexems.getInstance();
            List<IFormat> unitExp = new List<IFormat>();
            int brackets=0;
            int startIndex=0;
            Types type = Types.GlobalU;
            string options="";
            
            for(int i=0;i<input.Count;i++)
            {
                foreach (var lex in ll.getUnitsLexem())
                {
                    if (matches(input[i], lex.getRegex()))
                    {
                        brackets++;
                        if (brackets == 1)
                        {
                            startIndex = i;
                            type = lex.getType();
                            options = getTokenValue(input[i], lex.getRegex(), "options");
                        }
                        break;
                    }
                }
                
                // Изначально в самом внешнем скопе
                if (brackets == 0)
                {
                    if(input[i]!="") unitExp.Add(ParseExpression(input[i]));
                    continue;
                }

                if (matches(input[i], "}}")) brackets--;

                if (brackets==0) // После операции вычитания, мы вернулись в самый внешний скоп
                {
                    string[] array = new string[i-startIndex-1];
                    input.CopyTo(startIndex+1,array,0,i-startIndex-1);
                    unitExp.Add(ParseInputToFormat(array.ToList(),type,options));
                }
            }

            IFormat c = outOptions =="" ? null:ParseExpression(outOptions);
            return new Latex(unitType,unitExp,c);
        }

        private bool matches(string str, string pattern)
        {
            Regex rgx = new Regex(pattern);
            return rgx.IsMatch(str);
        }

        private string getTokenValue(string str, string pattern, string token)
        {
            Regex rgx = new Regex(pattern);
            Match match = rgx.Match(str);
            return match.Groups[token].Value;
        }

        private IFormat ParseExpression(string str)
        {
            List<IFormat> operands = new List<IFormat>();
            LatexLexems ll = LatexLexems.getInstance();
            
            foreach (var def in ll.getDefinitionsLexem())
            {
                if (matches(str, def.getRegex()))
                {
                    IFormat name = ParseExpression(getTokenValue(str, def.getRegex(), "name"));
                    IFormat value = ParseExpression(getTokenValue(str, def.getRegex(), "expr"));
                    operands.Add(name);
                    operands.Add(value);
                    return new Latex(def.getType(),operands,null);
                }
            }
            
            if((operands = ParseMultiOperandsExpression(str,'+'))!=null) 
                return new Latex(Types.Addition,operands,null);
            if((operands = ParseMultiOperandsExpression(str,'-'))!=null) 
                return new Latex(Types.Substraction,operands,null);
            if((operands = ParseMultiOperandsExpression(str,'*'))!=null) 
                return new Latex(Types.Multiplication,operands,null);

            operands = new List<IFormat>();
            string[] strings;
            Lexem tmp=new Lexem(Types.Variable,"");
            int length = 0;
            
            foreach (var def in ll.getOperandsLexem())
            {
                Regex rgx = new Regex(def.getRegex());
                Match match = rgx.Match(str);
                if (match.Length > length)
                {
                    tmp = def;
                    length = match.Length;
                }
            }
            if(tmp.getRegex()=="") throw new Exception("Bad input, really! Follow the syntax");
            
            switch (tmp.getType())
            {
                case Types.Variable: 
                    return new Latex(str);
                case Types.Number:
                    return new Latex(Convert.ToDouble(str));
                case Types.Interval:
                case Types.List:
                {
                    str = str.Substring(1, str.Length - 2);
                    strings = str.Split(';');
                    foreach (var s in strings) operands.Add(ParseExpression(s));
                    return new Latex(tmp.getType(),operands,null);
                }
                case Types.FuncExpression:
                    IFormat funcName = ParseExpression(getTokenValue(str, tmp.getRegex(), "fr"));
                    string parametrs = getTokenValue(str, tmp.getRegex(), "sec");
                        
                    parametrs = parametrs.Substring(1, parametrs.Length - 2);
                    strings = parametrs.Split(',');
                    foreach (var s in strings) operands.Add(ParseExpression(s));
                    return new Latex(tmp.getType(),operands,funcName);
            }
            IFormat first = ParseExpression(getTokenValue(str, tmp.getRegex(), "fr"));
            IFormat second = ParseExpression(getTokenValue(str, tmp.getRegex(), "sec"));
            operands.Add(first);
            operands.Add(second);
            return new Latex(tmp.getType(),operands,null);
        }
        
        private List<IFormat> ParseMultiOperandsExpression(string exp, char splitSymbol)
        {
            List<IFormat> op = new List<IFormat>();
            List<String> expListed = exp.Split(splitSymbol).ToList();
            
            for(int i=0;i<expListed.Count;i++)
            {
                if (expListed.Count==1 || expListed[0]=="") return null;
                if (checkForBrakets(expListed[i]) && expListed.Count!=1) op.Add(ParseExpression(expListed[i]));
                else
                {
                    expListed[i] += splitSymbol+expListed[i + 1];
                    expListed.RemoveAt(i+1);
                    i--;
                }
            }
            return op;
        }
        
        private bool checkForBrakets(string str)
        {
            MatchCollection opener = new Regex(@"\(|{|\[").Matches(str);
            MatchCollection closer = new Regex(@"\)|}|\]").Matches(str);
            if (opener.Count - closer.Count == 0) return true;
            return false;
        }

        public IFormat UnitImportFromModelToFormat(IExpressionType input, int listPosition)
        {
            return null;
        }

        public IFormat ExpressionImportFromModelToFormat(IExpressionType expression)
        {
            return null;
        }
    }
}