using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IcerSystem.Helper.WPFHelper.Demo
{
    /// <summary>
    /// Interaction logic for DemoControl.xaml
    /// </summary>
    [SnippetDependencyProperty(property = "From", defaultValue = "new Point()",
                           type = "Point", containerType = "NodeConnection")]
    [SnippetDependencyProperty(property = "To", defaultValue = "new Point()",
                               type = "Point", containerType = "NodeConnection")]
    [SnippetDependencyProperty(property = "Via", defaultValue = "new Point()",
                               type = "Point", containerType = "NodeConnection")]
    [SnippetDependencyProperty(property = "Stroke",
                               type = "Brush", containerType = "NodeConnection")]
    [SnippetDependencyProperty(property = "StrokeThickness", defaultValue = "0.0",
                               type = "double", containerType = "NodeConnection")]
    [SnippetDependencyProperty(property = "IsHighlighted", defaultValue = "false",
                               type = "bool", containerType = "NodeConnection")]
    public partial class DemoControl : UserControl
    {
        public DemoControl()
        {
            InitializeComponent();
        }
    }
}
