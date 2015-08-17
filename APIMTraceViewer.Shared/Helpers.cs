using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APIMTraceViewer.Shared
{
    class Helpers
    {
        const string apiManagementServerHeader = "Microsoft-HTTPAPI/2.0";

        public static Uri UriGuard(string url)
        {
            Uri outputUri;
            if (url == null || !Uri.TryCreate(url, UriKind.Absolute, out outputUri))
            {
                throw new ArgumentException("Url invalid or not set");
            }

            return outputUri;
        }

        /// <summary>
        /// Bit of a hack to format the json nicely onto multiple lines. 
        /// </summary>
        /// <param name="traceUri"></param>
        /// <returns></returns>
        public static string FormatJsonNicely(string unformattedResponse)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(unformattedResponse);

            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }
    }
}
