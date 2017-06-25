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
using Random = UnityEngine.Random;

namespace ZzSystems.Unity.Shared.Util
{
    /// <summary>
    /// Extends collections with random features
    /// </summary>
    public static class RandomExtensions  {

        /// <summary>
        /// Pick a single random element from a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Source collection</param>
        /// <returns>Value at a random position in the collection</returns>
        public static T RandomElement<T>(this IList<T> source)
        {
            return source[Random.Range(0, source.Count)];
        }

        /// <summary>
        /// Pick a single random element from an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Source collection</param>
        /// <returns>Value at a random position in the collection</returns>
        public static T RandomElement<T>(this T[] source)
        {
            return source[Random.Range(0, source.Length)];
        }

        /// <summary>
        /// Pick a single random element from an array if it is not previously taken
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Source collection</param>
        /// <param name="lastItem">Previosly taken value</param>
        /// <returns>Not a previously taken value at a random position in the collection</returns>
        public static T UniqueRandomElement<T>(this T[] source, T lastItem)
            where T : IEquatable<T>
        {
            var item = source.RandomElement();

            while (item.Equals(lastItem))
            {
                item = source.RandomElement();
            }

            return item;
        }

        /// <summary>
        ///  Pick a single random element from a list if it is not previously taken
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Source collection</param>
        /// <param name="lastItem">Previosly taken value</param>
        /// <returns>Not a previously taken value at a random position in the collection</returns>
        public static T UniqueRandomElement<T>(this IList<T> source, T lastItem)
            where T : IEquatable<T>
        {
            var item = source.RandomElement();

            while (item.Equals(lastItem))
            {
                item = source.RandomElement();
            }

            return item;
        }
    }
}
