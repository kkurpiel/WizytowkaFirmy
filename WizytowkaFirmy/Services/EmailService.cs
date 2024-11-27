using MimeKit;
using MailKit.Net.Smtp;
using NLog;
using System.Security.Cryptography;
using System.Text;

namespace WizytowkaFirmy.Services
{
    public class EmailService
    {
        private readonly IConfiguration configuration;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("SmtpServiceKey1#");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("##SmtpServiceIV#");
        public EmailService(IConfiguration configuration)
        {
            try
            {
                this.configuration = configuration;
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync("https://www.google.com").Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        logger.Error("Brak połączenia z internetem");
                    }
                    else
                    {
                        logger.Info("Połączenie z internetem działa");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        public async Task WyslijEmailAsync(string email_od, string temat, string message)
        {
            try
            {
                var smtpConfig = configuration.GetSection("Smtp");

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Witryna Firmowa", smtpConfig["EmailOd"]));
                email.To.Add(new MailboxAddress("Administrator", smtpConfig["EmailDo"]));
                email.Subject = temat;
                email.Body = new TextPart("plain")
                {
                    Text = $"Wiadomość od: {email_od}\n\n{message}"
                };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(smtpConfig["Host"], int.Parse(smtpConfig["Port"]), MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(smtpConfig["EmailOd"], Decrypt(smtpConfig["Password"]));
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        public static string Decrypt(string hasloSzyfr)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(hasloSzyfr)))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return string.Empty;
            }
        }
    }
}
