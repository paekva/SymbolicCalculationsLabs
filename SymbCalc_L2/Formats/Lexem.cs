namespace SymCal_2.Formats.parser
{
    public struct Lexem
    {
        private Types _type;
        private string _regex;
        
        public Lexem(Types type, string regex)
        {
            _type = type;
            _regex = regex;
        }

        public string getRegex()
        {
            return _regex;
        }
        
        public Types getType()
        {
            return _type;
        }
    }
}