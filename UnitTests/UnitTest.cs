////namespace UnitTests
////{
////    using System;
////    using System.Linq;
////    using IcerWPFSmartGen;
////    using Microsoft.VisualStudio.TestTools.UnitTesting;
////    using System.Diagnostics;

////    [TestClass]
////    public class UnitTest
////    {
////        [TestMethod]
////        public void GenTest()
////        {
////            Generator g = new Generator();
////            var source = @"// -----------------------------------------------------------------------
////// <copyright file=""WireEditingViewModel.cs"" company=""Honeywell"">
////// TODO: Update copyright text.
////// </copyright>
////// -----------------------------------------------------------------------
////
////namespace Chart.VMWire.ViewModel
////{
////    using System;
////    using System.Collections.Generic;
////    using System.Linq;
////    using System.Text;
////
////    /// <summary>
////    /// TODO: Update summary.
////    /// </summary>
////    public class WireEditingViewModel : WireBaseViewModel
////    {
////    }
////}";
////            var ret = g.Gen(source);

////            var expect = @"";

////            Console.WriteLine(ret);

////            //ret = ret.Substring(ret.IndexOf("namespace"));
////            //Assert.AreEqual(expect.Trim(), ret.Trim());
////            Assert.AreEqual(null, ret);
////        }

////        [TestMethod]
////        public void GenTest2()
////        {
////            Generator g = new Generator();
////            var source = @"// -----------------------------------------------------------------------
////// <copyright file=""WireBaseViewModel.cs"" company=""Honeywell"">
////// TODO: Update copyright text.
////// </copyright>
////// -----------------------------------------------------------------------
////
////namespace Chart.VMWire.ViewModel
////{
////    using System;
////    using System.Collections.Generic;
////    using System.Linq;
////    using System.Text;
////    using System.Windows;
////
////    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
////    public sealed class AutoGenAttribute : Attribute
////    {
////        public string FieldName { get; private set; }
////        public string BackFieldName { get; private set; }
////        public Type FieldType { get; private set; }
////        public object DefaultValue { get; private set; }
////
////        public AutoGenAttribute(string fieldName, Type fieldType, object defaultValue)
////            : this(fieldName, ""_"" + fieldName.Substring(0, 1).ToLower() + fieldName.Substring(1), fieldType, defaultValue)
////        {
////        }
////
////        public AutoGenAttribute(string fieldName, string backFieldName, Type fieldType, object defaultValue)
////        {
////            this.FieldName = fieldName;
////            this.BackFieldName = backFieldName;
////            this.FieldType = fieldType;
////            this.DefaultValue = defaultValue;
////        }
////    }
////
////    /// <summary>
////    /// TODO: Update summary.
////    /// </summary>
////    [AutoGen(""WireData"", typeof(SquarePolyline), null)]
////    [AutoGen(""Visibility"", typeof(Visibility), Visibility.Hidden)]
////    public partial class WireBaseViewModel : ViewModelBase
////    {
////    }
////}";
////            var ret = g.Gen(source);

////            var expect = @"namespace Chart.VMWire.ViewModel
////{
////    using System;
////    using System.Collections.Generic;
////    using System.Linq;
////    using System.Text;
////    using System.Windows;
////
////    public partial class WireBaseViewModel
////    {
////        /// <summary>
////        /// Field which backs the WireData property
////        /// </summary>
////        private SquarePolyline _wireData = null;
////
////        /// <summary>
////        /// Gets / sets the WireData value
////        /// </summary>
////        public SquarePolyline WireData
////        {
////            get { return _wireData; }
////            set
////            {
////                if (_wireData == value) return;
////                _wireData = value;
////
////                OnWireDataChanged(value);
////                OnPropertyChanged(""WireData"");
////            }
////        }
////
////        /// <summary>
////        /// Invoked when the value of WireData changes
////        /// </summary>
////        partial void OnWireDataChanged(SquarePolyline value);
////
////        /// <summary>
////        /// Field which backs the Visibility property
////        /// </summary>
////        private Visibility _visibility = Visibility.Hidden;
////
////        /// <summary>
////        /// Gets / sets the Visibility value
////        /// </summary>
////        public Visibility Visibility
////        {
////            get { return _visibility; }
////            set
////            {
////                if (_visibility == value) return;
////                _visibility = value;
////
////                OnVisibilityChanged(value);
////                OnPropertyChanged(""Visibility"");
////            }
////        }
////
////        /// <summary>
////        /// Invoked when the value of Visibility changes
////        /// </summary>
////        partial void OnVisibilityChanged(Visibility value);
////    }
////}";

////            //Console.WriteLine(ret);
////            //ret = ret.Substring(ret.IndexOf("namespace"));
////            //Assert.AreEqual(expect.Trim(), ret.Trim());
////            Assert.IsNull(ret);
////        }

////        [TestMethod]
////        public void GenTest3()
////        {
////            Generator g = new Generator();
////            var source = @"// -----------------------------------------------------------------------
////// <copyright file=""WireBaseViewModel.cs"" company=""Honeywell"">
////// TODO: Update copyright text.
////// </copyright>
////// -----------------------------------------------------------------------
////
////namespace Chart.VMWire.ViewModel
////{
////    using System;
////    using System.Collections.Generic;
////    using System.Linq;
////    using System.Text;
////    using System.Windows;
////
////    /// <summary>
////    /// TODO: Update summary.
////    /// </summary>
////    [AutoGen(""WireData"", typeof(string), ""null"")]
////    [AutoGen(""Visibility"", typeof(Visibility), ""Visibility.Hidden"")]
////    public partial class WireBaseViewModel : ViewModelBase
////    {
////    }
////}";
////            var ret = g.Gen(source);

////            var expect = @"namespace Chart.VMWire.ViewModel
////{
////    using System;
////    using System.Collections.Generic;
////    using System.Linq;
////    using System.Text;
////    using System.Windows;
////
////    public partial class WireBaseViewModel
////    {
////        /// <summary>
////        /// Field which backs the WireData property
////        /// </summary>
////        private string _wireData = ""null"";
////
////        /// <summary>
////        /// Gets / sets the WireData value
////        /// </summary>
////        public string WireData
////        {
////            get { return _wireData; }
////            set
////            {
////                if (_wireData == value) return;
////                _wireData = value;
////
////                OnWireDataChanged(value);
////                OnPropertyChanged(""WireData"");
////            }
////        }
////
////        /// <summary>
////        /// Invoked when the value of WireData changes
////        /// </summary>
////        partial void OnWireDataChanged(string value);
////        /// <summary>
////        /// Field which backs the Visibility property
////        /// </summary>
////        private Visibility _visibility = Visibility.Hidden;
////
////        /// <summary>
////        /// Gets / sets the Visibility value
////        /// </summary>
////        public Visibility Visibility
////        {
////            get { return _visibility; }
////            set
////            {
////                if (_visibility == value) return;
////                _visibility = value;
////
////                OnVisibilityChanged(value);
////                OnPropertyChanged(""Visibility"");
////            }
////        }
////
////        /// <summary>
////        /// Invoked when the value of Visibility changes
////        /// </summary>
////        partial void OnVisibilityChanged(Visibility value);
////    }
////}";

////            Console.WriteLine(ret);
////            ret = ret.Substring(ret.IndexOf("namespace"));
////            Assert.AreEqual(Standardize(expect), Standardize(ret));
////        }

////        [TestMethod]
////        public void GenTest4()
////        {
////            Generator g = new Generator();
////            var source = @"// -----------------------------------------------------------------------
////// <copyright file=""WireBaseViewModel.cs"" company=""Honeywell"">
////// TODO: Update copyright text.
////// </copyright>
////// -----------------------------------------------------------------------
////
////namespace Chart.VMWire.ViewModel
////{
////    using System;
////    using System.Collections.Generic;
////    using System.Linq;
////    using System.Text;
////    using System.Windows;
////
////    /// <summary>
////    /// TODO: Update summary.
////    /// </summary>
////    [AutoGenEntity(""WireData"", typeof(string), false)]
////    [AutoGenEntity(""Visibility"", typeof(Visibility), true, ""Visibility.Hidden"")]
////    [AutoGen(""WireInfo"", typeof(string), ""null"")]
////    public partial class WireBaseViewModel : ViewModelBase
////    {
////    }
////}";
////            var entitygen = new AutoGenEntity();
////            var autogen = new WpfInpcGenerator();
////            var ret = g.Gen(source, s => s == "" ? (IGenerator)autogen : entitygen);

////            var expect = @"// -----------------------------------------------------------------------
////// Auto generated by Icer WPF Smart Generator
////// Don't modify this file manually!
////// <auto-generated />
////// -----------------------------------------------------------------------
////
////namespace Chart.VMWire.ViewModel
////{
////    using System;
////    using System.Collections.Generic;
////    using System.Linq;
////    using System.Text;
////    using System.Windows;
////
////    public partial class WireBaseViewModel
////    {
////        public string WireData { get; set; }
////        public Visibility Visibility { get; private set; }
////
////        public WireBaseViewModel(string wireData, Visibility visibility = Visibility.Hidden)
////        {
////            this.WireData = wireData;
////            this.Visibility = visibility;
////        }
////        /// <summary>
////        /// Field which backs the WireInfo property
////        /// </summary>
////        private string _wireInfo = ""null"";
////
////        /// <summary>
////        /// Gets / sets the WireInfo value
////        /// </summary>
////        public string WireInfo
////        {
////            get { return _wireInfo; }
////            set
////            {
////                if (_wireInfo == value) return;
////                _wireInfo = value;
////
////                OnWireInfoChanged(value);
////                OnPropertyChanged(""WireInfo"");
////            }
////        }
////
////        /// <summary>
////        /// Invoked when the value of WireInfo changes
////        /// </summary>
////        partial void OnWireInfoChanged(string value);
////    }
////}
////";

////            //Console.WriteLine(ret);
////            //ret = ret.Substring(ret.IndexOf("namespace"));
////            for (int i = 0; i < expect.Length; i++)
////            {
////                if (ret[i] != expect[i]) Debugger.Break();
////            }
////            Assert.AreEqual(Standardize(expect), Standardize(ret));
////        }

////        private string Standardize(string input)
////        {
////            return input.Trim().Replace("\r", "");
////        }
////    }
////}