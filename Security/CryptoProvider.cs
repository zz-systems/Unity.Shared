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
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZzSystems.Unity.Shared.Security
{
    /// <summary>
    /// Provides a hashing, encryption and decryption abstraction layer
    /// </summary>
    public class CryptoProvider : ICryptoProvider
    {
        /// <summary>
        /// CryptoProvider instance
        /// </summary>
        private readonly ICryptoKeyProvider _keyProvider;

        /// <summary>
        /// Creates an instance of CryptoProvider with a ICryptoKeyProvider instance 
        /// </summary>
        /// <param name="keyProvider">ICryptoKeyProvider used for encryption settings</param>
        public CryptoProvider(ICryptoKeyProvider keyProvider)
        {
            _keyProvider = keyProvider;
        }

        /// <summary>
        /// Encrypt provided plain text using AES/CBC/PKCS7
        /// </summary>
        /// <param name="plainText">string to encrypt</param>
        /// <returns>encrypted string</returns>
        public string Encrypt(string plainText)
        {            
            var salt        = _keyProvider.Salt;
            var iv          = _keyProvider.InitialVector;
            var key         = _keyProvider.GetKey(salt);

            var input       = Encoding.UTF8.GetBytes(plainText);

            using (var cipher = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 })            
            using (var encryptor = cipher.CreateEncryptor(key, iv))                
            using (var ms = new MemoryStream())                    
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                ms.Write(salt, 0, salt.Length);
                ms.Write(iv, 0, iv.Length);

                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();
                
                return Convert.ToBase64String(ms.ToArray());
            }      
        }

        /// <summary>
        /// Decrypt provided encrypted text using AES/CBC/PKCS7
        /// </summary>
        /// <param name="encryptedText">string to decrypt</param>
        /// <returns>decrypted string</returns>
        public string Decrypt(string encryptedText)
        {
            var input = Convert.FromBase64String(encryptedText);

            var len = _keyProvider.KeyLength / 8;

            var salt = new byte[len];
            var iv = new byte[len];

            using (var ms = new MemoryStream(input))
            {
                ms.Read(salt, 0, len);
                ms.Read(iv, 0, len);

                var key = _keyProvider.GetKey(salt);

                using (var cipher = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 })
                using (var decryptor = cipher.CreateDecryptor(key, iv))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    var output = new byte[input.Length];
                    var count = cs.Read(output, 0, input.Length);

                    return Encoding.UTF8.GetString(output, 0, count).TrimEnd("\0".ToCharArray());
                }
            }
        }

        /// <summary>
        /// Generate fingerprint for provided string using SHA1
        /// </summary>
        /// <param name="text">string to hash</param>
        /// <returns>fingerprint</returns>
        public string Hash(string text)
        {
            using (var provider = new SHA1CryptoServiceProvider())
            {
                var data = provider.ComputeHash(new UTF8Encoding().GetBytes(text + _keyProvider.Secret));

                var sb = new StringBuilder();
                foreach (byte b in data)
                {
                    var hex = b.ToString("x2");
                    sb.Append(hex);
                }
                return sb.ToString();
            }
        }
    }
}
