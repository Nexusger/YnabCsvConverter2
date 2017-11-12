using System;
using System.Diagnostics;
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
            //Kontonummer;Buchungsdatum;Valuta;Empfaenger 1;Empfaenger 2;Verwendungszweck;Betrag;Waehrung
            //90935230;15.09.2017;15.09.2017;;;SEPA-GUTSCHRIFT flightright GmbH PAYOUTID 456578 IHRE ENTSCH AEDIGUNG CASE 388486;345,30;EUR
            var fields = singleLine.Split(';').Select(t => t.RemoveBackTicks()).ToArray();
            if (!DateTime.TryParse(fields[2], out var date))
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
