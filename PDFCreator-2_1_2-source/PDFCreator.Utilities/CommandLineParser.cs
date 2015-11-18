using System.Collections.Generic;

namespace pdfforge.PDFCreator.Utilities
{
    public class CommandLineParser
    {
        private readonly Dictionary<string, string> _args;

        public CommandLineParser(IEnumerable<string> args)
        {
            _args = AnalyzeCommandLine(args);
        }

        public bool HasArgument(string key)
        {
            return _args.ContainsKey(key.ToLower());
        }

        public string GetArgument(string key)
        {
            return _args[key.ToLower()];
        }

        private static Dictionary<string, string> AnalyzeCommandLine(IEnumerable<string> args)
        {
            var arguments = new Dictionary<string, string>();

            foreach (string arg in args)
            {
                if (string.IsNullOrEmpty(arg))
                    continue;
                
                char c = arg[0];
                if ((c != '/') && (c != '-'))
                    continue;

                string s = arg.Substring(1);
                int pos = s.IndexOf('=');

                if (pos < 0)
                {
                    arguments.Add(s.ToLower(), null);
                }
                else
                {
                    string[] argPair = s.Split(new[] {'='}, 2);
                    arguments.Add(argPair[0].ToLower(), argPair[1]);
                }
            }

            return arguments;
        }
    }
}