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

using UnityEngine;

namespace ZzSystems.Unity.Shared.Settings
{
    /// <summary>
    /// Provides transparent settings migration between different SettingsProvider versions. 
    /// </summary>
    public class MigratingSettingsProvider : ISettingsProvider
    {
        /// <summary>
        /// SettingsProvider to migrate from
        /// </summary>
        public ISettingsProvider Previous { get; set; }

        /// <summary>
        /// SettingsProvider to migrate to
        /// </summary>
        public ISettingsProvider Current { get; set; }

        /// <summary>
        /// Creates an instance of MigratingSettingsProvider with a legacy SettingsProvider and a current SettingsProvider instance  
        /// </summary>
        /// <param name="previous">Legacy SettingsProvider</param>
        /// <param name="current">Current SettingsProvider</param>
        public MigratingSettingsProvider(ISettingsProvider previous, ISettingsProvider current)
        {
            Debug.LogFormat("Creating migrating settings provider. {0} -> {1}", previous.GetType().Name, current.GetType().Name);

            Previous = previous;
            Current = current;
        }

        /// <summary>
        /// Gets a value from the setting cache on both providers
        /// </summary>
        /// <typeparam name="T">Desired type</typeparam>
        /// <param name="key">Setting key</param>
        /// <param name="defaultValue">Default value in case of failed lookup / conversion</param>
        /// <returns>Found value or default value</returns>
        public T Get<T>(string key, T defaultValue = default(T))
        {
            var result = Previous.Get(key, defaultValue);

            Current.Set(key, result);

            return result;
        }

        /// <summary>
        /// Sets a value in the settings cache on both providers
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="key">Setting key</param>
        /// <param name="value">Value to set</param>
        /// <returns>self, fluent interface</returns>
        public ISettingsProvider Set<T>(string key, T value)
        {
            Previous.Set(key, value);
            Current.Set(key, value);

            return this;
        }

        /// <summary>
        /// Checks if a particular key exists in "previous" SettingsProvider
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <returns>true if key exists</returns>
        public bool HasKey(string key)
        {
            return Previous.HasKey(key);
        }

        /// <summary>
        /// Deletes a particular key on both providers
        /// </summary>
        /// <param name="key">Key to delete</param>
        /// <returns>self, fluent interface</returns>
        public ISettingsProvider DeleteKey(string key)
        {
            Previous.DeleteKey(key);
            Current.DeleteKey(key);

            return this;
        }

        /// <summary>
        /// Deletes all keys on both providers
        /// </summary>
        /// <returns>self, fluent interface</returns>
        public ISettingsProvider DeleteAll()
        {
            Previous.DeleteAll();
            Current.DeleteAll();

            return this;
        }

        /// <summary>
        /// Saves changes on both providers and increments the stored version.
        /// </summary>
        /// <returns>self, fluent interface</returns>
        public ISettingsProvider Save()
        {
            Previous.Save();
            Current.Save();

            PlayerPrefs.SetString("Version", Application.version);

            return this;
        }
    }
}