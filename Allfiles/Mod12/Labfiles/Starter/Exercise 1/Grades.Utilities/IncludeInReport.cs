using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Grades.Utilities
{
    /// <summary>
    /// Custom attribute that specifies whether a field or property should be included in a report
    /// </summary>
    // Exercise 1: Task 1b: Specify the possible targets to which the IncludeInReport attribute can be applied

    // Exercise 1: Task 1a: Specify that IncludeInReportAttribute is an attribute class
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class IncludeInReportAttribute : Attribute
    {
        // Exercise 1: Task 1c: Define a private field to hold the value of the attribute
        private bool _include;

        // Exercise 1: Task 1d: Add public properties that specify how an included item should be formatted
        public bool Underline { get; set; }
        public bool Bold { get; set; }
        // Exercise 1: Task 1e: Add a public property that specifies a label (if any) for the item
        public string Label { get; set; }
        // Exercise 1: Task 1f: Define constructors
        public IncludeInReportAttribute()
        {
            this._include = true;
            this.Underline = false;
            this.Bold = false;
            this.Label = string.Empty;
        }

        public IncludeInReportAttribute(bool includeInReport)
        {
            this._include = includeInReport;
            this.Underline = false;
            this.Bold = false;
            this.Label = string.Empty;
        }
    }
}
