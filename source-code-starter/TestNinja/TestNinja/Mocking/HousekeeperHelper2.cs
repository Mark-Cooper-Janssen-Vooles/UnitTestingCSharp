using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TestNinja.Mocking
{
    public class HousekeeperHelper2
    {
        private static IHousekeeperHelperRepository _houseKeeperRepository;
        private static IStatementGenerator _statementGenerator;
        private static IEmailSender _emailSender;
        private static IXtraMessageBox _xtraMessageBox;

        public HousekeeperHelper2(
            IHousekeeperHelperRepository houseKeeperRepository, 
            IStatementGenerator statementGenerator, 
            IEmailSender emailSender, 
            IXtraMessageBox xtraMessageBox)
        {
            _houseKeeperRepository = houseKeeperRepository;
            _statementGenerator = statementGenerator;
            _emailSender = emailSender;
            _xtraMessageBox = xtraMessageBox;
        }

        public void SendStatementEmails(DateTime statementDate)
        {
            foreach (var housekeeper in _houseKeeperRepository.GetHouseKeepers())
            {
                if (housekeeper.Email == null)
                    continue;
                var statementFilename = _statementGenerator.SaveStatement(housekeeper.Oid, housekeeper.FullName, statementDate);
                if (string.IsNullOrWhiteSpace(statementFilename))
                    continue;
                
                TrySendStatementEmail(statementDate, housekeeper, statementFilename);
            }
        }

        private static void TrySendStatementEmail(DateTime statementDate, Housekeeper housekeeper, string statementFilename)
        {
            try
            {
                _emailSender.EmailFile(housekeeper.Email, housekeeper.StatementEmailBody, statementFilename,
                    CreateSubject(statementDate, housekeeper));
            }
            catch (Exception e)
            {
                _xtraMessageBox.Show(e.Message, HousekeeperStatements(housekeeper),
                    MessageBoxButtons.OK);
            }
        }

        private static string HousekeeperStatements(Housekeeper housekeeper)
        {
            return string.Format("Email failure: {0}", housekeeper.Email);
        }

        private static string CreateSubject(DateTime statementDate, Housekeeper housekeeper)
        {
            return string.Format("Sandpiper Statement {0:yyyy-MM} {1}", statementDate, housekeeper.FullName);
        }
    }


    public interface IEmailSender
    {
        void EmailFile(string emailAddress, string emailBody, string filename, string subject);
    }

    public class EmailSender : IEmailSender
    {
        public void EmailFile(string emailAddress, string emailBody, string filename, string subject)
        {
            var client = new SmtpClient(SystemSettingsHelper.EmailSmtpHost)
            {
                Port = SystemSettingsHelper.EmailPort,
                Credentials =
                    new NetworkCredential(
                        SystemSettingsHelper.EmailUsername,
                        SystemSettingsHelper.EmailPassword)
            };

            var from = new MailAddress(SystemSettingsHelper.EmailFromEmail, SystemSettingsHelper.EmailFromName,
                Encoding.UTF8);
            var to = new MailAddress(emailAddress);

            var message = new MailMessage(from, to)
            {
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                Body = emailBody,
                BodyEncoding = Encoding.UTF8
            };

            message.Attachments.Add(new Attachment(filename));
            client.Send(message);
            message.Dispose();

            File.Delete(filename);
        }
    }
    public interface IStatementGenerator
    {
        string SaveStatement(int housekeeperOid, string housekeeperName, DateTime statementDate);
    }

    public class StatementGenerator : IStatementGenerator
    {
        public string SaveStatement(int housekeeperOid, string housekeeperName, DateTime statementDate)
        {
            var report = new HousekeeperStatementReport(housekeeperOid, statementDate);
            //var report = _housekeeperStatementReport.generateReport(housekeeperOid, statementDate);
            
            if (!report.HasData)
                return string.Empty;

            report.CreateDocument();

            var filename = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                string.Format("Sandpiper Statement {0:yyyy-MM} {1}.pdf", statementDate, housekeeperName));

            report.ExportToPdf(filename);

            return filename;
        }
    }

    public enum MessageBoxButtons
    {
        OK
    }


    public interface IXtraMessageBox
    {
        void Show(string s, string housekeeperStatements, MessageBoxButtons ok);
    }

    public class XtraMessageBox : IXtraMessageBox
    {
        public void Show(string s, string housekeeperStatements, MessageBoxButtons ok)
        {
        }
    }

    public class SystemSettingsHelper
    {
        public static string EmailSmtpHost { get; set; }
        public static int EmailPort { get; set; }
        public static string EmailUsername { get; set; }
        public static string EmailPassword { get; set; }
        public static string EmailFromEmail { get; set; }
        public static string EmailFromName { get; set; }
    }

    public class Housekeeper
    {
        public string Email { get; set; }
        public int Oid { get; set; }
        public string FullName { get; set; }
        public string StatementEmailBody { get; set; }
    }

    public interface IHousekeeperStatementReport
    {
        bool HasData { get; set; }
        void CreateDocument();
        void ExportToPdf(string filename);
    }

    public class HousekeeperStatementReport : IHousekeeperStatementReport
    {
        public HousekeeperStatementReport(int housekeeperOid, DateTime statementDate)
        {
        }

        public bool HasData { get; set; }

        public void CreateDocument()
        {
        }

        public void ExportToPdf(string filename)
        {
        }
    }
}