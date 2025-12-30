using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Translation.V2;
using static Google.Apis.Translate.v2.TranslationsResource;

namespace TranslateAPI.AppHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TranslateController : ControllerBase
    {
        public class TranslateRequest
        {
            public string text { get; set; }
        }

        [HttpPost("translate")]
        public async Task<IActionResult> Translate([FromBody] TranslateRequest req)
        {
            var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
            var client = TranslationClient.CreateFromApiKey(apiKey);
            var result = await client.TranslateTextAsync(req.text, "pl");
            return Ok(new { translatedText = result.TranslatedText });
        }
    }
}
