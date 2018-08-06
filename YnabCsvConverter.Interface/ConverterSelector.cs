using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            Statements = statements.ToList();
            FillConfidenceMap();
        }
        
        public IDictionary<StatementConverter, float> GetConfidence()
        {
            var result = new Dictionary<StatementConverter, float>();
            foreach (var item in ConfidenceMap)
            {
                result.Add(item.Key, item.Value);
            }
            return result;            
        }

        public StatementConverter GetConverterWithHighestConfidence()
        {
            if (ConfidenceMap.Count == 1)
            {
                return ConfidenceMap.Select(t=>t.Key).First();
            }

            //Handling für mehrere gleiche Werte
            return ConfidenceMap.OrderByDescending(t => t.Value).Select(k => k.Key).FirstOrDefault();
        }
          

        private void FillConfidenceMap()
        {
            if (ConfidenceMap == null)
            {
                ConfidenceMap = new ConcurrentDictionary<StatementConverter, float>();
                Parallel.ForEach(Converters, (converter) =>
                {
                    converter.LoadStatements(Statements);
                    ConfidenceMap.TryAdd(converter, converter.GetConfidence());
                });
            }
        }
    }
}
