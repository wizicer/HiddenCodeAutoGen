Feature: Generate
    As Generator user
    I want to get generated content
    So that I can improve my development speed

Scenario: No partial class
    Given I have source code:
    """
    // -----------------------------------------------------------------------
    // <copyright file="WireEditingViewModel.cs" company="Honeywell">
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
    }
    """
    And I design to use default generator
    When I ask to generate
    Then the result should be:
    """
    """

Scenario: multiple class
    Given I have source code:
    """
    // -----------------------------------------------------------------------
    // <copyright file="WireBaseViewModel.cs" company="Honeywell">
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
                : this(fieldName, "_" + fieldName.Substring(0, 1).ToLower() + fieldName.Substring(1), fieldType, defaultValue)
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
        [AutoGen("WireData", typeof(SquarePolyline), null)]
        [AutoGen("Visibility", typeof(Visibility), Visibility.Hidden)]
        public partial class WireBaseViewModel : ViewModelBase
        {
        }
    }
    """
    And I design to use default generator
    When I ask to generate
    Then the result should be:
    """
    """

Scenario: standard autogen (only one type of AutoGen)
    Given I have source code:
    """
    // -----------------------------------------------------------------------
    // <copyright file="WireBaseViewModel.cs" company="Honeywell">
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
        [AutoGen("WireData", typeof(string), "null")]
        [AutoGen("Visibility", typeof(Visibility), "Visibility.Hidden")]
        public partial class WireBaseViewModel : ViewModelBase
        {
        }
    }
    """
    And I design to use default generator
    When I ask to generate
    And Remove comments before namespace
    Then the result should be:
    """
    namespace Chart.VMWire.ViewModel
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
            private string _wireData = "null";

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
                    OnPropertyChanged("WireData");
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
                    OnPropertyChanged("Visibility");
                }
            }

            /// <summary>
            /// Invoked when the value of Visibility changes
            /// </summary>
            partial void OnVisibilityChanged(Visibility value);
        }
    }
    """

Scenario: mixed type of autogen
    Given I have source code:
    """
    // -----------------------------------------------------------------------
    // <copyright file="WireBaseViewModel.cs" company="Honeywell">
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
        [AutoGenEntity("WireData", typeof(string), false)]
        [AutoGenEntity("Visibility", typeof(Visibility), true, "Visibility.Hidden")]
        [AutoGen("WireInfo", typeof(string), "null")]
        public partial class WireBaseViewModel : ViewModelBase
        {
        }
    }
    """
    And I design to use embedded generator
    When I ask to generate
    Then the result should be:
    """
    // -----------------------------------------------------------------------
    // Auto generated by Icer WPF Smart Generator
    // Don't modify this file manually!
    // <auto-generated />
    // -----------------------------------------------------------------------

    namespace Chart.VMWire.ViewModel
    {
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Text;
        using System.Windows;

        public partial class WireBaseViewModel
        {
            public string WireData { get; set; }
            public Visibility Visibility { get; private set; }

            public WireBaseViewModel(string wireData, Visibility visibility = Visibility.Hidden)
            {
                this.WireData = wireData;
                this.Visibility = visibility;
            }
            /// <summary>
            /// Field which backs the WireInfo property
            /// </summary>
            private string _wireInfo = "null";

            /// <summary>
            /// Gets / sets the WireInfo value
            /// </summary>
            public string WireInfo
            {
                get { return _wireInfo; }
                set
                {
                    if (_wireInfo == value) return;
                    _wireInfo = value;

                    OnWireInfoChanged(value);
                    OnPropertyChanged("WireInfo");
                }
            }

            /// <summary>
            /// Invoked when the value of WireInfo changes
            /// </summary>
            partial void OnWireInfoChanged(string value);
        }
    }
    """

Scenario: autogen dp
    Given I have source code:
    """
    // -----------------------------------------------------------------------
    // <copyright file="WireBaseViewModel.cs" company="Honeywell">
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
        [AutoGenDP("WireInfo", typeof(string), "null")]
        [AutoGenDP("WireInfo2", typeof(Wire), "new Wire()")]
        public partial class WireBaseViewModel : ViewModelBase
        {
        }
    }
    """
    And I design to use embedded generator
    When I ask to generate
    Then the result should be:
    """
    // -----------------------------------------------------------------------
    // Auto generated by Icer WPF Smart Generator
    // Don't modify this file manually!
    // <auto-generated />
    // -----------------------------------------------------------------------

    namespace Chart.VMWire.ViewModel
    {
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Text;
        using System.Windows;

        public partial class WireBaseViewModel
        {
            /// <summary>
            /// Gets / sets the WireInfo property value, This is a dependency property
            /// </summary>
            public string WireInfo
            {
                get { return (string)GetValue(WireInfoProperty); }
                set { SetValue(WireInfoProperty, value); }
            }

            /// <summary>
            /// Defines the WireInfo dependnecy property.
            /// </summary>
            public static readonly DependencyProperty WireInfoProperty =
                DependencyProperty.Register("WireInfo", typeof(string), typeof(WireBaseViewModel),
                    new PropertyMetadata("null", new PropertyChangedCallback(OnWireInfoPropertyChanged)));

            /// <summary>
            /// Invoked when the WireInfo property changes
            /// </summary>
            partial void OnWireInfoPropertyChanged(DependencyPropertyChangedEventArgs e);

            private static void OnWireInfoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                WireBaseViewModel control = d as WireBaseViewModel;
                control.OnWireInfoPropertyChanged(e);
            }
            /// <summary>
            /// Gets / sets the WireInfo2 property value, This is a dependency property
            /// </summary>
            public Wire WireInfo2
            {
                get { return (Wire)GetValue(WireInfo2Property); }
                set { SetValue(WireInfo2Property, value); }
            }

            /// <summary>
            /// Defines the WireInfo2 dependnecy property.
            /// </summary>
            public static readonly DependencyProperty WireInfo2Property =
                DependencyProperty.Register("WireInfo2", typeof(Wire), typeof(WireBaseViewModel),
                    new PropertyMetadata(new Wire(), new PropertyChangedCallback(OnWireInfo2PropertyChanged)));

            /// <summary>
            /// Invoked when the WireInfo2 property changes
            /// </summary>
            partial void OnWireInfo2PropertyChanged(DependencyPropertyChangedEventArgs e);

            private static void OnWireInfo2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                WireBaseViewModel control = d as WireBaseViewModel;
                control.OnWireInfo2PropertyChanged(e);
            }
        }
    }
    """
