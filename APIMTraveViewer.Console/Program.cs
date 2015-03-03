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

            [ParserState]
            public IParserState LastParserState { get; set; }


            [Option('u', "url", Required = false, HelpText = "Input files to be processed.")]
            public string RequestUri { get; set; }

            [Option('f', "file", Required = false, HelpText = "Output folder where traces will be saved.")]
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

                Console.WriteLine("Request to Trace: " + options.RequestUri);
                Console.WriteLine("Output folder for result: " + options.OutputFolder);

                IssueRequest(options.RequestUri, options.OutputFolder).Wait();
            }

            Console.ReadLine();
        }

        private static async Task IssueRequest(string requestUri, string folder)
        {

            //Issue a request and get trace
            EchoApiService echoTester = new EchoApiService();
            var result = await echoTester.GetRequestWithTrace(requestUri);

            var trace = string.Format("trace-{0:yyyy-MM-dd_hh-mm-ss-tt}.json", DateTime.Now);
            var traceFilePath = Path.Combine(folder, trace);

            using (StreamWriter outputFile = new StreamWriter(traceFilePath))
            {
                //Could be MVVM but feels like overkill given the simplicity.

                //Show errors, if any
                if (result.HasError)
                {
                    await outputFile.WriteLineAsync(string.Format("An error occured '{0}' Exception Details: '{1}'", result.ErrorDetails.Message, result.ErrorDetails.ToString()));
                }

                await outputFile.WriteAsync(result.TraceString);
            }

            var request = string.Format("request-{0:yyyy-MM-dd_hh-mm-ss-tt}.txt", DateTime.Now);
            var requestFilePath = Path.Combine(folder, request);
            using (StreamWriter outputFile = new StreamWriter(requestFilePath))
            {
                await outputFile.WriteLineAsync("-----------RequestUrl-----------");
                await outputFile.WriteAsync(requestUri);
                await outputFile.WriteLineAsync("-----------Body-----------");
                await outputFile.WriteAsync(result.BodyContent);
                await outputFile.WriteLineAsync("-----------Headers-----------");
                await outputFile.WriteAsync(result.ResponseMessage.Headers.ToString());
            }
        }

    }
}
