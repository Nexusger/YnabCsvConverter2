using System.Collections.Generic;
using YnabCsvConverter.Common;
namespace YnabCsvConverter.Interface
{
    public interface IConverterSelector
    {
        IEnumerable<(StatementConverter, float)> GetConfidence();

        StatementConverter GetConverterWithHighestConfidence();
    }
}