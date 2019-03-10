using System;
using System.IO;
using System.Text;
using Xunit;
using YnabCsvConverter.Converter.N26;
using YnabCsvConverter.Interface.Model;

namespace YnabCsvConverter.IntegrationTest
{
    public class N26StatementConverterTest
    {
        [Fact]
        public void Convert()
        {
            //Arrange
            var contenOfStatement = File.ReadLines(@".\TestData\n26-checking-german.csv", Encoding.GetEncoding(0));
            var target = new N26StatementConverter();

            //Act
            target.LoadStatements(contenOfStatement);
            var actual = target.ConvertedStatements;

            //Assert
            Assert.Equal(TestData.ComparisonData(), actual);

        }
    }
}
