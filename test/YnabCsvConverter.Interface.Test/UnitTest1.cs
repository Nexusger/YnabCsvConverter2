using System;
using System.Linq;
using Xunit;

namespace YnabCsvConverter.Interface.Test
{
    public class StringExtensionTest
    {
        [Theory]
        [InlineData("",0)]
        [InlineData("\"", 0)]
        [InlineData("\"\"", 1)]
        [InlineData("\"Hallo\",\"Holla\",",2)]
        [InlineData("\"Hallo",0)]
        public void SplitByBackticksCountTest(string text, int amount)
        {
            //Act
            var actual=text.SplitByBackticks();

            Assert.Equal(amount, actual.Count());
        }
        [Fact]
        public void SplitByBackticksBackslashTest()
        {
            //Act
            var actual = "\"Hallo\"".SplitByBackticks();

            Assert.Equal("Hallo", actual.First());
        }
    }
}
