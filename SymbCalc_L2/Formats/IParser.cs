namespace SymCal_2
{
    public interface IParser
    {
        IExpressionType UnitImportFromFormatToModel(IFormat input, int listPosition);
        IExpressionType ExpressionImportFromFormatToModel(IFormat expression);
        
        IFormat UnitImportFromModelToFormat(IExpressionType input, int listPosition);
        IFormat ExpressionImportFromModelToFormat(IExpressionType expression);
    }
}