using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web
{
    /// <summary>
    /// Wird ausgelöst sobald ein Fehler in einem RestClient auftritt.
    /// </summary>
    public class RestApiException : Exception
    {
        #region Properties

        public HttpStatusCode StatusCode { get; set; }

        #endregion

        #region Ctor

        public RestApiException()
        {
        }

        public RestApiException(string message)
            : base(message)
        {
        }

        public RestApiException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public RestApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
