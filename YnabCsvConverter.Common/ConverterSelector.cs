using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using YnabCsvConverter.Interface;

namespace YnabCsvConverter.Common
{
    public class ConverterSelector : IConverterSelector
    {
        private ConcurrentDictionary<StatementConverter, float> ConfidenceMap;
        private readonly IEnumerable<StatementConverter> Converters;
        private readonly IEnumerable<string> Statements;
        public ConverterSelector(IEnumerable<StatementConverter> converters, IEnumerable<string> statements)
        {
            Converters = converters;
            Statements = statements;
            FillConfidenceMap();
        }
        
        public IEnumerable<(StatementConverter, float)> CalculateConfidence()
        {
            return ConfidenceMap.Select(t => (t.Key, t.Value));
            
        }

        public StatementConverter GetConverterWithHighestConfidence()
        {
            if (ConfidenceMap.Count == 1)
            {
                return ConfidenceMap.Select(t=>t.Key).First();
            }

            //Handling für mehrere gleiche Werte
            return ConfidenceMap.OrderBy(t => t.Value).Select(k => k.Key).FirstOrDefault();
        }
          

        private void FillConfidenceMap()
        {
            if (ConfidenceMap == null)
            {
                ConfidenceMap = new ConcurrentDictionary<StatementConverter, float>();
                foreach (var converter in Converters)
                {
                    converter.LoadStatements(Statements);
                    ConfidenceMap.TryAdd(converter, converter.GetConfidence());
                }
            }
        }
    }
}
