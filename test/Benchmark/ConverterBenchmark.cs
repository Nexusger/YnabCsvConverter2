using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using YnabCsvConverter.Converter.DKB;
using YnabCsvConverter.Converter.Hypovereinsbank;

namespace Benchmark
{
    public class ConverterBenchmark
    {
        private List<string> Statements = new List<string>();
        public ConverterBenchmark()
        {
            for (int i = 0; i < 1000; i++)
            {
                var date = DateTime.Parse("01.01.2000").AddDays(i).ToString("dd.MM.yyyy");
                var random = new Random();
                var amount = (random.NextDouble() - 0.5) * 100;
                var culture = CultureInfo.CreateSpecificCulture("de-DE");
                Statements.Add($"\"Ja\";\"{date}\";\"{date}\";\"HabenzinsenZ 000000618 T 029   0000\";\"{amount.ToString("C", culture)}\";\"\";");
                Statements.Add($"\"{date}\";\"{date}\";\"Lastschrift\";\"DB Vertrieb GmbH\";\"Fahrschein Q64P6BFahrschein 2B1EUH\";\"DE02100100100152517108\";\"PBNKDEFFXXX\";\"{amount.ToString("C", culture)}\";\"DE39DBV00000002177    \";\"1100119479210010      \";\"\";");
                Statements.Add($"90935230;{date};{date};;;SEPA-GUTSCHRIFT flightright GmbH PAYOUTID 456578 IHRE ENTSCH AEDIGUNG CASE 388486;{amount.ToString("C", culture)};EUR");
            }
        }
            
        [Benchmark]
        public void DkbGiroStatementConverter()
        {
            var converter = new DkbGiroStatementConverter();
            converter.LoadStatements(Statements);
        }

        [Benchmark]
        public void DkbCreditcardStatementConverter()
        {
            var converter = new DkbCreditcardStatementConverter();
            converter.LoadStatements(Statements);
        }

        [Benchmark]
        public void HypoGiroStatementConverter()
        {
            var converter = new HypoGiroStatementConverter();
            converter.LoadStatements(Statements);
        }

    }
}
