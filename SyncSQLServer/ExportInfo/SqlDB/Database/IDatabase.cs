using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ServerDBExt.Database
{
    public interface IDatabase
    {
        int Execute(string commandText, IEnumerable parameters, bool FinallyClose);
        object QueryValue(string commandText, IEnumerable parameters, bool FinallyClose);
        List<Dictionary<string, string>> Query(string commandText, IEnumerable parameters, bool FinallyClose);
        string GetStrValue(string commandText, IEnumerable parameters, bool FinallyClose);
        bool DoEnsureOpen(Action<string> CB);
        void DoEnsureClose();

    }
}
