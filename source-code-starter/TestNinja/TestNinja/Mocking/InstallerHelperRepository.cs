using System.Net;

namespace TestNinja.Mocking
{
    public interface IInstallerHelperRepository
    {
        void Download(string url, string destinationFile);
    }

    public class InstallerHelperRepository : IInstallerHelperRepository
    {
        public void Download(string url, string destinationFile)
        {
            new WebClient().DownloadFile($"{url}", destinationFile);
        }
    }
}