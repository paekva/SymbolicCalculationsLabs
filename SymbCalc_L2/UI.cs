using System;
using System.Collections.Generic;
using SymCal_2.Formats;

namespace SymCal_2
{
    public class UI
    {
        private static UI instance;
        private string syntax =
                "SYNTAX RULES\n\n"+
                "To define a var use '=', to define lazy calculation var use ':='" +
                "Equation 'variableName' = 'expression'" +
                "Operators: +, * \n" +
                "Brackets \\left( and \\right)" +
                "Division \\frac{numerator}{denumerator} \n" +
                "Root \\sqrt[root]{base} \n" +
                "Power {power}^{base} \n"  +
                "To define unit use {{ ... }}"
            ;
        private UI() {}
 
        public static UI getInstance()
        {
            if (instance == null)
                instance = new UI();
            return instance;
        }

        public List<string> getUserInput()
        {
            Console.WriteLine(syntax);
            Console.WriteLine("Input all the expressions u need. Divide them by Enter. Write 'End' to stop");
            
            string str;
            List<string> input = new List<string>();
            
            while ((str = Console.ReadLine()) != "End") input.Add(str);
            
            return input;
        }
        
        public void printOutput(List<string> exprs)
        {
            foreach (var e in exprs) Console.WriteLine(e);
        }
        
    }
}