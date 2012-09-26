// -----------------------------------------------------------------------
// <copyright file="Class1.cs" company="Honeywell">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace IcerSystem.Helper.WPFHelper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AutoGenAttribute : Attribute
    {
        public AutoGenAttribute(object p1, object p2, object p3, object p4)
        {
        }
    }
}
