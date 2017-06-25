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
    /// Implement this interface to provide custom hashing, encryption and decryption
    /// </summary>
    public interface ICryptoProvider
    {
        /// <summary>
        /// Encrypt provided plain text
        /// </summary>
        /// <param name="plainText">string to encrypt</param>
        /// <returns>encrypted string</returns>
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypt provided encrypted text
        /// </summary>
        /// <param name="encryptedText">string to decrypt</param>
        /// <returns>decrypted string</returns>
        string Decrypt(string encryptedText);

        /// <summary>
        /// Generate fingerprint for provided string
        /// </summary>
        /// <param name="text">string to hash</param>
        /// <returns>fingerprint</returns>
        string Hash(string text);
    }
}