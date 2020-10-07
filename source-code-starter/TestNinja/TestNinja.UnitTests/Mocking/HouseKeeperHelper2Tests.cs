using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class HouseKeeperHelper2Tests
    {
        private HousekeeperHelper2 _housekeeperHelper2;
        private Mock<IHousekeeperHelperRepository> _housekeeperHelperRepository;
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IEmailSender> _emailSender;
        private Mock<IXtraMessageBox> _xtraMessageBox;
        
        [SetUp]
        public void Setup()
        {
            _housekeeperHelperRepository = new Mock<IHousekeeperHelperRepository>();
            _statementGenerator = new Mock<IStatementGenerator>();
            _emailSender = new Mock<IEmailSender>();
            _xtraMessageBox = new Mock<IXtraMessageBox>();
            _housekeeperHelper2 = new HousekeeperHelper2(_housekeeperHelperRepository.Object, _statementGenerator.Object, _emailSender.Object, _xtraMessageBox.Object);
        }
        
        //do same tests below for multiple house keepers? 
        [Test]
        public void SendStatementEmails_NoHouseKeeperEmail_StatementGeneratorSaveStatementIsNotCalled()
        {
            //arrange
            _housekeeperHelperRepository.Setup(h => h.GetHouseKeepers()).Returns(
                new List<Housekeeper>()
                {
                    new Housekeeper() { Email = null }
                }.AsQueryable());

            //act
            _housekeeperHelper2.SendStatementEmails(new DateTime());

            //assert
            _statementGenerator.Verify(sg => 
                sg.SaveStatement(It.IsAny<int>(), 
                    It.IsAny<string>(), 
                    It.IsAny<DateTime>()), 
                Times.Never());
        }
        
        [Test]
        public void SendStatementEmails_statementFilenameIsEmptyString_TrySendStatementEmailIsNotCalled()
        {
            //arrange
            _housekeeperHelperRepository.Setup(h => h.GetHouseKeepers()).Returns(
                new List<Housekeeper>()
                {
                    new Housekeeper() { Email = "email@email.com" }
                }.AsQueryable());
            _statementGenerator.Setup(sg =>
                sg.SaveStatement(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns("");

            //act
            _housekeeperHelper2.SendStatementEmails(new DateTime());

            //assert
            _emailSender.Verify(es => 
                    es.EmailFile(It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }
        
                
        [Test]
        public void SendStatementEmails_EmailSenderEmailFileSucceeds_NoExceptionThrownReturnsTrue()
        {
            //arrange
            _housekeeperHelperRepository.Setup(h => h.GetHouseKeepers()).Returns(
                new List<Housekeeper>()
                {
                    new Housekeeper() { Email = "email@email.com" }
                }.AsQueryable());
            _statementGenerator.Setup(sg => 
                sg.SaveStatement(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns("file name");
            //note: better to call this with proper values, i.e. set up the new Housekeeper with proper values. So you know you're testing the right things
            
            //act
            _housekeeperHelper2.SendStatementEmails(new DateTime());
            
            //assert
            Assert.DoesNotThrow(() => _housekeeperHelper2.SendStatementEmails(new DateTime()));
        }
        
        [Test]
        public void SendStatementEmails_EmailSenderEmailFileUnsuccessful_xtraMessageBoxShowIsCalled()
        {
            //arrange
            _housekeeperHelperRepository.Setup(h => h.GetHouseKeepers()).Returns(
                new List<Housekeeper>()
                {
                    new Housekeeper() { Email = "email@email.com" }
                }.AsQueryable());
            _statementGenerator.Setup(sg => 
                sg.SaveStatement(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns("file name");
            _emailSender.Setup(es =>
                    es.EmailFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws<Exception>();
            
            //act
            _housekeeperHelper2.SendStatementEmails(new DateTime());
            
            //assert
            _xtraMessageBox.Verify(x => x.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButtons>()));
        }
    }
}