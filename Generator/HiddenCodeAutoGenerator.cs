namespace Generator
{
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.TextTemplating.VSHost;
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    [ComVisible(true)]
    [Guid("DC5BEA27-51D3-4330-9CB3-CB2F5FF7A014")]
    [ProvideObject(typeof(HiddenCodeAutoGenerator))]
    [ProvideCodeGeneratorExtension("HiddenCodeAutoGenerator", ".cs")]
    [VisualStudio.CodeGeneratorRegistration(typeof(HiddenCodeAutoGenerator), "HiddenCodeAutoGenerator", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}")]
    public class HiddenCodeAutoGenerator : CustomToolBase
    {
        #region IVsSingleFileGenerator Members

        protected override string DefaultExtension()
        {
            return ".autogen.cs";
        }

        protected override byte[] Generate(string inputFilePath, string inputFileContents, string defaultNamespace, IVsGeneratorProgress progressCallback)
        {
            if (inputFileContents == null)
            {
                throw new ArgumentNullException(nameof(inputFileContents));
            }

            var gen = GeneratorAgent.Gen(inputFileContents);
            return gen == null ? new byte[] { } : Encoding.UTF8.GetBytes(gen);
        }

        #endregion IVsSingleFileGenerator Members
    }
}