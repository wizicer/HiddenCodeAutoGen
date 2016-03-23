namespace Generator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using CSScriptLibrary;
    using Contracts;
    public class GeneratorAgent
    {
        [Serializable]
        public class GenerationException : Exception
        {
            public GenerationException() { }
            public GenerationException(string message) : base(message) { }
            public GenerationException(string message, Exception inner) : base(message, inner) { }
            protected GenerationException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context)
            { }
        }

        public static string Gen(string inputFilePath, string inputFileContents, string defaultNamespace)
        {
            try
            {
                var source = RemoveComments(inputFileContents);

                Dictionary<string, IGenerator> dictGen = GetGenerators(inputFilePath, inputFileContents);
                Func<string, IGenerator> getGenerator = (s) => dictGen.ContainsKey(s) ? dictGen[s] : null;

                string className;
                var head = GetHead(source, out className);
                if (head == null) return null;
                var par = GetList(source);

                var str = string.Empty;

                foreach (var genName in par.Select(agi => agi.Name).Distinct())
                {
                    var ps = par.Where(agi => agi.Name == genName).Select(agi => agi.Parameters).ToList();
                    var generator = getGenerator(genName);
                    if (generator != null)
                    {
                        str += generator.Generate(className, ps);
                    }
                }

                return head + str + GetFoot();
            }
            catch (GenerationException ex)
            {
                return string.Format("{0}/* {1}{0}*/{0}", Environment.NewLine, ex.Message);
            }
        }

        private static Dictionary<string, IGenerator> GetGenerators(string inputFilePath, string inputFileContents)
        {
            var agRx = new Regex("// <AutoGen src=\"(?<path>.*)\" />");

            return agRx.Matches(inputFileContents)
                .OfType<Match>()
                .Select(m =>
                {
                    try
                    {
                        var fi = new FileInfo(inputFilePath);
                        var file = Path.Combine(fi.DirectoryName, m.Groups["path"].Value);
                        if (File.Exists(file))
                        {
                            var script = CSScript.Evaluator.LoadFile<IGenerator>(file);
                            return script;
                        }

                        throw new GenerationException(string.Format("Error as file [{0}] not exist", file));
                    }
                    catch (Exception inex)
                    {
                        throw new GenerationException(string.Format("Error when parsing [{1}]:{0}{2}",
                            Environment.NewLine, inputFilePath, inex.Message), inex);
                    }
                })
                .ToDictionary(g => g.AttributeName);
        }

        public static string RemoveComments(string content)
        {
            var blockComments = @"/\*(.*?)\*/";
            var lineComments = @"//(.*?)(\r?\n|$)";
            //var strings = @"""((\\[^\n]|[^""\n])*)""";
            //var verbatimStrings = @"@(""[^""]*"")+";

            string noComments = Regex.Replace(
                content,
                blockComments + "|" + lineComments,// + "|" + strings + "|" + verbatimStrings,
                Environment.NewLine,
                RegexOptions.Singleline);

            return noComments;
        }

        private static List<AutoGenInfo> GetList(string bstrInputFileContents)
        {
            Regex atRx = new Regex(@"^\s*\[AutoGen(?<name>[\w\d]*)\((?<data>.*)\)\]", RegexOptions.Multiline);
            Regex clRx = new Regex(@"( *\[[\w\d]*\(.*\)\]\s*){1,}
(?<class>.*partial class \w*)");
            var dataRx = new Regex(@"(?:^|, ?)(?<par>\""(?:[^\""]+|\""\"")*\""|[^,]*)");

            var clswithatt = clRx.Match(bstrInputFileContents).Value;
            return atRx.Matches(clswithatt)
                .OfType<Match>()
                .Select(m => new AutoGenInfo()
                {
                    Name = m.Groups["name"].Value,
                    Parameters = dataRx.Matches(m.Groups["data"].Value).OfType<Match>().Select(_ => _.Groups["par"].Value).ToList(),
                })
                .ToList();
        }

        private static string GetHead(string bstrInputFileContents, out string className)
        {
            Regex nsRx = new Regex("namespace (.*)\r\n\\{");
            Regex usRx = new Regex("using .*;");
            //Regex clRx = new Regex(@"(?<class>.*class \w*)");
            Regex pclRx = new Regex(@"(?<class>.*partial class [\w<>, ]*)");
            //Regex pclRx = new Regex(@"(?<class>.*partial class \w*)");
            Regex pclnmRx = new Regex(@".*partial class (?<class>[\w<>, ]*)");
            //Regex atRx = new Regex(@"^ *\[AutoGen(?<name>[\w\d]*)\((?<data>.*)\)\]", RegexOptions.Multiline);

            className = null;

            //if (clRx.Matches(bstrInputFileContents).Count > 1)
            //{
            //    throw new Exception("more than one classes found in this file, this is not allowed!");
            //}

            var cls = pclRx.Match(bstrInputFileContents).Groups["class"].Value;
            className = pclnmRx.Match(bstrInputFileContents).Groups["class"].Value;
            if (string.IsNullOrEmpty(cls)) return null;
            StringBuilder fs = new StringBuilder();

            // header
            fs.AppendLine(@"// -----------------------------------------------------------------------
// Auto generated by Hidden Code Auto Generator
// Don't modify this file manually!
// <auto-generated />
// -----------------------------------------------------------------------
");

            // namespace
            fs.AppendLine(nsRx.Match(bstrInputFileContents).Value);

            // using
            fs.AppendLine(usRx.Matches(bstrInputFileContents)
                .OfType<Match>()
                .Aggregate("", (str, m) => string.Format("{0}    {1}\r\n", str, m.Value)));

            // class header
            fs.AppendLine(string.Format("    {0}", cls.TrimStart()));

            // class start
            fs.AppendLine("    {");

            return fs.ToString();
        }

        private static string GetFoot()
        {
            StringBuilder fs = new StringBuilder();

            // end
            fs.AppendLine("    }");
            fs.AppendLine("}");
            return fs.ToString();
        }

        private class AutoGenInfo
        {
            public string Name { get; set; }

            public IList<string> Parameters { get; set; }
        }
    }
}