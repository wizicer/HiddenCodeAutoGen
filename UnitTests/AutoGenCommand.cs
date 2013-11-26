using System;
using System.Collections.Generic;
using System.Linq;
using IcerWPFSmartGen;

public class CommandAutoGen : IGenerator
{
    public string Generate(string className, IList<IList<string>> ps)
    {
        return ps.Aggregate("", (str, m) => str + GenAutoCommand(m));
    }

    private string GenAutoCommand(IList<string> ps)
    {
        var snippet = @"        /// <summary>
        /// The $property$ command
        /// </summary>
        private RelayCommand $field$Command;

        /// <summary>
        /// Gets the $property$ command.
        /// </summary>
        /// <value>The $property$ command.</value>
        public ICommand $property$Command
        {
            get { return this.$field$Command ?? (this.$field$Command = new RelayCommand($command$)); }
        }$execute$
";

        var snippetCommandExecute = @"

        /// <summary>
        /// delegate for invoking $property$Command
        /// </summary>
        private void Execute$property$Wrap(object obj)
        {
            Execute$property$(obj);
        }

        /// <summary>
        /// Invoked when $property$Command invokes
        /// </summary>
        partial void Execute$property$(object obj);
";

        var prop = ps[0].Replace("\"", "").Trim();
        string execute = null;
        string canexecute = null;
        if (ps.Count > 1 && ps[1].Trim() != "null") execute = ps[1].Replace("\"", "").Trim();
        if (ps.Count > 2 && ps[2].Trim() != "null") canexecute = ps[2].Replace("\"", "").Trim();

        var dict = new Dictionary<string, string>();
        dict.Add("property", prop);
        dict.Add("field", "_" + prop.Substring(0, 1).ToLower() + prop.Substring(1));

        dict.Add("execute", execute == null ? Format(snippetCommandExecute, dict) : Environment.NewLine);
        var command = string.Join(
            ", ",
            new[] {
                execute == null ? Format("this.Execute$property$Wrap", dict) : execute,
                canexecute,
            }.Where(s => s != null).ToArray());
        dict.Add("command", command);

        return Format(snippet, dict);
    }

    private string Format(string snippet, Dictionary<string, string> dict)
    {
        var trans = snippet
            .Replace("\r", "")
            .Replace("\n", "\r\n")
            .Replace("{", "{{")
            .Replace("}", "}}");
        for (int i = 0; i < dict.Count; i++)
        {
            trans = trans.Replace(string.Format("${0}$", dict.ElementAt(i).Key), string.Format("{{{0}}}", i));
        }

        return string.Format(trans, dict.Select(kvp => kvp.Value).ToArray());
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AutoGenCommandAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoGenCommandAttribute"/> class.
        /// if the type is not string, the default value will remove the " on the begin and end
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field. only support typeof() method here</param>
        /// <param name="defaultValue">The default value.</param>
        public AutoGenCommandAttribute(string commandName, string execute = null, string canExecute = null)
        {
        }
    }
}