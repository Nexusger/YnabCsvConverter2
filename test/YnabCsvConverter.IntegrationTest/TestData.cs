using System;
using System.Collections.Generic;
using System.Text;
using YnabCsvConverter.Interface.Model;

namespace YnabCsvConverter.IntegrationTest
{
    internal static class TestData
    {
        internal static IEnumerable<YnabStatementLine> ComparisonData()
        {
            var result = new List<YnabStatementLine>();
            result.Add(new YnabStatementLine());
            return result;
        }
    }
}
