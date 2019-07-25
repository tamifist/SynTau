using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Common.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IndexAttribute : Attribute
    {
        public bool IsUnique { get; set; }
        public bool IsClustered { get; set; }
    }
}
