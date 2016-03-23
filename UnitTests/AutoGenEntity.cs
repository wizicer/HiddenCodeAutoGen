using Generator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

public class AutoGenEntity : IGenerator
{
    public string AttributeName { get { return "Entity"; } }
    public string Generate(string className, IList<IList<string>> ps)
    {
        var snippetProp = @"        public $type$ $property$ { get;$private$ set; }";
        var snippetInsideCtor = @"            this.$property$ = $field$;";
        var snippetCtor = @"$props$

        public $classname$($parameter$)
        {
$insidector$
        }
";
        var snippetParam = @"$type$ $field$$value$";

        var inputDicts = new List<Tuple<int, Tuple<string, Func<string, string>>>>();
        inputDicts.Add(Tuple.Create(0, Tuple.Create("property", (Func<string, string>)(s => s.Replace("\"", "").Trim()))));
        inputDicts.Add(Tuple.Create(0, Tuple.Create("field", (Func<string, string>)(s => s))));//.Substring(0, 1).ToLower() + s.Substring(1)))));
        inputDicts.Add(Tuple.Create(1, Tuple.Create("type", (Func<string, string>)(s => s.Replace("typeof(", "").Replace(")", "").Trim()))));
        inputDicts.Add(Tuple.Create(2, Tuple.Create("private", (Func<string, string>)(s => s.Trim() == "true" ? " private" : ""))));
        inputDicts.Add(Tuple.Create(3, Tuple.Create("value", (Func<string, string>)(s => s.Trim()))));

        var parList = ps
            .Select(lst => lst
                .SelectMany((s, i) =>
                    inputDicts.Where(t => t.Item1 == i).Select(tp => new KeyValuePair<string, string>(tp.Item2.Item1, tp.Item2.Item2(s))))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
            .ToArray();

        foreach (var item in parList)
        {
            item["field"] = item["property"].Substring(0, 1).ToLower() + item["property"].Substring(1);
            if (!item.ContainsKey("value")) item.Add("value", string.Empty);
            if (item["type"] != "string") item["value"] = item["value"].Trim('\"');
            if (item["value"] != string.Empty) item["value"] = string.Format(" = {0}", item["value"]);
        }

        var props = string.Join(Environment.NewLine, parList.Select(lst => Substitute(snippetProp, lst)));

        var insides = string.Join(Environment.NewLine, parList.Select(lst => Substitute(snippetInsideCtor, lst)));

        var pars = string.Join(", ", parList.OrderByDescending(d => string.IsNullOrEmpty(d["value"])).Select(lst => Substitute(snippetParam, lst)));

        var dicts = new Dictionary<string, string>();
        dicts.Add("props", props);
        dicts.Add("classname", className);
        dicts.Add("parameter", pars);
        dicts.Add("insidector", insides);

        var whole = Substitute(snippetCtor, dicts);

        return whole;
    }

    private string Substitute(string template, Dictionary<string, string> dicts)
    {
        var last = template;
        foreach (var pair in dicts)
        {
            last = last.Replace(string.Format("${0}$", pair.Key), pair.Value);
        }
        return last;
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AutoGenEntityAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoGenEntityAttribute"/> class.
        /// if the type is not string, the default value will remove the " on the begin and end
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field. only support typeof() method here</param>
        /// <param name="isPrivateSet">set to mark private before set</param>
        /// <param name="defaultValue">The default value.</param>
        public AutoGenEntityAttribute(string fieldName, Type fieldType, bool isPrivateSet, object defaultValue = null)
        {
        }
    }
}