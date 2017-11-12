﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using YnabCsvConverter.Converter.DKB;

namespace YnabCsvConverter.Converter.Test
{
    public class DkbGiroStatementConverterTest
    {

        [Fact]
        public void ConverterCanBeInstantiated()
        {
            var target = new DkbGiroStatementConverter();
            Assert.NotNull(target);
        }

        [Fact]
        public void StatementIsNullNoStatementIsSaved()
        {
            var target = new DkbGiroStatementConverter();
            Assert.Throws<ArgumentNullException>(()=> target.LoadStatements(null));
        }


        [Fact]
        public void StatementsAreSaved()
        {
            var statement = new List<string>().AsEnumerable();
            var target = new DkbGiroStatementConverter();
            target.LoadStatements(statement);
            Assert.NotNull(target);
            Assert.Equal(statement, target.InputStatements);

        }
        
        [Fact]
        public void BalanceStatementIgnored()
        {
            var statement = new List<string>();
            statement.Add("\"02.10.2017\";\"01.10.2017\";\"Abschluss\";\"\";\"Abrechnung 29.09.2017      siehe Anlage<br />Abrechnung 29.09.2017<br />Information zur Abrechnung<br />Kontostand am 29.09.2017                                        4.390,49 +<br />Abrechnungszeitraum vom 01.07.2017 bis 30.09.2017<br />Abrechnung 30.09.2017                                                0,00+<br />Sollzinss�tze am 30.09.2017<br /> 6,9000 v.H. f�r Dispositionskredit<br />(aktuelle Kreditlinie       2.400,00)<br /> 6,9000 v.H. f�r Konto�berziehungen<br />�ber die Kreditlinie hinaus<br />Kontostand/Rechnungsabschluss am 29.09.2017                     4.390,49 +<br />Rechnungsnummer: 20171002-BY111-00040704235\";\"1007317249\";\"12030000\";\"0,00\";\"\";\"\";\"\";");
            var target = new DkbGiroStatementConverter();
            target.LoadStatements(statement.AsEnumerable());
            Assert.NotNull(target);
            Assert.Equal(statement, target.InputStatements);
            var actual = target.GetConvertedStatements().Count();
            Assert.Equal(0,actual);
         }

        [Fact]
        public void StatementConverted()
        {
            var statement = new List<string>();
            statement.Add("\"22.07.2017\";\"21.07.2017\";\"Lastschrift\";\"DB Vertrieb GmbH\";\"Fahrschein Q64P6BFahrschein 2B1EUH\";\"DE02100100100152517108\";\"PBNKDEFFXXX\";\"-153,40\";\"DE39DBV00000002177    \";\"1100119479210010      \";\"\";");
            var date = DateTime.Parse("20.07.2017");
            var target = new DkbGiroStatementConverter();
            target.LoadStatements(statement.AsEnumerable());
            Assert.NotNull(target);
            Assert.Equal(statement, target.InputStatements);
            var actual = target.GetConvertedStatements().FirstOrDefault();
            Assert.NotNull(actual);
            Assert.Equal("", actual.Category);
            Assert.Equal(DateTime.Parse("21.07.2017"), actual.Date);
            Assert.InRange(actual.Outflow, 153.3999, 153.4001);
            Assert.Equal("Converted! Lastschrift From DB Vertrieb GmbH Fahrschein Q64P6BFahrschein 2B1EUH", actual.Memo);
            Assert.Equal(0.0, actual.Inflow);
            Assert.Equal("DB Vertrieb GmbH", actual.Payee);
        }

        [Fact]
        public void TimeFilterWorks()
        {
            var statement = new List<string>();
            statement.Add("\"22.07.2017\";\"21.07.2017\";\"Lastschrift\";\"DB Vertrieb GmbH\";\"Fahrschein Q64P6BFahrschein 2B1EUH\";\"DE02100100100152517108\";\"PBNKDEFFXXX\";\"-153,40\";\"DE39DBV00000002177    \";\"1100119479210010      \";\"\";");

            var target = new DkbGiroStatementConverter();
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
            var target = new DkbGiroStatementConverter();
            target.LoadStatements(statement.AsEnumerable());
            Assert.NotNull(target);
            Assert.Equal(statement, target.InputStatements);

            var actual = target.GetConvertedStatements(date);
            Assert.Empty(actual);
        }
        [Fact]
        public void HasName()
        {
            var target = new DkbGiroStatementConverter();
            var actual = target.NameOfStatement();
            Assert.False(string.IsNullOrEmpty(actual));
            Assert.False(string.IsNullOrWhiteSpace(actual));
        }

        [Fact]
        public void ConfidenceIsZero()
        {
            var target = new DkbGiroStatementConverter();
            Assert.Equal(0f, target.GetConfidence());
        }
        [Fact]
        public void ConfidenceIsHUndred()
        {
            var statement = new List<string>();
            statement.Add("\"22.07.2017\";\"21.07.2017\";\"Lastschrift\";\"DB Vertrieb GmbH\";\"Fahrschein Q64P6BFahrschein 2B1EUH\";\"DE02100100100152517108\";\"PBNKDEFFXXX\";\"-153,40\";\"DE39DBV00000002177    \";\"1100119479210010      \";\"\";");
            var target = new DkbGiroStatementConverter();
            target.LoadStatements(statement);
            Assert.Equal(100f, target.GetConfidence());
        }
    }
}
