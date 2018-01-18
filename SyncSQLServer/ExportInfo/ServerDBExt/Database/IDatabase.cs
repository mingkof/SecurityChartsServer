using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ServerDBExt.Database
{
    public interface IDatabase
    {
        int Execute(string commandText, IEnumerable parameters);
        object QueryValue(string commandText, IEnumerable parameters);
        List<Dictionary<string, string>> Query(string commandText, IEnumerable parameters);
        string GetStrValue(string commandText, IEnumerable parameters);
        bool DoEnsureOpen(Action<string> CB);
        void DoEnsureClose();

    }
}
