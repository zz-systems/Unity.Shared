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

namespace ZzSystems.Unity.Shared.Security
{
    /// <summary>
    /// Implement this interface to provide custom encryption settings
    /// </summary>
    public interface ICryptoKeyProvider
    {
        /// <summary>
        /// Encryption key length, e.g 128, 192, 256 bits
        /// </summary>
        int KeyLength { get; }

        /// <summary>
        /// Password used for encryption
        /// </summary>
        string Secret { get; }

        /// <summary>
        /// Initial vector provider
        /// </summary>
        byte[] InitialVector { get; }

        /// <summary>
        /// Salt provider
        /// </summary>
        byte[] Salt { get; }

        /// <summary>
        /// Key generator
        /// </summary>
        /// <param name="salt">Salt used for key generation</param>
        /// <returns>Generated key</returns>
        byte[] GetKey(byte[] salt);
    }
}