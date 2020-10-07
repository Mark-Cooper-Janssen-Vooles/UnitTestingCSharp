using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class HtmlFormatterTests
    {
        [Test]
        public void FormatAsBold_WhenCalled_ShouldReturnStringWrappedInStrongTag()
        {
            var htmlFormatter = new HtmlFormatter();
            var result = htmlFormatter.FormatAsBold("Hello World");
            Assert.That(result, Is.EqualTo("<strong>Hello World</strong>")); //this is specific
            //by default, assertions against strings are case-sensitive. To ignore case:
            Assert.That(result, Is.EqualTo("<strong>Hello World</strong>").IgnoreCase);
            
            //more general ways to test:
            Assert.That(result, Does.StartWith("<strong>"));
            Assert.That(result, Does.EndWith("</strong>")); 
            Assert.That(result, Does.Contain("Hello World")); 
        }
    }
}