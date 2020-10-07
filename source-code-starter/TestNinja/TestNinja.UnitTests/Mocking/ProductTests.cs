using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class ProductTests
    {
        [Test]
        public void GetPrice_WhenCustomerIsGold_ShouldGive30PercentDiscount()
        {
            var product = new Product() { ListPrice = 10.0f };

            var price = product.GetPrice(new Customer() {IsGold = true});

            Assert.That(price, Is.EqualTo(7.0f));
        }

        [Test]
        public void GetPrice_WhenCustomerIsGold_MockExample()
        {
            var customer = new Mock<ICustomer>();
            customer.Setup(c => c.IsGold).Returns(true); //an example of mock abuse, for something so simple
            
            var product = new Product() { ListPrice = 10.0f };
            
            var price = product.GetPrice(customer.Object);

            Assert.That(price, Is.EqualTo(7.0f));
        }
    }
}