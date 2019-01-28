using System.Collections.Generic;

namespace SymCal_2
{
    public interface IFormat
    {
        string getFormatName();
        
        Types getType();
        
        List<IFormat> getOperands();

        IFormat getOptions();

        string getValue();
    
    }
}