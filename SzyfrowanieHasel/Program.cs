using System.Security.Cryptography;
using System.Text;

namespace SzyfrowanieHasla
{
    public class SzyfrowanieHasla
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("SmtpServiceKey1#");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("##SmtpServiceIV#");
        static void Main(string[] args)
        {
            Console.WriteLine("Podaj hasło, które ma zostać zaszyfrowane: ");
            var haslo = Console.ReadLine();
            Console.WriteLine($"Twoje zaszyfrowane hasło: {Encrypt(haslo)}");

        }

        public static string Encrypt(string haslo)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(haslo);
                            }
                            return Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
            catch
            {
                throw;
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
            catch
            {
                throw;
            }
        }
    }
}
