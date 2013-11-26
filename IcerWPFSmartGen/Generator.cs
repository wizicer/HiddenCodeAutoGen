﻿namespace IcerWPFSmartGen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using CSScriptLibrary;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;

    [ComVisible(true)]
    [Guid("DC5BEA27-51D3-4330-9CB3-CB2F5FF7A014")]
    public class Generator : IVsSingleFileGenerator
    {
        #region IVsSingleFileGenerator Members

        public int DefaultExtension(out string pbstrDefaultExtension)
        {
            pbstrDefaultExtension = ".autogen.cs";
            return VSConstants.S_OK;
        }

        public int Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace,
            IntPtr[] rgbOutputFileContents, out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
        {
            if (bstrInputFileContents == null)
            {
                throw new ArgumentNullException("bstrInputFileContents");
            }

            var gen = this.Gen(bstrInputFileContents);

            if (gen == null)
            {
                pcbOutput = 0;
                return VSConstants.S_FALSE;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(gen);
            int length = bytes.Length;

            rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(length);
            Marshal.Copy(bytes, 0, rgbOutputFileContents[0], length);

            pcbOutput = (uint)length;

            return VSConstants.S_OK;
        }

        public string Gen(string bstrInputFileContents)
        {
            try
            {
                var source = RemoveComments(bstrInputFileContents);
                return Gen(source, genName =>
                {
                    //var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("AutoGen{0}.cs", genName));
                    var file = Path.Combine(
                        (new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)).Directory.FullName,
                        string.Format("AutoGen{0}.cs", genName));
                    if (File.Exists(file))
                    {
                        IGenerator script = CSScript.Evaluator.LoadFile<IGenerator>(file);
                        return script;
                    }
                    return null;
                });
            }
            catch (Exception ex)
            {
                return string.Format("/*\r\n{0}\r\n*/", ex.Message);
            }
        }

        private string Gen(string bstrInputFileContents, Func<string, IGenerator> getGenerator)
        {
            string className;
            var head = GetHead(bstrInputFileContents, out className);
            if (head == null) return null;
            var par = GetList(bstrInputFileContents);

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

        private string RemoveComments(string content)
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

        private List<AutoGenInfo> GetList(string bstrInputFileContents)
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

        private string GetHead(string bstrInputFileContents, out string className)
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
// Auto generated by Icer WPF Smart Generator
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

        private string GetFoot()
        {
            StringBuilder fs = new StringBuilder();

            // end
            fs.AppendLine("    }");
            fs.AppendLine("}");
            return fs.ToString();
        }

        #endregion IVsSingleFileGenerator Members
    }

    public class AutoGenInfo
    {
        public string Name { get; set; }

        public IList<string> Parameters { get; set; }
    }
}