using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using YnabCsvConverter.Interface;
using YnabCsvConverter.Interface.Model;

namespace YnabCsvConverter.Converter.DKB
{
    public class DkbGiroStatementConverter : StatementConverter
    {

        protected override bool IsValidStatement(string line)
        {
            if (line == "" || line == "\n") return false;
            if (line.StartsWith("\"Kontonummer")) return false;
            if (line.StartsWith("\"Von")) return false;
            if (line.StartsWith("\"Bis")) return false;
            if (line.StartsWith("\"Kontostand")) return false;
            if (line.StartsWith("\"Buchungstag")) return false;
            if (!line.Contains(';')) return false;

            var u = line.Split(';');
            var fields = u.Select(t => t.RemoveBackTicks()).ToArray();
            if (fields.Length <= 8) return false;
            if (fields[2] == "Abschluss") return false;

                return true;
        }

        protected override YnabStatementLine ConvertToValidYnabStatement(string line)
        {
            var fields = line.SplitByBackticks().ToArray();
            var culture = CultureInfo.CreateSpecificCulture("de-DE");
            if (!DateTime.TryParse(fields[1],culture,DateTimeStyles.None,out var date))
            {
                Debug.WriteLine("Problem in creating ynabstatement");
            }
            var payee = fields[3].Replace(",","");
            var category = "";

            var memo = $"{CONVERTERMARKER} {fields[4]}";

            var inflow = 0f;
            var outflow = 0f;
            if (!float.TryParse(fields[7], NumberStyles.Float ,culture,out float value))
            {
                Debug.WriteLine($"Error converting Value {fields[7]}");
            }
            if (value >= 0)
            {
                inflow = Math.Abs(value);
            }
            else
            {
                outflow = Math.Abs(value);
            }
            return new YnabStatementLine(date, payee, category, memo, inflow, outflow);
        }
        public override string NameOfStatement()
        {
            return "DKB Girokonto";
        }
    }
}
