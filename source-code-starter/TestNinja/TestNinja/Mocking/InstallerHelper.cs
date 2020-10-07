using System.Net;

//what if class made with no setupDestinationFile?
//client should be injected, not newed up
// string can be changed to $
//exception handling should be its own function
//test for true 
//test for false
namespace TestNinja.Mocking
{
    public class InstallerHelper
    {
        private readonly string _setupDestinationFile;
        private readonly IInstallerHelperRepository _installerHelperRepository;

        public InstallerHelper(string setupDestinationFile, IInstallerHelperRepository installerHelperRepository)
        {
            _setupDestinationFile = setupDestinationFile;
            _installerHelperRepository = installerHelperRepository;
        }

        public bool DownloadInstaller(string customerName, string installerName)
        {
            try
            {
                _installerHelperRepository.Download($"http://example.com/{customerName}/{installerName}", _setupDestinationFile);
                return true;
            }
            catch (WebException)
            {
                return false; 
            }
        }
    }
}