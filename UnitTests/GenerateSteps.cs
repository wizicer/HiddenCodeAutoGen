using Generator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TechTalk.SpecFlow;

namespace UnitTests
{
    [Binding]
    public class GenerateSteps
    {
        private string _sourceCode = null;
        private string _generatedCode = null;
        //private string _generatorName = null;

        [Given(@"I have source code:")]
        public void GivenIHaveSourceCode(string multilineText)
        {
            this._sourceCode = multilineText;
        }

        //[Given(@"I design to use (\w*) generator")]
        //public void GivenIDesignToUseDefaultGenerator(string generatorName)
        //{
        //    this._generatorName = generatorName;
        //}

        [When(@"I ask to generate")]
        public void WhenIAskToGenerate()
        {
            //if (this._generatorName == "default")
            //{
            //    this._generatedCode = g.Gen(this._sourceCode);
            //}
            //else
            //{
            //    var entitygen = new AutoGenEntity();
            //    var autogen = new WpfInpcGenerator();
            //    this._generatedCode = g.Gen(this._sourceCode, s => s == "" ? (IGenerator)autogen : entitygen) ?? "";
            //}
            //this._generatedCode = g.Gen(this._sourceCode, s => (IGenerator)( new CommandAutoGen()) );
            this._generatedCode = Gen(this._sourceCode) ?? string.Empty;
        }

        [When(@"Remove comments before namespace")]
        public void WhenRemoveCommentsBeforeNamespace()
        {
            this._generatedCode = this._generatedCode.Substring(this._generatedCode.IndexOf("namespace"));
        }

        [Then(@"the result should be:")]
        public void ThenTheResultShouldBe(string multilineText)
        {
            var expectedCode = multilineText;
            Assert.AreEqual(Standardize(expectedCode), Standardize(this._generatedCode ?? string.Empty));
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