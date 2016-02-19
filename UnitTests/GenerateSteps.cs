namespace UnitTests
{
    using Generator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;
    using TechTalk.SpecFlow;

    [Binding]
    public class GenerateSteps
    {
        private string _sourceCode = null;
        private string _generatedCode = null;
        private string _generatedCodeInternal = null;
        private string _basePath = @"testcases\";

        [When(@"I ask to generate")]
        public void WhenIAskToGenerate()
        {
            this._generatedCodeInternal = Gen(this._sourceCode) ?? string.Empty;
            this._generatedCode = GeneratorAgent.Gen(this._sourceCode) ?? string.Empty;
        }

        [Given(@"I have source code in file (.*)")]
        public void GivenIHaveSourceCodeInFile(string filename)
        {
            var source = File.ReadAllText(Path.Combine(this._basePath, filename));
            this._sourceCode = source;
        }

        [Then(@"the result should be like in file (.*)")]
        public void ThenTheResultShouldBeLikeInFile(string filename)
        {
            var expectedCode = File.ReadAllText(Path.Combine(this._basePath, filename));
            Assert.AreEqual(Standardize(expectedCode), Standardize(this._generatedCode ?? string.Empty));
            Assert.AreEqual(Standardize(expectedCode), Standardize(this._generatedCodeInternal ?? string.Empty));
        }

        private static string Gen(string inputFileContents)
        {
            try
            {
                var source = GeneratorAgent.RemoveComments(inputFileContents);
                return GeneratorAgent.Gen(source, genName =>
                {
                    switch (genName)
                    {
                        case "Command":
                            return new CommandAutoGen();

                        case "DP":
                            return new DPGenerator();

                        case "Entity":
                            return new AutoGenEntity();

                        case "":
                            return new WpfInpcGenerator();

                        default:
                            return null;
                    }
                });
            }
            catch (Exception ex)
            {
                return string.Format("/*\r\n{0}\r\n*/", ex.ToString());
            }
        }

        private string Standardize(string input)
        {
            return input.Trim()
                //.Replace("\t", "")
                .Replace(" ", "")
                .Replace("\r", "");
        }
    }
}