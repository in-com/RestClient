﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Incom.Web.Models;

namespace Incom.Web
{
    public static class HttpResponseMessageExtensions
    {
        #region Methods (Public)

        /// <summary>
        /// Gibt den Inhalt der <see cref="HttpResponseMessage"/> konvertiert.
        /// </summary>
        /// <typeparam name="T">Der erwartete <see cref="Type"/> des Inhalts.</typeparam>
        /// <param name="response">Das <see cref="HttpResponseMessage"/> Objekt dessen Inhalt extrahiert werden soll.</param>
        /// <exception cref="RestApiException">Wenn der Inhalt der <see cref="HttpResponseMessage"/> eine Fehlermeldung ist.</exception>
        /// <returns></returns>
        public static async Task<T> GetContentAsync<T>(this HttpResponseMessage response)
        {
            return (T)await response.GetContentAsync(typeof(T));
        }

        /// <summary>
        /// Gibt den Inhalt der <see cref="HttpResponseMessage"/> konvertiert.
        /// </summary>
        /// <param name="response">Das <see cref="HttpResponseMessage"/> Objekt dessen Inhalt extrahiert werden soll.</param>
        /// <param name="type">Der erwartete <see cref="Type"/> des Inhalts.</param>
        /// <exception cref="RestApiException">Wenn der Inhalt der <see cref="HttpResponseMessage"/> eine Fehlermeldung ist.</exception>
        /// <returns></returns>
        public static async Task<object> GetContentAsync(this HttpResponseMessage response, Type type = null)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.InternalServerError:
                    {
                        var error = await GetError<InternalServerError>(response);
                        throw new RestApiException(error?.Message, response.StatusCode);
                    }
                case HttpStatusCode.NotFound:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Conflict:
                case HttpStatusCode.BadRequest:
                    {
                        var error = await GetError<RestApiError>(response);
                        throw new RestApiException(error?.Message, response.StatusCode);
                    }
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                    {
                        if (type != null)
                        {
                            bool resultIsByteArray = false;
                            object result = null;
                            if (type.IsArray && type.GetElementType() == typeof(byte))
                            {
                                resultIsByteArray = true;
                                result = await response.Content.ReadAsByteArrayAsync();
                            }
                            else
                            {
                                result = await response.Content.ReadAsStringAsync();
                            }

                            if (resultIsByteArray)
                                return result;
                            else
                                return JsonConvert.DeserializeObject(result.ToString(), type);
                        }
                        else
                        {
                            return null;
                        }
                    }
            }

            return null;
        }

        /// <summary>
        /// Gibt alle <see cref="Cookie"/>s die vom Server gesendet wurden.
        /// </summary>
        /// <param name="response">Das <see cref="HttpResponseMessage"/> Objekt dessen Cookies extrahiert werden soll.</param>
        /// <param name="cookies">Ein <see cref="CookieContainer"/> der für das extrahieren der Cookies verwendet wird.</param>
        /// <exception cref="RestApiException">Wenn der Inhalt der <see cref="HttpResponseMessage"/> eine Fehlermeldung ist.</exception>
        /// <returns></returns>
        public static async Task<IEnumerable<Cookie>> GetCookiesAsync(this HttpResponseMessage response, CookieContainer cookies)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.InternalServerError:
                    {
                        var error = await GetError<InternalServerError>(response);
                        throw new RestApiException(error?.Message, response.StatusCode);
                    }
                case HttpStatusCode.NotFound:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Conflict:
                case HttpStatusCode.BadRequest:
                    {
                        var error = await GetError<RestApiError>(response);
                        throw new RestApiException(error?.Message, response.StatusCode);
                    }
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                    {
                        Uri uri = new Uri(response.RequestMessage.RequestUri.AbsoluteUri.Replace(response.RequestMessage.RequestUri.PathAndQuery, ""));
                        return cookies.GetCookies(uri).Cast<Cookie>();
                    }
            }

            return null;
        }

        #endregion

        #region Methods (Private)

        /// <summary>
        /// Versucht eine Fehlermeldung aus der <see cref="HttpResponseMessage"/> zu extrahieren.
        /// </summary>
        /// <typeparam name="T">Der Typ der Fehlermeldung</typeparam>
        /// <param name="response">Das <see cref="HttpResponseMessage"/> Objekt aus dem der Fehler extrahiert werden soll.</param>
        /// <returns></returns>
        private static async Task<T> GetError<T>(HttpResponseMessage response)
        {
            string result = await response.Content.ReadAsStringAsync();
            if (result.IsValidJson(out Exception jsonEx))
            {
                return JsonConvert.DeserializeObject<T>(result.ToString());
            }
            else
            {
                return default(T);
            }
        }

        #endregion
    }
}
