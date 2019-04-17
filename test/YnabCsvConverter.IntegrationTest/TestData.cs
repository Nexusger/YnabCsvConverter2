using System;
using System.Collections.Generic;
using System.Text;
using YnabCsvConverter.Interface;
using YnabCsvConverter.Interface.Model;

namespace YnabCsvConverter.IntegrationTest
{
    internal static class TestData
    {
        internal static IEnumerable<YnabStatementLine> ComparisonData()
        {
            var result = new List<YnabStatementLine>();
            result.Add(new YnabStatementLine(DateTime.Parse("2018-09-24"), "CsvConverter äüöß", "", $"{StatementConverter.CONVERTERMARKER} Positive üäöß .,;", 123.45f, 0f));
            result.Add(new YnabStatementLine(DateTime.Parse("2018-10-10"), "CsvConverter äüöß", "", $"{StatementConverter.CONVERTERMARKER} Negative üäöß .,;", 0f, 123.45f));
            return result;
        }
    }
}
