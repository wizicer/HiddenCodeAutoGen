using System;
using System.Collections.Generic;
using System.Linq;
using IcerWPFSmartGen;

public class WpfInpcGenerator : IGenerator
{
    public string Generate(string className, IList<IList<string>> ps)
    {
        return ps.Aggregate("", (str, m) => str + GenAutoINPC(m));
    }

    private string GenAutoINPC(IList<string> ps)
    {
        var snippet = @"        /// <summary>
        /// Field which backs the $property$ property
        /// </summary>
        private $type$ $field$ = $defaultValue$;

        /// <summary>
        /// Gets / sets the $property$ value
        /// </summary>
        public $type$ $property$
        {
            get { return $field$; }
            set
            {
                if ($field$ == value) return;
                $field$ = value;

                On$property$Changed(value);
                OnPropertyChanged(""$property$"");
            }
        }
    
        /// <summary>
        /// Invoked when the value of $property$ changes
        /// </summary>
        partial void On$property$Changed($type$ value);
"
            .Replace("\r", "")
            .Replace("\n", "\r\n")
            .Replace("{", "{{")
            .Replace("}", "}}")
            .Replace("$property$", "{0}")
            .Replace("$type$", "{1}")
            .Replace("$field$", "{2}")
            .Replace("$defaultValue$", "{3}");

        var pr = ps[0].Replace("\"", "").Trim();
        var dv = ps[2].Trim();
        var tp = ps[1].Replace("typeof(", "").Replace(")", "").Trim();
        if (tp.ToLower() != "string") dv = dv.Replace("\"", "");
        return string.Format(snippet,
            pr,
            tp,
            "_" + pr.Substring(0, 1).ToLower() + pr.Substring(1),
            dv);
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AutoGenAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoGenAttribute"/> class.
        /// if the type is not string, the default value will remove the " on the begin and end
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field. only support typeof() method here</param>
        /// <param name="defaultValue">The default value.</param>
        public AutoGenAttribute(string fieldName, Type fieldType, object defaultValue)
        {
        }
    }
}