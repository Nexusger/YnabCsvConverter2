using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using YnabCsvConverter.Interface;
using YnabCsvConverter.Interface.Model;

namespace YnabCsvConverter.Converter.Hypovereinsbank
{
    public class HypoGiroStatementConverter : StatementConverter
    {
        public override string NameOfStatement()
        {
            return "Hypovereinsbank Giro";
        }

        protected override YnabStatementLine ConvertToValidYnabStatement(string singleLine)
        {
            var fields = singleLine.Split(';').Select(t => t.RemoveBackTicks()).ToArray();
            var culture = CultureInfo.CreateSpecificCulture("de-DE");
            if (!DateTime.TryParse(fields[2], culture, DateTimeStyles.None,out var date))
            {
                Debug.WriteLine("Problem in creating ynabstatement");
            }
            var payee = "";
            var category = "";

            var memo = $"Converted! {fields[5]}, original currency was '{fields[7]}'";

            var inflow = 0f;
            var outflow = 0f;
            if (!float.TryParse(fields[6], out float value))
            {
                Debug.WriteLine($"Error converting Value {fields[6]}");
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

        protected override bool IsValidStatement(string singleStatementLine)
        {
            if (!singleStatementLine.Contains(';')) return false;
            var fields = singleStatementLine.Split(';').Select(t => t.RemoveBackTicks()).ToArray();
            if (fields.Length != 8) return false;
            if (!int.TryParse(fields[0], out int accountNumber)) return false;

            return true;

        }
    }
}
