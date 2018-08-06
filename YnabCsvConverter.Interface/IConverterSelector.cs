using System.Collections.Generic;
using YnabCsvConverter.Common;
namespace YnabCsvConverter.Interface
{
    public interface IConverterSelector
    {
        IDictionary<StatementConverter, float> GetConfidence();

        StatementConverter GetConverterWithHighestConfidence();
    }
}