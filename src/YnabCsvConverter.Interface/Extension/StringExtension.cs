namespace YnabCsvConverter.Interface
{
    public static class StringExtension
    {
        public static string RemoveBackTicks(this string text)
        {
            if (text.StartsWith("\""))
            {
                return text.Substring(1, text.Length - 2);
            }
            return text;
        }
    }
}
