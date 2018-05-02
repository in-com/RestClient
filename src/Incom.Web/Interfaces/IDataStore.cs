using Incom.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web
{
    /// <summary>
    /// Speichert und verwaltet die erhaltenen Token Informationen.
    /// </summary>
    public interface IDataStore
    {
        /// <summary>
        /// Speichert erhaltene Token Informationen anhand des übergebenen Keys.
        /// </summary>
        /// <param name="key">Der key.</param>
        /// <param name="value">Der Wert der gespeichert werden soll.</param>
        Task StoreAsync(string key, Token value);

        /// <summary>
        /// Gibt die gespeicherten Daten anhand des übergebenen keys zurück.
        /// </summary>
        /// <param name="key">Der Key mit Hilfe die Daten ermittelt werden sollen.</param>
        /// <returns>Die gespeicherten Token Informationen.</returns>
        Task<Token> GetAsync(string key);

        /// <summary>
        /// Löscht die gespeicherten Informationen anhand des übergebenen Keys.
        /// </summary>
        /// <param name="key">Der Key der gelöscht werden soll.</param>
        Task DeleteAsync(string key);

        /// <summary>
        /// Löscht alle Daten im DataStore.
        /// </summary>
        Task ClearAsync();
    }
}
