using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class CustomerControllerTests
    {

        [Test]
        public void GetCustomer_WhenCalledWithIdZero_ReturnsNotFound()
        {
            var customerController = new CustomerController();

            var result = customerController.GetCustomer(0);

            Assert.That(result, Is.TypeOf<NotFound>()); //exactly a NotFound object
            //Assert.That(result, Is.InstanceOf<NotFound>()); //a NotFound object, or one of its derivitives
        }
        
        [Test]
        public void GetCustomer_WhenCalled_ReturnsOk()
        {
            var customerController = new CustomerController();

            var result = customerController.GetCustomer(1);

            Assert.That(result, Is.TypeOf<Ok>());
        }
    }
}