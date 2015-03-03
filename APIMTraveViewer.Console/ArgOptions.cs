using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMTraveViewer.CmdLine
{
    class ArgOptions
    {
        [ParserState]
        public IParserState LastParserState { get; set; }
        [Option('u', "url", Required = false, HelpText = "Url of the request to trace.")]
        public string RequestUri { get; set; }

        [Option('f', "file", Required = false, HelpText = "Output folder where traces will be saved.")]
        public string OutputFolder { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            foreach (var error in this.LastParserState.Errors)
            {
                Console.WriteLine("Error on input {0} IsRequired {1} IsInvalidFormat {2}", error.BadOption.LongName, error.ViolatesRequired, error.ViolatesFormat);
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();

            return HelpText.AutoBuild(this);
        }
    }
}
