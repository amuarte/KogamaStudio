using Il2CppSystem.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace KogamaStudio.Translator
{
    internal class MessageTranslator
    {
        public static async Task<string> Translate(string text)
        {
            return await TranslateMessage(text);
        }

        private static async Task<string> TranslateMessage(string text)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var url = $"https://api.mymemory.translated.net/get?q={Uri.EscapeDataString(text)}&langpair=en|pl";
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                // Parse JSON
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                return data.responseData.translatedText;
            }
        }
    }
}
