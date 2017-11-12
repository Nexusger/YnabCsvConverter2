using System;
using System.Collections.Generic;
using System.Linq;
using YnabCsvConverter.Common.Extension;
using System.Text;
using YnabCsvConverter.Interface.Model;

namespace YnabCsvConverter.Common
{
    public abstract class StatementConverter : IStatementConverter, IStatementConfidenceCalculator
    {
        public IEnumerable<string> InputStatements { get; private set; }
        public IList<YnabStatementLine> ConvertedStatements { get; private set; } = new List<YnabStatementLine>();
        protected float Confidence;
        public virtual void LoadStatements(IEnumerable<string> statements)
        {
            InputStatements = statements;
            ConvertStatements();
        }

        public IEnumerable<YnabStatementLine> GetConvertedStatements()
        {
            return GetConvertedStatements(DateTime.MinValue);
        }

        public IEnumerable<YnabStatementLine> GetConvertedStatements(DateTime oldestStatementToIncludeFilter)
        {
            return ConvertedStatements.Where(t => t.Date > oldestStatementToIncludeFilter);
        }

        public float GetConfidence()
        {
            return Confidence;
        }
        public abstract string NameOfStatement();
        protected virtual void ConvertStatements()
        {
            int counter = 0;
            foreach (var singleLine in InputStatements)
            {
                if (IsValidCreditCardStatement(singleLine))
                {
                    counter++;
                    ConvertedStatements.Add(ConvertToValidYnabStatement(singleLine));
                }
            }

            Confidence = (100f / (float)InputStatements.Count()) * (float)counter;
        }
        protected abstract bool IsValidCreditCardStatement(string singleStatementLine);
        protected abstract YnabStatementLine ConvertToValidYnabStatement(string singleLine);
    }
}
