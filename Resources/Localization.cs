#region copyright
/***************************************************************************
 * The Void
 * Copyright (C) 2015-2017  Sergej Zuyev
 * sergej.zuyev - at - zz-systems.net
 
 * Permission is hereby granted, free of charge, to any person obtaining 
 * a copy of this software and associated documentation files 
 * (the "Software"), to deal in the Software without restriction, including 
 * without limitation the rights to use, copy, modify, merge, publish, 
 * distribute, sublicense, and/or sell copies of the Software, and to 
 * permit persons to whom the Software is furnished to do so, subject to 
 * the following conditions:

 * The above copyright notice and this permission notice shall be included 
 * in all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *
 **************************************************************************/
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZzSystems.Unity.Shared.Resources
{
    /// <summary>
    /// Loads translations from resource files
    /// </summary>
    [ExecuteInEditMode]
    public class Localization : MonoBehaviour {

        private Dictionary<string, object> _strings;
        private Dictionary<string, object> _cache;
        private Dictionary<string, string> _strCache;

        /// <summary>
        /// Load available locales
        /// </summary>
        void Awake ()
        {
            TextAsset text;

            // TODO: provider
            switch (Application.systemLanguage)
            {
                case SystemLanguage.German:
                    text = LoadLanguage("de-DE");
                    break;
                case SystemLanguage.Russian:
                    text = LoadLanguage("ru-RU");
                    break;
                default:
                    text = LoadLanguage("en-EN");
                    break;
            }

            var obj = MiniJSON.Json.Deserialize(text.text);
            _strings    = (Dictionary<string, object>)obj;
            _cache      = new Dictionary<string, object>();
            _strCache   = new Dictionary<string, string>();
        }

        TextAsset LoadLanguage(string lang)
        {
            return UnityEngine.Resources.Load<TextAsset>(string.Format("i18n/{0}", lang));
        }

        /// <summary>
        /// Retrieves a value from currently loaded locale cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Value from the cache</returns>
        public object this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                    return null;

                object value;
                if (_cache.TryGetValue(key, out value))
                    return value;
                
                
                var current = _strings;
                var segments = key.Split('.');

                if (segments.Any())
                {
                    current = segments
                        .Take(segments.Length - 1)
                        .Aggregate(current, (current1, segment)
                            => (Dictionary<string, object>)current1[segment]);
                }

                value = current[segments.Last()];
                _cache.Add(key, value);

                return value;
            }
        }

        /// <summary>
        /// Get strongly typed value from currently loaded locale cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Access key</param>
        /// <returns>Strongly typed value</returns>
        public T Get<T>(string key)
        {
            return (T) this[key];
        }

        /// <summary>
        ///  Get textual value from currently loaded locale cache
        /// </summary>
        /// <param name="key">Access key</param>
        /// <returns>A string if the value exists, otherwise an empty string</returns>
        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            string value;
            if (_strCache.TryGetValue(key, out value))
                return value;

            value = Get<string>(key);

            if (!string.IsNullOrEmpty(value))
                value = value.ToLower();

            _strCache.Add(key, value);

            return value;
        }
    }
}
