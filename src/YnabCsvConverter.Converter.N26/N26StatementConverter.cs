using System;
using System.Globalization;
using System.Linq;
using YnabCsvConverter.Interface;
using YnabCsvConverter.Interface.Model;

namespace YnabCsvConverter.Converter.N26
{
    public class N26StatementConverter : StatementConverter
    {
        public override string NameOfStatement()
        {
            return "N26-Giro";
        }

        protected override YnabStatementLine ConvertToValidYnabStatement(string singleLine)
        {
            //"Datum","Empfänger","Kontonummer","Transaktionstyp","Verwendungszweck","Kategorie","Betrag (EUR)","Betrag (Fremdwährung)","Fremdwährung","Wechselkurs"

            var content = singleLine.SplitByBackticks().ToArray();

            var date = DateTime.Parse(content[0]);
            var payee = content[1];
            var memo = $"{CONVERTERMARKER} {content[4]}";
            var inflow = 0f;
            var outflow = 0f;
            var value = float.Parse(content[6], CultureInfo.InvariantCulture);
            if (value > 0)
            {
                inflow = Math.Abs(value);
            }
            else
            {
                outflow = Math.Abs(value);
            }
            return new YnabStatementLine(date, payee, "", memo, inflow, outflow);
        }

        protected override bool IsValidStatement(string singleStatementLine)
        {
            if (string.IsNullOrEmpty(singleStatementLine))
            {
                return false;
            }
            var t = singleStatementLine.Replace("\",\"","³").Split('³');
            if (t.Length != 10)
            {
                return false;
            }
            if (singleStatementLine.Contains("Datum"))
            {
                return false;
            }
            return true;
        }
    }
}