using System.Collections.Generic;
using System.Linq;
using YnabCsvConverter.Interface.Model;

namespace YnabCsvConverter.Interface
{
    public static class Helper
    {
        public static IEnumerable<string> ToStringList(this IEnumerable<YnabStatementLine> @this)
        {
            return @this.Select(t => t.ToString());
        }
    }
}
