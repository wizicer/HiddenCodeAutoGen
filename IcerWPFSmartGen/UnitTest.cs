// -----------------------------------------------------------------------
// <copyright file="UnitTest.cs" company="Honeywell">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
#if DEBUG
namespace IcerWPFSmartGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using System.Diagnostics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestFixture]
    public class UnitTest
    {
        [Test]
        public void GenTest()
        {
            Generator g = new Generator();
            var source = @"// -----------------------------------------------------------------------
// <copyright file=""WireEditingViewModel.cs"" company=""Honeywell"">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Chart.VMWire.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WireEditingViewModel : WireBaseViewModel
    {
        
    }
}";
            var ret = g.GenerateDebug(source);

            var expect = @"";

            Console.WriteLine(ret);
            //ret = ret.Substring(ret.IndexOf("namespace"));
            //Assert.AreEqual(expect.Trim(), ret.Trim());
            Assert.AreEqual(null, ret);
        }     
        
        [Test]
        public void GenTest2()
        {
            Generator g = new Generator();
            var source = @"// -----------------------------------------------------------------------
// <copyright file=""WireBaseViewModel.cs"" company=""Honeywell"">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Chart.VMWire.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AutoGenAttribute : Attribute
    {
        public string FieldName { get; private set; }
        public string BackFieldName { get; private set; }
        public Type FieldType { get; private set; }
        public object DefaultValue { get; private set; }

        public AutoGenAttribute(string fieldName, Type fieldType, object defaultValue)
            : this(fieldName, ""_"" + fieldName.Substring(0, 1).ToLower() + fieldName.Substring(1), fieldType, defaultValue)
        {
        }

        public AutoGenAttribute(string fieldName, string backFieldName, Type fieldType, object defaultValue)
        {
            this.FieldName = fieldName;
            this.BackFieldName = backFieldName;
            this.FieldType = fieldType;
            this.DefaultValue = defaultValue;
        }
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AutoGen(""WireData"", typeof(SquarePolyline), null)]
    [AutoGen(""Visibility"", typeof(Visibility), Visibility.Hidden)]
    public partial class WireBaseViewModel : ViewModelBase
    {
    }
}";
            var ret = g.GenerateDebug(source);

            var expect = @"namespace Chart.VMWire.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;

    public partial class WireBaseViewModel
    {
        /// <summary>
        /// Field which backs the WireData property
        /// </summary>
        private SquarePolyline _wireData = null;

        /// <summary>
        /// Gets / sets the WireData value
        /// </summary>
        public SquarePolyline WireData
        {
            get { return _wireData; }
            set
            {
                if (_wireData == value) return;
                _wireData = value;

                OnWireDataChanged(value);
                OnPropertyChanged(""WireData"");
            }
        }
    
        /// <summary>
        /// Invoked when the value of WireData changes
        /// </summary>
        partial void OnWireDataChanged(SquarePolyline value);
        /// <summary>
        /// Field which backs the Visibility property
        /// </summary>
        private Visibility _visibility = Visibility.Hidden;

        /// <summary>
        /// Gets / sets the Visibility value
        /// </summary>
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                if (_visibility == value) return;
                _visibility = value;

                OnVisibilityChanged(value);
                OnPropertyChanged(""Visibility"");
            }
        }
    
        /// <summary>
        /// Invoked when the value of Visibility changes
        /// </summary>
        partial void OnVisibilityChanged(Visibility value);
    }
}";

            Console.WriteLine(ret);
            ret = ret.Substring(ret.IndexOf("namespace"));
            Assert.AreEqual(expect.Trim(), ret.Trim());
        }

        [Test]
        public void GenTest3()
        {
            Generator g = new Generator();
            var source = @"// -----------------------------------------------------------------------
// <copyright file=""WireBaseViewModel.cs"" company=""Honeywell"">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Chart.VMWire.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AutoGen(""WireData"", typeof(string), ""null"")]
    [AutoGen(""Visibility"", typeof(Visibility), ""Visibility.Hidden"")]
    public partial class WireBaseViewModel : ViewModelBase
    {
    }
}";
            var ret = g.GenerateDebug(source);

            var expect = @"namespace Chart.VMWire.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;

    public partial class WireBaseViewModel
    {
        /// <summary>
        /// Field which backs the WireData property
        /// </summary>
        private string _wireData = ""null"";

        /// <summary>
        /// Gets / sets the WireData value
        /// </summary>
        public string WireData
        {
            get { return _wireData; }
            set
            {
                if (_wireData == value) return;
                _wireData = value;

                OnWireDataChanged(value);
                OnPropertyChanged(""WireData"");
            }
        }
    
        /// <summary>
        /// Invoked when the value of WireData changes
        /// </summary>
        partial void OnWireDataChanged(string value);
        /// <summary>
        /// Field which backs the Visibility property
        /// </summary>
        private Visibility _visibility = Visibility.Hidden;

        /// <summary>
        /// Gets / sets the Visibility value
        /// </summary>
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                if (_visibility == value) return;
                _visibility = value;

                OnVisibilityChanged(value);
                OnPropertyChanged(""Visibility"");
            }
        }
    
        /// <summary>
        /// Invoked when the value of Visibility changes
        /// </summary>
        partial void OnVisibilityChanged(Visibility value);
    }
}";

            Console.WriteLine(ret);
            ret = ret.Substring(ret.IndexOf("namespace"));
            Assert.AreEqual(expect.Trim(), ret.Trim());
        }
    }
}
#endif