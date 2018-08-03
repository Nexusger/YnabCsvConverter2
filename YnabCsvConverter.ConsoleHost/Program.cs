using System.Collections.Generic;
using System.IO;
using System.Text;
using YnabCsvConverter.Converter.DKB;
using YnabCsvConverter.Interface.Model;
using YnabCsvConverter.Interface;
using System;
using YnabCsvConverter.Common;
using System.Linq;
using System.Reflection;

namespace YnabCsvConverter.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length <= 2 && args.Length > 0 && File.Exists(args[0]))
            {
                var contentOfdkbStatement = File.ReadLines(args[0], Encoding.GetEncoding(0));
                StatementConverter converter = SelectConverterWithHighestConfidence(contentOfdkbStatement);

                var resultText = new List<string>() { YnabStatementLine.YnabHeader() };

                var result = converter.GetConvertedStatements();
                resultText.AddRange(result.ToStringList());
                var outputName = string.Empty;
                if (args.Length == 2)
                {
                    outputName = args[1];
                }
                else
                {
                    outputName = $"{converter.NameOfStatement()}.csv"; 
                }
                File.WriteAllLines(outputName, resultText, Encoding.UTF8);
            }
            else
            {

                Console.WriteLine("Probleme mit den Parametern");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Usage: YnabConverter <Eingabe.csv> <Ausgabe.csv>");
                if (args.Length != 2)
                {
                    Console.WriteLine($"Es darf nur ein Parameter angegeben werden. Es wurden {args.Length} übergeben.");
                }
                if (!File.Exists(args[0]))
                {
                    Console.WriteLine($"Die Datei {args[0]} existiert nicht");
                }
                if (string.IsNullOrEmpty(args[1]))
                {
                    Console.WriteLine($"Der Dateiname der Ausgabedatei ist leer");
                }

            }
        }

        private static StatementConverter SelectConverterWithHighestConfidence(IEnumerable<string> contentOfdkbStatement)
        {
            var converters = GetAllConverters();
            IConverterSelector selector = new ConverterSelector(converters, contentOfdkbStatement);
            PrintConfidenceLevels(selector.GetConfidence());
            var converter = selector.GetConverterWithHighestConfidence();
            return converter;
        }

        private static void PrintConfidenceLevels(IEnumerable<(StatementConverter, float)> confidenceMap)
        {
            Console.WriteLine("----------Confidence----------");
            foreach (var item in confidenceMap.OrderByDescending(t => t.Item2))
            {
                Console.WriteLine($"{item.Item2,6:##0.00}.......... {item.Item1.NameOfStatement()}");
            }
            Console.WriteLine("----------Confidence----------");
        }

        private static IEnumerable<StatementConverter> GetAllConverters()
        {
            return new List<StatementConverter>
            {
                new DkbCreditcardStatementConverter(),
                new DkbGiroStatementConverter(),
            };
        }
    }
}
