using System.Collections.Generic;

namespace SymCal_2.Formats.parser
{
    public class LatexLexems
    {
        private static LatexLexems instance;
        
        private static List<Lexem> _operands;
        private static List<Lexem> _definitions;
        private static List<Lexem> _units;

        private LatexLexems()
        {
            _operands = new List<Lexem>();
            _definitions = new List<Lexem>();
            _units = new List<Lexem>();
            
            _operands.Add(new Lexem(Types.FuncExpression, @"(?<fr>[a-zA-Z_$][a-zA-Z0-9_$]*)(?<sec>\(\S*\))"));
            _operands.Add(new Lexem(Types.List,@"\[\S*\]" ));
            _operands.Add(new Lexem(Types.Interval,@"\(\S*\)" ));
            _operands.Add(new Lexem(Types.Fraction, @"\\frac{(?<fr>\S*)}{(?<sec>\S*)}"));
            _operands.Add(new Lexem(Types.Power, @"{(?<fr>\S*)}\^{(?<sec>\S*)}"));
            _operands.Add(new Lexem(Types.Root, @"\\sqrt\[(?<fr>\S*)\]{(?<sec>\S*)}")); 
            _operands.Add(new Lexem(Types.Variable, @"(?<variable>[a-zA-Z_$][a-zA-Z0-9_$]*)"));
            _operands.Add(new Lexem(Types.Number, @"(?<int>^-?[0-9]\d*(\.\d+)?$)"));
            
            _definitions.Add(new Lexem(Types.SuspendedVarDefinition, @"(?<name>[a-zA-Z_$][a-zA-Z0-9_$]*):=(?<expr>\S*)"));
            _definitions.Add(new Lexem(Types.VarDefinition, @"(?<name>[a-zA-Z_$][a-zA-Z0-9_$]*)=(?<expr>\S*)"));
            
            _units.Add(new Lexem(Types.WhileU, @"while\((?<options>\S*)\){{"));
            _units.Add(new Lexem(Types.ConditionU, @"if\((?<options>\S*)\){{"));
            _units.Add(new Lexem(Types.FuncDefinition, @"_(?<options>[a-zA-Z_$][a-zA-Z0-9_$]*\([a-zA-Z_$|,]*\))=>{{"));
            _units.Add(new Lexem(Types.GlobalU, @"(?<options>){{"));

        }

        public static LatexLexems getInstance()
        {
            if (instance == null)
                instance = new LatexLexems();
            return instance;
        }

        public List<Lexem> getOperandsLexem()
        {
            return _operands;
        }
        
        public List<Lexem> getDefinitionsLexem()
        {
            return _definitions;
        }
        
        public List<Lexem> getUnitsLexem()
        {
            return _units;
        }
    }
}