using System;
using System.IO;
using System.Text;
using Xunit;
using YnabCsvConverter.Converter.DKB;
using YnabCsvConverter.Converter.N26;
using YnabCsvConverter.Interface.Model;

namespace YnabCsvConverter.IntegrationTest
{
    public class DkbStatementConverterTest
    {
        [Fact]
        public void Convert()
        {
            //Arrange
            var contenOfStatement = File.ReadLines(@".\TestData\dkb-checking-german.csv", Encoding.GetEncoding(0));
            var target = new DkbGiroStatementConverter();

            //Act
            target.LoadStatements(contenOfStatement);
            var actual = target.ConvertedStatements;

            //Assert
            Assert.Equal(TestData.ComparisonData(), actual);

        }
    }
}
