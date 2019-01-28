using System;

namespace SymCal_2
{
    [Flags]
    public enum Types
    {
        GlobalU,
        ConditionU,
        WhileU,
        //
        FuncDefinition,
        VarDefinition,
        SuspendedVarDefinition,
        //
        Number,
        Variable,
        List,
        Interval,
        //
        Fraction,
        Root,
        Power,
        FuncExpression,
        //
        Addition,
        Multiplication,
        Substraction,
    }
}