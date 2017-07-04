using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Incom.Web.RestClient
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns a string that represents the current object, using <see cref="CultureInfo.InvariantCulture"/> where possible.
        /// </summary>
        [DebuggerStepThrough]
        public static string ToInvariantString(this object obj)
        {
            // inspired by: http://stackoverflow.com/a/19570016/62600

            if (obj is IConvertible c)
                return c.ToString(CultureInfo.InvariantCulture);

            if (obj is IFormattable f)
                return f.ToString(null, CultureInfo.InvariantCulture);

            return obj.ToString();
        }
    }
}
