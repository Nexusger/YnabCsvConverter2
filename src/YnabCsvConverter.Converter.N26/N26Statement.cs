using CsvHelper.Configuration.Attributes;
using System;

namespace YnabCsvConverter.Converter.N26
{
    public class N26Statement
    {
        [Name("Datum")]
        public DateTime Date { get; set; }
        [Name("Empfänger")]
        public string Receiver { get; set; }
        [Name("Verwendungszweck")]
        public string Comment { get; set; }
        [Name("Betrag (EUR)")]
        public float Sum { get; set; }
        [Name("Betrag (Fremdwährung)")]
        public float SumForeignCurrency { get; set; }


    }
}
