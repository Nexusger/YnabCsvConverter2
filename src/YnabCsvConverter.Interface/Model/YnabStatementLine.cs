using System;
using System.Globalization;

namespace YnabCsvConverter.Interface.Model
{
    public struct YnabStatementLine
    {

        public YnabStatementLine(DateTime date, string payee, string category, string memo, float inflow, float outflow)
        {
            Date = date;
            Payee = payee;
            Category = category;
            Memo = memo;
            Outflow = outflow;
            Inflow = inflow;
        }
        public DateTime Date { get; private set; }
        public string Payee { get; private set; }
        public string Category { get; private set; }
        public string Memo { get; private set; }
        public float Outflow { get; private set; }
        public float Inflow { get; private set; }
        public override string ToString()
        {
            var outflow = Outflow.ToString("F2", CultureInfo.InvariantCulture);
            var inflow = Inflow.ToString("F2", CultureInfo.InvariantCulture);
            return $"{Date.ToString("dd'/'MM'/'yyyy")},\"{Payee}\",\"{Category}\",\"{Memo.Replace(',',' ')}\",{outflow},{inflow}";
        }
        public static string YnabHeader()
        {
            return "Date,Payee,Category,Memo,Outflow,Inflow";
        }
    }
}
