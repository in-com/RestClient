using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.RestClient
{
    public interface IRequestSyntax
    {
        /// <summary>
        /// Führt eine GET-Anfrage auf dem Server aus.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetAsync<T>();

        /// <summary>
        /// Führt eine GET-Anfrage auf dem Server aus.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<object> GetAsync(Type type);

        /// <summary>
        /// Führt eine POST-Anfrage auf dem Server aus.
        /// </summary>
        /// <returns></returns>
        Task PostAsync();

        /// <summary>
        /// Führt eine PUT-Anfrage auf dem Server aus.
        /// </summary>
        /// <returns></returns>
        Task PutAsync();

        /// <summary>
        /// Führt eine DELETE-Anfrage auf dem Server aus.
        /// </summary>
        /// <returns></returns>
        Task DeleteAsync();

        /// <summary>
        /// Fügt dem aktuellen Pfad ein neues Segment hinzu.
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        IRequestSyntax AppendPathSegment(object segment, bool encode = false);

        /// <summary>
        /// Fügt dem aktuellen Pfad ein Query String Parameter hinzu.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IRequestSyntax SetQueryParam(string name, object value);

        /// <summary>
        /// Fügt dem aktuellen Pfad ein oder mehrere Query String Parameter hinzu.
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        IRequestSyntax SetQueryParams(object queryParams);

        /// <summary>
        /// Fügt der Request Body-Daten hinzu.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        IRequestSyntax SetBody(object body, string contentType = "application/json");
    }
}
