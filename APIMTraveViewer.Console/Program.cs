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
        class Options
        {
            [Option('r', "request", Required = true, HelpText = "Input files to be processed.")]
            public Uri RequestUri { get; set; }

            [Option('f', "file", Required = true, HelpText = "Output folder where traces will be saved.")]
            public string OutputFolder { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this);
            }

        }

        static void Main(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {

                Console.WriteLine("Request to Trace: " + options.RequestUri.ToString());
                Console.WriteLine("Output folder for result: " + options.OutputFolder);

                IssueRequest(options.RequestUri.ToString(), options.OutputFolder).Wait();
            }
        }

        private static async Task IssueRequest(string requestUri, string folder)
        {
            var n = string.Format("text-{0:yyyy-MM-dd_hh-mm-ss-tt}.json", DateTime.Now);
            var filePath = Path.Combine(folder, n);
            using (StreamWriter outputFile = new StreamWriter(filePath))
            {
                //Could be MVVM but feels like overkill given the simplicity.
                EchoApiService echoTester = new EchoApiService();

                //Issue a request and get trace
                var result = await echoTester.GetRequestWithTrace(requestUri);

                //Show errors, if any
                if (result.HasError)
                {
                    await outputFile.WriteLineAsync(string.Format("An error occured '{0}' Exception Details: '{1}'", result.ErrorDetails.Message, result.ErrorDetails.ToString()));
                }

                await outputFile.WriteLineAsync("Trace");
                await outputFile.WriteAsync(result.TraceString);
                await outputFile.WriteLineAsync("Body");
                await outputFile.WriteAsync(result.BodyContent);
                await outputFile.WriteLineAsync("Headers");
                await outputFile.WriteAsync(result.ResponseMessage.Headers.ToString());

            }



        }
    }
}
