using System;
using System.Collections.Generic;
using System.Linq;
using IcerWPFSmartGen;

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
        public static readonly DependencyProperty $property$Property =
            DependencyProperty.Register(""$property$"", typeof($type$), typeof($containerType$),
                new PropertyMetadata($defaultValue$, new PropertyChangedCallback(On$property$PropertyChanged)));

        /// <summary>
        /// Invoked when the $property$ property changes
        /// </summary>
        partial void On$property$PropertyChanged(DependencyPropertyChangedEventArgs e);

        private static void On$property$PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            $containerType$ control = d as $containerType$;
            control.On$property$PropertyChanged(e);
        }
"
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
}