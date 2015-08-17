using APIMTraceViewer.Shared;
using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMTraveViewer.CmdLine
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new ArgOptions();
            if (Parser.Default.ParseArguments(args, options))
            {

                Console.WriteLine("Request to Trace: " + options.RequestUri);
                Console.WriteLine("With subscription key: " + options.SubscriptionKey);
                Console.WriteLine("Output folder for result: " + options.OutputFolder);

                IssueRequest(options.RequestUri, options.SubscriptionKey, options.OutputFolder).Wait();
            }
        }

        private static async Task IssueRequest(string requestUri, string subKey, string folder)
        {

            //Issue a request and get trace
            using (TraceViewerService echoTester = new TraceViewerService(subKey))
            {
                var result = await echoTester.GetRequestWithTrace(requestUri);
                await WriteTraceToFile(folder, result);
                await WriteRequestDetailsToFile(requestUri, folder, result);
            }
        }

        private static async Task WriteRequestDetailsToFile(string requestUri, string folder, TraceResponse result)
        {
            var request = string.Format("request-{0:yyyy-MM-dd_hh-mm-ss-tt}.txt", DateTime.Now);
            var requestFilePath = Path.Combine(folder, request);
            using (StreamWriter outputFile = new StreamWriter(requestFilePath))
            {
                await outputFile.WriteLineAsync("-----------RequestUrl-----------");
                await outputFile.WriteLineAsync(requestUri);
                await outputFile.WriteLineAsync("-----------Body-----------");
                await outputFile.WriteLineAsync(result.BodyContent);
                await outputFile.WriteLineAsync("-----------Headers-----------");
                await outputFile.WriteLineAsync(result.ResponseMessage.Headers.ToString());
            }
        }

        private static async Task WriteTraceToFile(string folder, TraceResponse result)
        {
            var trace = string.Format("trace-{0:yyyy-MM-dd_hh-mm-ss-tt}.json", DateTime.Now);
            var traceFilePath = Path.Combine(folder, trace);

            using (StreamWriter outputFile = new StreamWriter(traceFilePath))
            {
                //Show errors, if any
                if (result.HasError)
                {
                    var error = string.Format("An error occured '{0}' Exception Details: '{1}'", result.ErrorDetails.Message, result.ErrorDetails.ToString());
                    Console.WriteLine(error);
                    await outputFile.WriteLineAsync(error);
                }

                await outputFile.WriteLineAsync(result.TraceString);
            }
        }
    }
}
