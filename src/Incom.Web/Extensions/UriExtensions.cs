using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Linq;

namespace Incom.Web
{
    public static class UriExtensions
    {
        /// <summary>
        /// Fügt der aktuellen <see cref="Uri"/> ein neues Segment hinzu.
        /// </summary>
        /// <param name="uri">Die Uri an der das neue Segment angefügt werden soll.</param>
        /// <param name="segment">Das Segment welches angefügt werden soll.</param>
        /// <param name="encode">Angabe, ob das Segment Url encoded werden soll.</param>
        public static Uri AppendSegment(this Uri uri, string segment, bool encode = true)
        {
            // Vorhandenen Query String ermitteln.
            string query = uri.Query;

            // Neues Segment an Pfad anknüpfen.
            string pathA = uri.AbsoluteUriWithoutQuery().TrimEnd('/');
            string pathB = segment.TrimStart('/');

            var builder = new StringBuilder();
            builder.Append(pathA);
            builder.Append("/");
            builder.Append((encode) ? WebUtility.UrlEncode(pathB) : pathB);

            if (!string.IsNullOrWhiteSpace(query))
                builder.Append("?" + query);

            return new Uri(builder.ToString());
        }

        /// <summary>
        /// Fügt der aktuellen <see cref="Uri"/> ein Query String Parameter hinzu.
        /// </summary>
        /// <param name="uri">Die Uri an der der Query String angefügt werden soll.</param>
        /// <param name="name">Der Key/Name des Query Strings.</param>
        /// <param name="value">Der Wert des Query Strings.</param>
        /// <param name="encode">Angabe, ob der Wert des Query Strings Url encoded werden soll.</param>
        public static Uri SetQueryParam(this Uri uri, string name, string value, bool encode = true)
        {
            return uri.SetQueryParam(new Dictionary<string, string>()
            {
                { name, value }
            }, encode);
        }

        /// <summary>
        /// Fügt der aktuellen <see cref="Uri"/> ein oder mehrere Query String Parameter hinzu.
        /// </summary>
        /// <param name="uri">Die Uri an der der Query String angefügt werden soll.</param>
        /// <param name="queryString">Die Query Strings die angefügt werden sollen.</param>
        /// <param name="encode">Angabe, ob die Werte des Query Strings Url encoded werden soll.</param>
        public static Uri SetQueryParam(this Uri uri, Dictionary<string, string> queryString, bool encode = true)
        {
            // Schon vorhandenen Query String ermitteln.
            string[] queries = uri.QueryStringAsArray();

            // Übergebene Query String Parameter in ein Array konvertieren.
            string[] newQueries = queryString.Select(q => q.Key + "=" + ((encode) ? WebUtility.UrlEncode(q.Value) : q.Value)).ToArray();

            // Alten und neuen Query String verbinden.
            queries = queries.Concat(newQueries).ToArray();

            return new Uri(uri.AbsoluteUriWithoutQuery() + "?" + string.Join("&", queries));
            
        }

        /// <summary>
        /// Gibt einen Query String als Array zurück.
        /// </summary>
        /// <param name="uri">Die Uri mit dem Query String.</param>
        /// <returns></returns>
        public static string[] QueryStringAsArray(this Uri uri)
        {
            string query = uri.Query;

            if (!string.IsNullOrWhiteSpace(query))
                return query.Split('&');
            else
                return new string[] { };
        }

        /// <summary>
        /// Gibt einen Query string als <see cref="Dictionary{string, string}"/> zurück.
        /// </summary>
        /// <param name="uri">Die Uri mit dem Query String.</param>
        /// <returns></returns>
        public static Dictionary<string, string> QueryStringAsDictionary(this Uri uri)
        {
            string[] queries = string.Join("", uri.AbsoluteUri.Split('?').Skip(1)).Split('&');
            return queries.ToDictionary(q => q.Split('=').FirstOrDefault(), q => WebUtility.UrlDecode(q.Split('=').LastOrDefault()));
        }

        /// <summary>
        /// Gibt den absoluten URI Pfad ohne Query String zurück.
        /// </summary>
        /// <param name="uri">Die Instanz eines <see cref="Uri"/> Objekts.</param>
        /// <returns></returns>
        public static string AbsoluteUriWithoutQuery(this Uri uri)
        {
            string path = uri.AbsoluteUri;
            if (path.LastIndexOf('?') >= 0)
                return path.Remove(path.LastIndexOf('?'));
            else
                return path;
        }
    }
}
