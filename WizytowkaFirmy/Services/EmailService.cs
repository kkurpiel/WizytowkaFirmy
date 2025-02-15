﻿using MimeKit;
using MailKit.Net.Smtp;
using NLog;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

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
            this.configuration = configuration;
        }
        public async Task WyslijEmailAsync(string email_od, string temat, string message)
        {
            try
            {
                var smtpConfig = configuration.GetSection("Smtp");

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Witryna Firmowa", smtpConfig["EmailOd"]));
                email.To.Add(new MailboxAddress("Administrator", smtpConfig["EmailDo"]));

                if (email_od == smtpConfig["EmailDo"] && temat == "Witryna Firmowa, temat: admin")
                {
                    JwtService jwt = new JwtService();
                    var token = jwt.GenerujTokenJwt(smtpConfig["EmailDo"], 15);
                    var url = $"https://localhost:7231/opinie-klientow/admin?token={token}";

                    email.Subject = "Link do zarządzania opiniami";
                    email.Body = new TextPart("plain")
                    {
                        Text = $"Wygenerowany link do zarządzania opiniami (ważny 15 minut):\n{url}"
                    };
                }
                else
                {
                    email.Subject = temat;
                    email.Body = new TextPart("plain")
                    {
                        Text = $"Wiadomość od: {email_od}\n\n{message}"
                    };
                }

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
