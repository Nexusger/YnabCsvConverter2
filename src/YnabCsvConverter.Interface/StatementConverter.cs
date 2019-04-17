using System;
using System.Collections.Generic;
using System.Linq;
using YnabCsvConverter.Interface.Model;

namespace YnabCsvConverter.Interface
{
    public abstract class StatementConverter : IStatementConverter, IStatementConfidenceCalculator
    { public const string CONVERTERMARKER = "Converted!";
        public IEnumerable<string> InputStatements { get; private set; }
        public IList<YnabStatementLine> ConvertedStatements { get; private set; } = new List<YnabStatementLine>();
        protected float Confidence;
        public virtual void LoadStatements(IEnumerable<string> statements)
        {
            if (statements == null)
            {
                throw new ArgumentNullException();
            }
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
            int counterFile = 0;
            foreach (var singleLine in InputStatements)
            {
                if (IsValidStatement(singleLine))
                {
                    counterFile++;
                    ConvertedStatements.Add(ConvertToValidYnabStatement(singleLine));
                }
            }
            Confidence = (100f / (float)InputStatements.Count()) * (float)counterFile;
        }

        protected abstract bool IsValidStatement(string singleStatementLine);
        protected abstract YnabStatementLine ConvertToValidYnabStatement(string singleLine);
    }
}
