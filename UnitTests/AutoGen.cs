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
}