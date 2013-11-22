using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IcerWPFSmartGen;

namespace UnitTests
{
    [Binding]
    public class GenerateSteps
    {
        private string _sourceCode = null;
        private string _generatedCode = null;
        private string _generatorName = null;


        [Given(@"I have source code:")]
        public void GivenIHaveSourceCode(string multilineText)
        {
            this._sourceCode = multilineText;
        }

        [Given(@"I design to use (\w*) generator")]
        public void GivenIDesignToUseDefaultGenerator(string generatorName)
        {
            this._generatorName = generatorName;
        }

        [When(@"I ask to generate")]
        public void WhenIAskToGenerate()
        {
            Generator g = new Generator();
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
            this._generatedCode = g.Gen(this._sourceCode) ?? string.Empty;

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

        private string Standardize(string input)
        {
            return input.Trim().Replace("\r", "");
        }
    }
}
