using DuckyBot.Core.Utilities;
using Xunit;

namespace DuckyBot.xUnit.Tests
{
    public class UtilityTests
    {
        [Fact]
        public static void MyFirstTest()
        {
            const int expected = 5;

            var actual = Testing.MyTest(expected);

            Assert.Equal(expected, actual);
        }
    }
}
