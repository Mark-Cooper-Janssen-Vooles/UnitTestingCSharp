using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Math = TestNinja.Fundamentals.Math;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class MathTests
    {
        private Math _math;
        [SetUp]
        public void SetUp()
        {
            _math = new Math();
        }
        
        [Test]
        public void Add_WhenCalled_ReturnTheSumOfArguments()
        {
            var result = _math.Add(2, 2);
            Assert.That(result, Is.EqualTo(4));
        }

        [Test]
        [TestCase(2, 1, 2)]
        [TestCase(1, 2, 2)]
        [TestCase(1, 1, 1)]
        public void Max_WhenCalled_ReturnsLargerArgument(int a, int b, int expectedResult)
        {
            var result = _math.Max(a, b);
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        //[TestCase(5, new [] {1, 3, 5})]
        public void GetOddNumbers_WhenCalled_ReturnsEnumerableOfOddNumbersToMaxNumber()
        {
            var result = _math.GetOddNumbers(5);

            //general
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(3));
            //more specific
            Assert.That(result, Does.Contain(1));
            Assert.That(result, Does.Contain(3));
            Assert.That(result, Does.Contain(5));
            //hyper specific
            Assert.That(result, Is.EqualTo(new [] {1, 3, 5}));

            //other possible useful assertions:
            Assert.That(result, Is.Ordered);
            Assert.That(result, Is.Unique);
        }
    }
}