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

using System.Security.Cryptography;

namespace ZzSystems.Unity.Shared.Security
{
    /// <summary>
    /// Generic encryption settings
    /// </summary>
    public abstract class CryptoKeyProviderBase : ICryptoKeyProvider
    {
        /// <summary>
        /// Encryption key length, e.g 128, 192, 256 bits
        /// </summary>
        public virtual int KeyLength
        {
            get { return 128; }
        }

        /// <summary>
        /// Password used for encryption
        /// </summary>
        public abstract string Secret { get; }

        /// <summary>
        /// Initial vector provider
        /// </summary>
        public virtual byte[] InitialVector
        {
            get
            {
                var iv = new byte[KeyLength / 8];
                RandomNumberGenerator.Create().GetBytes(iv);

                return iv;
            }
        }

        /// <summary>
        /// Salt provider
        /// </summary>
        public virtual byte[] Salt
        {
            get
            {
                var salt = new byte[KeyLength / 8];
                RandomNumberGenerator.Create().GetBytes(salt);

                return salt;
            }
        }

        /// <summary>
        /// Key generator
        /// </summary>
        /// <param name="salt">Salt used for key generation</param>
        /// <returns>Generated key</returns>
        public virtual byte[] GetKey(byte[] salt)
        {
            return new Rfc2898DeriveBytes(Secret, salt).GetBytes(KeyLength / 8);
        }
    }
}
