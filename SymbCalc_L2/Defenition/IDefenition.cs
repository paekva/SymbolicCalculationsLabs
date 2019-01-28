namespace SymCal_2
{
    public interface IDefenition: IExpressionType
    {
        bool isDelayed();

        void setDelayed(bool delay);
    }
}