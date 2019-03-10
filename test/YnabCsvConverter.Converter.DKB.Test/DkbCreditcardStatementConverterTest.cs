using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using YnabCsvConverter.Converter.DKB;

namespace YnabCsvConverter.Converter.Test
{
    public class DkbCreditcardStatementConverterTest
    {

        [Fact]
        public void ConverterCanBeInstantiated()
        {
            var target = new DkbCreditcardStatementConverter();
            Assert.NotNull(target);
        }

        [Fact]
        public void StatementIsNullNoStatementIsSaved()
        {
            var target = new DkbCreditcardStatementConverter();
            Assert.Throws<ArgumentNullException>(()=> target.LoadStatements(null));
        }


        [Fact]
        public void StatementsAreSaved()
        {
            var statement = new List<string>().AsEnumerable();
            var target = new DkbCreditcardStatementConverter();
            target.LoadStatements(statement);
            Assert.NotNull(target);
            Assert.Equal(statement, target.InputStatements);

        }
        [Fact]
        public void StatementConverted()
        {
            var statement = new List<string>();
            statement.Add("\"Ja\";\"22.07.2017\";\"21.07.2017\";\"HabenzinsenZ 123456789 T 029   0000\";\"12,34\";\"\";");
            var date = DateTime.Parse("20.07.2017");
            var target = new DkbCreditcardStatementConverter();
            target.LoadStatements(statement.AsEnumerable());
            Assert.NotNull(target);
            Assert.Equal(statement, target.InputStatements);
            var actual = target.GetConvertedStatements().FirstOrDefault();
            Assert.Equal("",actual.Category);
            Assert.Equal(DateTime.Parse("21.07.2017"), actual.Date);
            Assert.Equal(12.34f ,actual.Inflow);
            Assert.Equal("Converted! Original: HabenzinsenZ 123456789 T 029   0000", actual.Memo);
            Assert.Equal(0.0, actual.Outflow);
            Assert.Equal("", actual.Payee);
         }
        [Fact]
        public void TimeFilterWorks()
        {
            var statement = new List<string>();
            statement.Add("\"Ja\";\"22.07.2017\";\"21.07.2017\";\"HabenzinsenZ 1234567890 T 029   0000\";\"12,34\";\"\";");
            
            var target = new DkbCreditcardStatementConverter();
            target.LoadStatements(statement.AsEnumerable());
            Assert.NotNull(target);
            Assert.Equal(statement, target.InputStatements);
            var actual = target.GetConvertedStatements().Count();
            Assert.Equal(1,actual);
            actual = target.GetConvertedStatements(DateTime.Parse("20.07.2017")).Count();
            Assert.Equal(1, actual);
            actual = target.GetConvertedStatements(DateTime.Parse("21.07.2017")).Count();
            Assert.Equal(0, actual);
        }
        [Fact]
        public void NonStatementConverted()
        {
            var statement = new List<string>();
            statement.Add("\"Umsatz abgerechnet und nicht im Saldo enthalten\";\"Wertstellung\";\"Belegdatum\";\"Beschreibung\";\"Betrag(EUR)\";\"Urspr�nglicher Betrag\";");
            var date = DateTime.UtcNow;
            var target = new DkbCreditcardStatementConverter();
            target.LoadStatements(statement.AsEnumerable());
            Assert.NotNull(target);
            Assert.Equal(statement, target.InputStatements);

            var actual = target.GetConvertedStatements(date);
            Assert.Empty(actual);
        }
        [Fact]
        public void HasName()
        {
            var target = new DkbCreditcardStatementConverter();
            var actual = target.NameOfStatement();
            Assert.False(string.IsNullOrEmpty(actual));
            Assert.False(string.IsNullOrWhiteSpace(actual));
        }

        [Fact]
        public void ConfidenceIsZero()
        {
            var target = new DkbCreditcardStatementConverter();
            Assert.Equal(0f, target.GetConfidence());
        }
        [Fact]
        public void ConfidenceIsHundred()
        {
            var statement = new List<string>();
            statement.Add("\"Ja\";\"22.07.2017\";\"21.07.2017\";\"HabenzinsenZ 123456789 T 029   0000\";\"12,34\";\"\";");
            var target = new DkbCreditcardStatementConverter();
            target.LoadStatements(statement);
            Assert.Equal(100f, target.GetConfidence());
        }
        
      

    }
}
