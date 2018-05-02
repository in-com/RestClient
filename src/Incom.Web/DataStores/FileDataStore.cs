using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Incom.Web.Models;

namespace Incom.Web
{
    /// <summary>
    /// File data store welches das Interface <see cref="IDataStore"/> implementiert. Dieser DataStore erstellt eine JSON-Datei
    /// mit den Token Informationen auf der Festplatte.
    /// </summary>
    public class FileDataStore : IDataStore
    {
        #region Fields

        /// <summary>
        /// Gibt den vollen Pfad zum Ablageort der Dateien.
        /// </summary>
        private string storagePath;

        #endregion

        #region Ctor

        /// <summary>
        /// Erstellt eine neue Instanz des file data stores. Wenn <c>path</c> leer ist, wird <see cref="Environment.SpecialFolder.ApplicationData"/> als Ablageort der Dateien
        /// verwendet
        /// Das Verzeichnis wird erstellt falls es dieses noch nicht gibt.
        /// </summary>
        /// <param name="path"></param>
        public FileDataStore(string path = "")
        {
            if (!string.IsNullOrEmpty(path))
            {
                storagePath = path;
            }
            else
            {
#if NETSTANDARD1_3
                storagePath = Path.Combine(Environment.GetEnvironmentVariable(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "LocalAppData" : "Home"), @"in-com\api.token.response");
#else
                storagePath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), @"in-com\api.token.response");
#endif
                if (!Directory.Exists(storagePath))
                    Directory.CreateDirectory(storagePath);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Speichert die Token Informationen entweder im AppData-Verzeichnis oder in das Verzeichnis welches
        /// übergeben wurde.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task StoreAsync(string key, Token value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("key");

            var filePath = Path.Combine(storagePath, key);
            File.WriteAllText(filePath, JsonConvert.SerializeObject(value, Formatting.Indented));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Ermittelt die Token Informationen anhand des Keys.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Token> GetAsync(string key)
        {
            var filePath = Path.Combine(storagePath, key);
            string content = "";

            if (File.Exists(filePath))
                content = File.ReadAllText(filePath);

            if (!string.IsNullOrEmpty(content))
                return await Task.Run(() => JsonConvert.DeserializeObject<Token>(content));
            else
                return await Task.FromResult<Token>(null);
        }

        /// <summary>
        /// Löscht eine Datei mit Hilfe des Keys.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task DeleteAsync(string key)
        {
            var filePath = Path.Combine(storagePath, key);
            if (File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Löscht alle Dateien mit den Token Informationen.
        /// </summary>
        /// <returns></returns>
        public Task ClearAsync()
        {
            if (Directory.Exists(storagePath))
            {
                Directory.Delete(storagePath, true);
                Directory.CreateDirectory(storagePath);
            }

            return Task.CompletedTask;
        }

        #endregion
    }
}
