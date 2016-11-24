using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Models
{
    public static class WebServiceHandler
    {
        public static async Task<string> GetResultFromAPIAsync(string url)
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync(url);
            return json;
        }

    }
}
