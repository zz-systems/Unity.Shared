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

namespace ZzSystems.Unity.Shared.Settings
{
    /// <summary>
    /// Provides an interface for high-level access to Unity.PlayerPrefs
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>
        /// Gets a value from the setting cache
        /// </summary>
        /// <typeparam name="T">Desired type</typeparam>
        /// <param name="key">Setting key</param>
        /// <param name="defaultValue">Default value in case of failed lookup / conversion</param>
        /// <returns>Found value or default value</returns>
        T Get<T>(string key, T defaultValue = default(T));

        /// <summary>
        /// Sets a value in the settings cache
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="key">Setting key</param>
        /// <param name="value">Value to set</param>
        /// <returns>self, fluent interface</returns>
        ISettingsProvider Set<T>(string key, T value);

        /// <summary>
        /// Checks if a particular key exists
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <returns>true if key exists</returns>
        bool HasKey(string key);

        /// <summary>
        /// Deletes a particular key
        /// </summary>
        /// <param name="key">Key to delete</param>
        /// <returns>self, fluent interface</returns>
        ISettingsProvider DeleteKey(string key);

        /// <summary>
        /// Deletes all keys
        /// </summary>
        /// <returns>self, fluent interface</returns>
        ISettingsProvider DeleteAll();

        /// <summary>
        /// Saves changes
        /// </summary>
        /// <returns>self, fluent interface</returns>
        ISettingsProvider Save();
    }
}