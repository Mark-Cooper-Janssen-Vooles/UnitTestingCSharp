using System;
using System.Net;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class InstallerHelperTests
    {
        private InstallerHelper _installerHelper;
        private Mock<IInstallerHelperRepository> _installerHelperRepository;

        [SetUp]
        public void Setup()
        {
            var setupDestinationFile = @"C:\dev";
            _installerHelperRepository = new Mock<IInstallerHelperRepository>();
            _installerHelper = new InstallerHelper(setupDestinationFile, _installerHelperRepository.Object);
        }
        
        [Test]
        public void DownloadInstaller_GivenNoException_ReturnsTrue()
        {
            var result = _installerHelper.DownloadInstaller("Mark", "Windows Installer");

            Assert.That(result, Is.EqualTo(true));
        }
        
        [Test]
        public void DownloadInstaller_GivenErrorIsThrown_ReturnsFalse()
        {
            _installerHelperRepository.Setup(ihr => 
                ihr.Download(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<WebException>();

            var result = _installerHelper.DownloadInstaller("Customer", "Installer");

            Assert.That(result, Is.EqualTo(false));
        }
    }
}