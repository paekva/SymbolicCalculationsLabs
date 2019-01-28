namespace SymCal_2
{
    public interface IMathExpression: IExpressionType
    {
        bool compare(IMathExpression e);
    }
}