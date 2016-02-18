﻿namespace Generator
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
        private class ErrorGenerator : IGenerator
        {
            private string errorMessage;
            public ErrorGenerator(string msg)
            {
                this.errorMessage = msg;
            }
            public string Generate(string className, IList<IList<string>> parameters)
            {
                return this.errorMessage;
            }
        }

        public static string Gen(string inputFileContents)
        {
            try
            {
                var source = RemoveComments(inputFileContents);
                return Gen(source, genName =>
                {
                    try
                    {
                        var file = Path.Combine(
                            (new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)).Directory.FullName,
                            string.Format("AutoGen{0}.cs", genName));
                        if (File.Exists(file))
                        {
                            var eval = new MonoEvaluator();
                            eval.Reset();
                            var script = eval.LoadFile<IGenerator>(file);
                            return script;
                        }
                        return null;
                    }
                    catch (Exception inex)
                    {
                        return new ErrorGenerator(string.Format("{0}/* Error when using [AutoGen{1}.cs]:{0}{2}{0}*/{0}", Environment.NewLine, genName, inex.Message));
                    }
                });
            }
            catch (Exception ex)
            {
                return string.Format("/*\r\n{0}\r\n*/", ex.Message);
            }
        }

        public static string Gen(string inputFileContents, Func<string, IGenerator> getGenerator)
        {
            string className;
            var head = GetHead(inputFileContents, out className);
            if (head == null) return null;
            var par = GetList(inputFileContents);

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

            var clswithatt = clRx.Match(bstrInputFileContents).Value;
            return atRx.Matches(clswithatt)
                .OfType<Match>()
                .Select(m => new AutoGenInfo()
                {
                    Name = m.Groups["name"].Value,
                    Parameters = m.Groups["data"].Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                })
                .ToList();
        }

        private static string GetHead(string bstrInputFileContents, out string className)
        {
            Regex nsRx = new Regex("namespace (.*)\r\n\\{");
            Regex usRx = new Regex("using .*;");
            //Regex clRx = new Regex(@"(?<class>.*class \w*)");
            Regex pclRx = new Regex(@"(?<class>.*partial class \w*)");
            Regex pclnmRx = new Regex(@".*partial class (?<class>\w*)");
            Regex atRx = new Regex(@"^ *\[AutoGen(?<name>[\w\d]*)\((?<data>.*)\)\]", RegexOptions.Multiline);

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