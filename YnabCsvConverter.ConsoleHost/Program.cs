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
            if (args.Length == 2 && File.Exists(args[0]) && !string.IsNullOrEmpty(args[1]))
            {
                var contentOfdkbStatement = File.ReadLines(args[0], Encoding.GetEncoding(0));
                var converters = GetAllConverters();
                IConverterSelector selector = new ConverterSelector(converters, contentOfdkbStatement);
                PrintConfidenceLevels(selector.GetConfidence());
                var converter = selector.GetConverterWithHighestConfidence();
                var result = converter.GetConvertedStatements();

                var resultText = new List<string>() { YnabStatementLine.YnabHeader() };

                resultText.AddRange(result.ToStringList());
                File.WriteAllLines(args[1], resultText, Encoding.UTF8);
                Console.ReadLine();
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
                Console.Read();

            }
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
