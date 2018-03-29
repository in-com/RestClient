using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Incom.Web.RestClient
{
    public static class HttpResponseMessageExtensions
    {
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
                        string result = await response.Content.ReadAsStringAsync();
                        InternalServerError error = null;

                        if (result.IsValidJson(out Exception jsonEx))
                        {
                            error = JsonConvert.DeserializeObject<InternalServerError>(result.ToString());
                        }

                        if (error == null)
                        {
                            error = new InternalServerError()
                            {
                                Message = result
                            };
                        }

                        throw new RestApiException(error.Message, response.StatusCode);
                    }
                case HttpStatusCode.NotFound:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Conflict:
                case HttpStatusCode.BadRequest:
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        RestApiError error = null;

                        if (result.IsValidJson(out Exception jsonEx))
                        {
                            error = JsonConvert.DeserializeObject<RestApiError>(result.ToString());
                        }

                        if (error == null)
                        {
                            error = new RestApiError()
                            {
                                Code = (int)response.StatusCode,
                                Message = result
                            };
                        }

                        throw new RestApiException(error.Message, (HttpStatusCode)error.Code);
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
    }
}
