namespace UnitTests
{
    using Generator;
    using NUnit.Framework;
    using System;
    using System.IO;
    using TechTalk.SpecFlow;

    [Binding]
    public class GenerateSteps
    {
        private string _sourceCode = null;
        private FileInfo _sourceCodeFileInfo = null;
        private string _generatedCode = null;
        private string _basePath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"testcases\");

        [When(@"I ask to generate")]
        public void WhenIAskToGenerate()
        {
            this._generatedCode = GeneratorAgent.Gen(this._sourceCodeFileInfo.FullName, this._sourceCode, null) ?? string.Empty;
        }

        [Given(@"I have source code in file (.*)")]
        public void GivenIHaveSourceCodeInFile(string filename)
        {
            var fi = new FileInfo(Path.Combine(this._basePath, filename));
            var source = File.ReadAllText(fi.FullName);
            this._sourceCodeFileInfo = fi;
            this._sourceCode = source;
        }

        [Then(@"the result should be like in file (.*)")]
        public void ThenTheResultShouldBeLikeInFile(string filename)
        {
            var expectedCode = File.ReadAllText(Path.Combine(this._basePath, filename));
            Assert.AreEqual(Standardize(expectedCode), Standardize(this._generatedCode ?? string.Empty));
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