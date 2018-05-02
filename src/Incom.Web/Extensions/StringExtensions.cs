using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Incom.Web
{
    public static class StringExtensions
    {
        /// <summary>
        /// Prüft ob der übergebene Werte ein JSON-Objekt oder -Array ist.
        /// </summary>
        /// <param name="str">Der Wert der geprüft werden soll.</param>
        /// <param name="exception">Die Exception die beim Fehlerhaften Parsen ausgeworfen wird. Ist null wenn das parsen geklappt hat.</param>
        /// <returns></returns>
        public static bool IsValidJson(this string str, out Exception exception)
        {
            exception = null;
            str = str.Trim();

            if ((str.StartsWith("{") && str.EndsWith("}")) || //For object
                (str.StartsWith("[") && str.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(str);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    exception = jex;
                    return false;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
