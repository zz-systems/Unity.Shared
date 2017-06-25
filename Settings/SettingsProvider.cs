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

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using ZzSystems.Unity.Shared.Security;

namespace ZzSystems.Unity.Shared.Settings
{
    /// <summary>
    /// Provides a secured high-level access to Unity.PlayerPrefs.
    /// Accepts any type serializable by Newtonsoft.Json
    /// </summary>
    public class SettingsProvider : ISettingsProvider
    {
        /// <summary>
        /// We need all possible type information for Json serialization
        /// </summary>
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        /// <summary>
        /// Settings cache
        /// </summary>
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        /// <summary>
        /// CryptoProvider instance
        /// </summary>
        private readonly ICryptoProvider _cryptoProvider;

        /// <summary>
        /// Creates an instance of SettingsProvider with a ICryptoProvider instance 
        /// </summary>
        /// <param name="cryptoProvider">CryptoProvider used for encryption and decryption</param>
        public SettingsProvider(ICryptoProvider cryptoProvider)
        {
            _cryptoProvider = cryptoProvider;

            var key = _cryptoProvider.Hash("_data");

            if (PlayerPrefs.HasKey(key))
            {                
                try
                {
                    var decrypted = _cryptoProvider.Decrypt(PlayerPrefs.GetString(key));
                    _data = JsonConvert.DeserializeObject<Dictionary<string, object>>(decrypted, _serializerSettings);
                }
                catch(Exception ex)
                {
                    Debug.LogErrorFormat("Corrupt settings. Deleting. Reason: {0}", ex);

                    PlayerPrefs.DeleteKey(key);
                    _data = new Dictionary<string, object>();
                }
            }   
        }

        /// <summary>
        /// Gets a value from the setting cache
        /// </summary>
        /// <typeparam name="T">Desired type</typeparam>
        /// <param name="key">Setting key</param>
        /// <param name="defaultValue">Default value in case of failed lookup / conversion</param>
        /// <returns>Found value or default value</returns>
        public T Get<T>(string key, T defaultValue = default(T))
        {
            try
            {
                object value;
                if (_data.TryGetValue(_cryptoProvider.Hash(key), out value))
                    return (T)Convert.ChangeType(value, typeof(T));
            }
            catch(Exception ex)
            {
                Debug.LogErrorFormat("Could not fetch value for key [{0}]. Reason: {1}", key, ex.Message);
            }

            return defaultValue;
        }

        /// <summary>
        /// Sets a value in the settings cache
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="key">Setting key</param>
        /// <param name="value">Value to set</param>
        /// <returns>self, fluent interface</returns>
        public ISettingsProvider Set<T>(string key, T value)
        {
            _data[_cryptoProvider.Hash(key)] = value;

            return this;
        }

        /// <summary>
        /// Checks if a particular key exists
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <returns>true if key exists</returns>
        public bool HasKey(string key)
        {
            return _data.ContainsKey(_cryptoProvider.Hash(key));
        }

        /// <summary>
        /// Deletes a particular key
        /// </summary>
        /// <param name="key">Key to delete</param>
        /// <returns>self, fluent interface</returns>
        public ISettingsProvider DeleteKey(string key)
        {
            var hash = _cryptoProvider.Hash(key);

            if (_data.ContainsKey(hash))
                _data.Remove(hash);

            return this;
        }

        /// <summary>
        /// Deletes all keys
        /// </summary>
        /// <returns>self, fluent interface</returns>
        public ISettingsProvider DeleteAll()
        {
            _data.Clear();

            return this;
        }

        /// <summary>
        /// Saves changes
        /// </summary>
        /// <returns>self, fluent interface</returns>
        public ISettingsProvider Save()
        {
            var serialized = JsonConvert.SerializeObject(_data, _serializerSettings);

            PlayerPrefs.SetString(_cryptoProvider.Hash("_data"), _cryptoProvider.Encrypt(serialized));
            PlayerPrefs.Save();

            return this;
        }
    }
}