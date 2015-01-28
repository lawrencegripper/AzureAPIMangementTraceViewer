using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APIMTraceViewer
{
    public class TraceResponse
    {
        public TraceResponse()
        {
            ResponseMessage = new HttpResponseMessage();
            TraceString = String.Empty;
        }
        public HttpResponseMessage ResponseMessage { get; set; }
        public string TraceString { get; set; }
        public bool HasError { get; set; }
        public Exception ErrorDetails { get; set; }
        public string BodyContent { get; set; }

    }
}
