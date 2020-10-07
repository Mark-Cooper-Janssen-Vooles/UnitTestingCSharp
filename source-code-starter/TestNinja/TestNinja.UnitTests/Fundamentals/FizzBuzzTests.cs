using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class FizzBuzzTests
    {
        [Test]
        [TestCase(3, "Fizz")]
        [TestCase(5, "Buzz")]
        [TestCase(15, "FizzBuzz")]
        [TestCase(1, "1")]
        public void GetOutput_WhenCalled_ShouldReturnCorrectString(int number, string returnString)
        {
            var result = FizzBuzz.GetOutput(number);
            Assert.That(result, Is.EqualTo(returnString));
        }
    }
}