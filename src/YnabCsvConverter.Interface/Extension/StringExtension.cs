using System.Collections.Generic;

namespace YnabCsvConverter.Interface
{
    public static class StringExtension
    {
        public static string RemoveBackTicks(this string text)
        {
            if (text.Length <= 2)
            {
                return text;
            }
            if (text.StartsWith("\""))
            {
                return text.Substring(1, text.Length - 2);
            }
            return text;
        }

        public static IEnumerable<string> SplitByBackticks(this string text)
        {
            bool isInBackTick = false;
            int start = 0;
            int end = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (!isInBackTick)
                {
                    if (text[i] == '"')
                    {
                        isInBackTick = true;
                        start = i;
                    }
                }
                else
                {
                    if (text[i] == '"')
                    {
                        isInBackTick = false;
                        end = i;
                        yield return text.Substring(start+1, (end - start)-1);
                    }
                }
            }
        }
    }
}
