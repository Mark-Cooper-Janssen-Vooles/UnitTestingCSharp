using System;
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class StackTests
    {
        [Test]
        public void Push_WhenCalledWithNull_ShouldThrowArgumentNullException()
        {
            var stack = new Stack<string>();
            Assert.That(() => stack.Push(null), Throws.Exception.TypeOf<ArgumentNullException>());
        }
        
        [Test]
        public void Push_WhenCalledWithObject_ShouldAddItToTheStack()
        {
            var stack = new Stack<string>();
            stack.Push("hello");

            Assert.That(stack.Count, Is.GreaterThan(0));
        }

        [Test]
        public void Count_WhenEmptyStack_ReturnZero()
        {
            var stack = new Stack<string>();
            Assert.That(stack.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void Pop_WhenCalledWithEmptyStack_ShouldThrowInvalidOperationException()
        {
            var stack = new Stack<string>();
            Assert.That(() => stack.Pop(), Throws.Exception.TypeOf<InvalidOperationException>());
        }
        
        [Test]
        public void Pop_WhenCalledWithObject_ShouldRemoveItFromTheStack()
        {
            var stack = new Stack<string>();
            stack.Push("hello");
            var result = stack.Pop();

            Assert.That(result, Is.EqualTo("hello"));
            Assert.That(stack.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void Pop_WhenCalledWithObjects_ShouldRemoveLastAddedObjectFromTheStack()
        {
            var stack = new Stack<string>();
            stack.Push("hello");
            stack.Push("world");
            var result = stack.Pop();

            Assert.That(result, Is.EqualTo("world"));
            Assert.That(stack.Count, Is.EqualTo(1));
        }
        
                
        [Test]
        public void Peek_WhenCalledWithEmptyStack_ShouldThrowInvalidOperationException()
        {
            var stack = new Stack<string>();
            Assert.That(() => stack.Peek(), Throws.Exception.TypeOf<InvalidOperationException>());
        }
        
        [Test]
        public void Peek_WhenCalledWithObject_ShouldReturnLatestPushedObject()
        {
            var stack = new Stack<string>();
            stack.Push("hello");
            stack.Push("world");
            var peek = stack.Peek();

            Assert.That(peek, Is.EqualTo("world"));
            Assert.That(stack.Count, Is.EqualTo(2));
        }
    }
}