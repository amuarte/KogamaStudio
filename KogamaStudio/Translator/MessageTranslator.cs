using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading;

namespace KogamaStudio.Translator
{
    internal class MessageTranslator
    {
        private static HttpClient client = new HttpClient();
        public static string TargetLanguage = "en";

        public static string LastTranslation = "";
        public static bool TranslationReady = false;

        public static void Translate(string text, string targetLang = null)
        {
            if (targetLang == null) targetLang = TargetLanguage;

            new Thread(() =>
            {
                try
                {
                    var res = client.PostAsync("https://kogamastudio-production.up.railway.app/translate/translate",
                        new StringContent(JsonConvert.SerializeObject(new { text, targetLanguage = targetLang }), Encoding.UTF8, "application/json")).Result;
                    dynamic data = JsonConvert.DeserializeObject(res.Content.ReadAsStringAsync().Result);
                    LastTranslation = data.translatedText.ToString();
                    TranslationReady = true;
                }
                catch { }
            }).Start();
        }
    }
}