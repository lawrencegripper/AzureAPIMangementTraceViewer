using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIMTraceViewer.Shared
{
    public class EchoApiService
    {
        const string traceHeaderReceive = "ocp-apim-trace-location";
        const string traceHeaderSend = "ocp-apim-trace";
        HttpClient client;

        public EchoApiService()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add(traceHeaderSend, "true");
        }

        /// <summary>
        /// Issue a request to the APIM URL and return the response with trace information from header. 
        /// </summary>
        /// <param name="url">API url</param>
        /// <returns>HttpResponse and Trace information</returns>
        public async Task<TraceResponse> GetRequestWithTrace(string url)
        {
            TraceResponse response = new TraceResponse();
            try
            {
                Uri traceUri = Helpers.UriGuard(url);
                response.ResponseMessage = await GetApiResponse(traceUri);
                response.TraceString = await GetTrace(response.ResponseMessage);
                response.BodyContent = await response.ResponseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorDetails = ex;
            }

            return response;
        }


        /// <summary>
        /// This initiates a call to the API with the trace header added. 
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Response from the API, should contain trace header</returns>
        private async Task<HttpResponseMessage> GetApiResponse(Uri traceUri)
        {
            var result = await client.GetAsync(traceUri);

            //Note not checking for success code as want to view trace on failure too. 

            if (!Helpers.IsAPIMServer(result))
            {
                throw new InvalidOperationException("Service is not being hosted by APIM");
            }

            return result;
        }

        /// <summary>
        /// Fetches the json trace from the request. 
        /// </summary>
        /// <param name="message"></param>
        /// <returns>content of the trace</returns>
        private async Task<string> GetTrace(HttpResponseMessage message)
        {
            var url = string.Empty;

            if (!message.Headers.Any(x => x.Key == traceHeaderReceive))
            {
                throw new InvalidOperationException("APIM didn't return a tracking header");
            }

            var traceUrlHeader = message.Headers.Where(x => x.Key == traceHeaderReceive).FirstOrDefault();

            Uri traceUri = Helpers.UriGuard(traceUrlHeader.Value.First());

            var unformattedResponse = await client.GetStringAsync(traceUri);

            return Helpers.FormatJsonNicely(unformattedResponse);
        }



    }
}
