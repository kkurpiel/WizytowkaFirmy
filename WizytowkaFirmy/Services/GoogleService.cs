using NLog;
using System.Text.Json.Nodes;

namespace WizytowkaFirmy.Services
{
    public class GoogleService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public async Task<bool> WeryfikacjareCAPTCHA(string odpowiedz, string serverKey)
        {
            try
            {
                if (string.IsNullOrEmpty(odpowiedz))
                {
                    return false;
                }
                using (var httpClient = new HttpClient())
                {
                    var url = $"https://www.google.com/recaptcha/api/siteverify";
                    MultipartFormDataContent content = new MultipartFormDataContent();
                    content.Add(new StringContent(odpowiedz), "response");
                    content.Add(new StringContent(serverKey), "secret");

                    var result = await httpClient.PostAsync(url, content);

                    if (result.IsSuccessStatusCode)
                    {
                        var strResponse = await result.Content.ReadAsStringAsync();
                        var jsonResponse = JsonNode.Parse(strResponse);
                        if (jsonResponse != null)
                        {
                            var success = ((bool?)jsonResponse["success"]);
                            if (success != null && success == true)
                            {
                                logger.Info("Weryfikacja reCAPTCHA przebiegła prawidłowo");
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            { 
                logger.Error(ex.Message);
                return false;
            }
        }
    }
}
