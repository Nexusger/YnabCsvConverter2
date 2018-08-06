using System;
using System.Collections.Generic;
using YnabCsvConverter.Interface.Model;

namespace YnabCsvConverter.Interface
{
    public interface IStatementConverter
    {
        IEnumerable<YnabStatementLine> GetConvertedStatements();
        IEnumerable<YnabStatementLine> GetConvertedStatements(DateTime oldestStatementToIncludeFilter);
    }
}
