using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteConversionEngine.ConversionData
{
    /// <summary>
    /// Describes a single view schema
    /// </summary>
    public class ViewSchema
    {
        /// <summary>
        /// Contains the view name
        /// </summary>
        public string ViewName;

        /// <summary>
        /// Contains the view SQL statement
        /// </summary>
        public string ViewSQL;
    }
}
