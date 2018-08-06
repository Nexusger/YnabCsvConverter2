using System;
using System.Diagnostics;
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

            var fields = line.Split(';').Select(t => t.RemoveBackTicks()).ToArray();
            if (fields.Length <= 8) return false;
            if (fields[2] == "Abschluss") return false;

                return true;
        }

        protected override YnabStatementLine ConvertToValidYnabStatement(string line)
        {
            //"29.09.2017";"29.09.2017";"Lastschrift";"PayPal Europe S.a.r.l. et Cie S.C.A";"PP.1044.PP . SPOTIFY, Ihr Einkauf bei SPOTIFY";"DE88500700100175526303";"DEUTDEFFXXX";"-4,99";"LU96ZZZ0000000000000000058                       ";"4G2J224RTX66J         ";"";
            var fields = line.Split(';').Select(t => t.RemoveBackTicks()).ToArray();
            if (!DateTime.TryParse(fields[1], out var date))
            {
                Debug.WriteLine("Problem in creating ynabstatement");
            }
            var payee = fields[3].Replace(",","");
            var category = "";

            var memo = $"Converted! {fields[2]} From {fields[3]} {fields[4]}";

            var inflow = 0f;
            var outflow = 0f;
            if (!float.TryParse(fields[7], out float value))
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
