using System;
using System.Diagnostics;
using System.Linq;
using YnabCsvConverter.Interface;
using YnabCsvConverter.Interface.Model;

namespace YnabCsvConverter.Converter.DKB
{
    public class DkbCreditcardStatementConverter : StatementConverter
    {
        protected override bool IsValidStatement(string line)
        {
            return (line.StartsWith("\"Nein") || line.StartsWith("\"Ja") || line.StartsWith("Nein"));
        }

        protected override YnabStatementLine ConvertToValidYnabStatement(string line)
        {
            var fields = line.SplitByBackticks().ToArray();
            if (!DateTime.TryParse(fields[2], out var date))
            {
                Debug.WriteLine("Problem in creating ynabstatement");
            }
            var payee = GetPayee(line);
            var category = "";
            var nativeCurrency = string.Empty;
            if (!string.IsNullOrEmpty(fields[5]))
            {
                nativeCurrency = $", Fremdwährung: {fields[5]}";

            };
            var memo = $"Converted! Original: {fields[3]}{nativeCurrency}";

            var inflow = 0f;
            var outflow = 0f;
            if (!float.TryParse(fields[4], out float value))
            {
                Debug.WriteLine($"Error converting Value {fields[4]}");
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

        private static string GetPayee(string line)
        {
            var fields = line.Split(';');
            if (fields[3].StartsWith("\"1,75") || fields[3].StartsWith("1,75"))
            {
                return "DKB";
            }
            return "";
        }

        public override string NameOfStatement()
        {
            return "DKB Kreditkarte";
        }

        protected override void ConvertStatements()
        {
            int counter = 0;
            foreach (var singleLine in InputStatements)
            {
                if (IsValidStatement(singleLine))
                {
                    counter++;
                    ConvertedStatements.Add(ConvertToValidYnabStatement(singleLine));
                }
            }

            Confidence = (100f / (float)InputStatements.Count()) * (float)counter;
        }
    }
}
