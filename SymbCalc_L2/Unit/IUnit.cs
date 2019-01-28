namespace SymCal_2
{
    public interface IUnit: IExpressionType
    {
        string getContext();
        
        IExpressionType getCondition();

        int getPosition();
    }
}