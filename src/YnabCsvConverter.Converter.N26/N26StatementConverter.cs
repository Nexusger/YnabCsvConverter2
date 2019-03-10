using System;
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
            //"2018-08-27","Jan Fuchs","DE15100110012622977805","Gutschrift","Ausflug","Gutschriften","20.0","","",""
            var content = singleLine.Replace("\"", "").Split(',');
            var date = DateTime.Parse(content[0]);
            var payee = content[1];
            var memo = content[4];
            var inflow = 0f;
            var outflow = 0f;
            var value = float.Parse(content[6]);
            if (value > 0)
            {
                inflow = value;
            }
            else
            {
                outflow = value;
            }
            return new YnabStatementLine(date, payee, "", memo, inflow, outflow);
        }

        protected override bool IsValidStatement(string singleStatementLine)
        {
            if (string.IsNullOrEmpty(singleStatementLine))
            {
                return false;
            }
            if (singleStatementLine.Split(',').Length != 10)
            {
                return false;
            }
            if (!singleStatementLine.StartsWith("\"Datum\""))
            {
                return false;
            }
            return true;
        }
    }
}