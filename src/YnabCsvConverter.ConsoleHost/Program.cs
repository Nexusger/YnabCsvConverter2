using System.Collections.Generic;
using System.IO;
using System.Text;
using YnabCsvConverter.Converter.DKB;
using YnabCsvConverter.Interface.Model;
using YnabCsvConverter.Interface;
using System;
using YnabCsvConverter.Common;
using System.Linq;
using YnabCsvConverter.Converter.N26;
using YnabCsvConverter.Converter.Hypovereinsbank;
using CsvHelper;

namespace YnabCsvConverter.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args.Length > 2)
            {
                PrintUsage();
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine($"Die angegebene input-Datei {args[0]} existiert nicht");
                return;
            }

            var contenOfStatement = File.ReadLines(args[0], Encoding.GetEncoding(0));
            StatementConverter converter = SelectConverterWithHighestConfidence(contenOfStatement);

        var resultText = new List<string>() { YnabStatementLine.YnabHeader() };

            var result = converter.GetConvertedStatements();
            resultText.AddRange(result.ToStringList());
            string outputName = ChooseOutputFileName(args, converter);
            
            Console.WriteLine($"Write output to file \"{outputName}\"");
            File.WriteAllLines(outputName, resultText, Encoding.UTF8);
        }

        private static string ChooseOutputFileName(string[] args, StatementConverter converter)
        {
            var outputName = string.Empty;
            if (args.Length == 2)
            {
                outputName = args[1];
            }
            else
            {
                outputName = converter.NameOfStatement();
            }
            outputName = EnsureCorrectFileEnding(outputName);

            return outputName;
        }

        private static string EnsureCorrectFileEnding(string outputName)
        {
            if (!outputName.ToLower().EndsWith(".csv"))
            {
                outputName = $"{outputName}.csv";
            }

            return outputName;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Probleme mit den Parametern");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Usage: YnabConverter <Eingabe.csv> [<Ausgabe.csv>]");
        }

        private static StatementConverter SelectConverterWithHighestConfidence(IEnumerable<string> contentOfdkbStatement)
        {
            var converters = GetAllConverters();
            IConverterSelector selector = new ConverterSelector(converters, contentOfdkbStatement);
            PrintConfidenceLevels(selector.GetConfidence());
            var converter = selector.GetConverterWithHighestConfidence();
            return converter;
        }

        private static void PrintConfidenceLevels(IDictionary<StatementConverter, float> confidenceMap)
        {
            Console.WriteLine("----------Confidence----------");
            foreach (var item in confidenceMap.OrderByDescending(t => t.Value))
            {
                Console.WriteLine($"{item.Value,6:##0.00}.......... {item.Key.NameOfStatement()}");
            }
            Console.WriteLine("----------Confidence----------");
        }

        private static IEnumerable<StatementConverter> GetAllConverters()
        {
            return new List<StatementConverter>
            {
                new DkbCreditcardStatementConverter(),
                new DkbGiroStatementConverter(),
                new N26StatementConverter(),
                new HypoGiroStatementConverter(),
            };
        }
    }
}
