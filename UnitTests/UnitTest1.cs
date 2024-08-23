using FluentAssertions;
using Microsoft.Extensions.Logging;
using UnitTests;

namespace PersephoneAlerting.Tests
{
    public class TestingExceptions
    {
        [Fact()]
        public async Task RunTest()
        {
            var exception =  await Record.ExceptionAsync(() => PersephoneAlerting.TestingExceptions
            .RunAsync(
                new HttpTestRequest(), 
                new LoggerFactory().CreateLogger("")));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<DivideByZeroException>();
        }
    }
}