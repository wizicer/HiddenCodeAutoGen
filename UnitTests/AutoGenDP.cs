using Generator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

public class DPGenerator : IGenerator
{
    public string Generate(string className, IList<IList<string>> ps)
    {
        return ps.Aggregate("", (str, m) => str + GenAutoDP(className, m));
    }

    private string GenAutoDP(string className, IList<string> ps)
    {
        var snippet = @"        /// <summary>
        /// Gets / sets the $property$ property value, This is a dependency property
        /// </summary>
        public $type$ $property$
        {
            get { return ($type$)GetValue($property$Property); }
            set { SetValue($property$Property, value); }
        }

        /// <summary>
        /// Defines the $property$ dependnecy property.
        /// </summary>
        public static readonly System.Windows.DependencyProperty $property$Property =
            System.Windows.DependencyProperty.Register(""$property$"", typeof($type$), typeof($containerType$),
                new System.Windows.PropertyMetadata($defaultValue$, new System.Windows.PropertyChangedCallback(On$property$PropertyChanged)));

        /// <summary>
        /// Invoked when the $property$ property changes
        /// </summary>
        partial void On$property$PropertyChanged(System.Windows.DependencyPropertyChangedEventArgs e);

        private static void On$property$PropertyChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            $containerType$ control = d as $containerType$;
            control.On$property$PropertyChanged(e);
        }
"
            .Replace("\r", "")
            .Replace("\n", "\r\n")
            .Replace("{", "{{")
            .Replace("}", "}}")
            .Replace("$property$", "{0}")
            .Replace("$type$", "{1}")
            .Replace("$defaultValue$", "{2}")
            .Replace("$containerType$", "{3}");

        var prop = ps[0].Replace("\"", "").Trim();
        var defaultValue = ps[2].Trim();
        var type = ps[1].Replace("typeof(", "").Replace(")", "").Trim();
        if (type.ToLower() != "string") defaultValue = defaultValue.Replace("\"", "");
        return string.Format(snippet,
            prop,
            type,
            defaultValue,
            className);
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AutoGenDPAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoGenEntityAttribute"/> class.
        /// if the type is not string, the default value will remove the " on the begin and end
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field. only support typeof() method here</param>
        /// <param name="defaultValue">The default value.</param>
        public AutoGenDPAttribute(string fieldName, Type fieldType, object defaultValue = null)
        {
        }
    }
}