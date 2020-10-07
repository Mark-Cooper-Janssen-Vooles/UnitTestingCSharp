using System;
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class DemeritPointsCalculatorTests
    {
        [Test]
        [TestCase(-1)]
        [TestCase(301)]
        public void CalculateDemeritPoints_WhenSpeedIsNegativeOrGreaterThanMaxSpeed_ShouldThrowArgumentOutOfRangeException(int speed)
        {
            var demeritPointsCalculator = new DemeritPointsCalculator();
            Assert.That(() => demeritPointsCalculator.CalculateDemeritPoints(speed), Throws.Exception.TypeOf<ArgumentOutOfRangeException>() );
        }
        
        [Test]
        [TestCase(0)]
        [TestCase(65)]
        public void CalculateDemeritPoints_WhenSpeedIsLessThanOrEqualToSpeedLimit_ShouldReturnZero(int speed)
        {
            var demeritPointsCalculator = new DemeritPointsCalculator();
            var result = demeritPointsCalculator.CalculateDemeritPoints(speed);
            Assert.That(result, Is.EqualTo(0));
        }
        
        [Test]
        [TestCase(64, 0)]
        [TestCase(70, 1)]
        [TestCase(300, 47)]
        public void CalculateDemeritPoints_WhenSpeedIsFiveKmOverSpeedLimit_ShouldReturnCorrectDemeritPoints(int speed, int expectedDemeritPoints)
        {
            var demeritPointsCalculator = new DemeritPointsCalculator();
            var result = demeritPointsCalculator.CalculateDemeritPoints(speed);
            Assert.That(result, Is.EqualTo(expectedDemeritPoints));
        }
    }
}