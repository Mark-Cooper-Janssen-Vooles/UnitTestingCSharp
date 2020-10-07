using System;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class EmployeeControllerTests
    {
        private EmployeeController _employeeController;
        private Mock<IEmployeeStorage> _employeeStorage;
        
        [SetUp]
        public void Setup()
        {
            _employeeStorage = new Mock<IEmployeeStorage>();
            _employeeController = new EmployeeController(_employeeStorage.Object);
        }
        
        [Test]
        public void DeleteEmployee_WhenCalled_DeletesEmployeeFromDb()
        {
            const int employeeId = 1;
            _employeeController.DeleteEmployee(employeeId);
            
            _employeeStorage.Verify(es => es.RemoveEmployee(employeeId));
        }
    }
}