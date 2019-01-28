using System.Collections;
using System.Collections.Generic;
using SymCal_2.ToSort;

namespace SymCal_2
{
    public class Context
    {
        private static Context instance;
        private static string currentPath;
        private List<Table> variables;

        private Context()
        {
            variables = new List<Table>();
            currentPath = "";
        }
        
        public static Context getInstance()
        {
            if (instance == null)
                instance = new Context();
            return instance;
        }

        public string getCurrPath() { return currentPath; }
        public void setCurrPath(string path) { currentPath = path; }
        public void getOut()
        {
            int i = currentPath.LastIndexOf("'");
            if(i>=0) currentPath = currentPath.Substring(0,i);
        }
        
        public void getIn(string context) { currentPath += "'"+context; }
        
        public void addVariable(string variableName, string currentPath, IExpressionType expression)
        {
            if (exists(variableName, currentPath)==-1)
            {
                Table nt = new Table();
                nt.name = variableName;
                nt.path = currentPath;
                nt.varReference = expression;
                variables.Add(nt);
            }
        }
        
        public void removeVariable(string variableName, string currentPath)
        {
            int index = exists(variableName, currentPath);
            if (index!=-1) variables.RemoveAt(index);
        }
        
        public void addFunction(string fucntionName, string currentPath, IExpressionType func)
        {
            addVariable(fucntionName,currentPath,func);
        }
        
        public void changeVariable(string name, string currentPath, IExpressionType exp)
        {
            if (exists(name, currentPath)!=-1)
            {
                Table tmp = variables.Find(x => x.name == name && x.path == currentPath);
                variables.Remove(tmp);
                tmp.varReference = exp;
                variables.Add(tmp);
            }
        }
        
        public IExpressionType getVariableValue(string variableName, string currentPath)
        {
            string currentCtx = getCurrPath();
            IExpressionType tmp = variables.Find(x => x.name == variableName && x.path == getCurrPath()).varReference;
            
            while (tmp==null)
            {
                getOut();
                if (getCurrPath() == "")
                {
                    setCurrPath(currentCtx);
                    return null;
                }
                tmp = variables.Find(x => x.name == variableName && x.path == getCurrPath()).varReference;
            }

            return tmp;
        }
        
        public int exists(string variableName, string currentPath)
        {
            int b = variables.FindIndex(x => x.name == variableName && x.path == currentPath);
            return b;
        }
        
    }
}