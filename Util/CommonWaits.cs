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

namespace ZzSystems.Unity.Shared.Util
{
    /// <summary>
    /// A collection of most common wait values used in coroutines
    /// </summary>
    public static class CommonWaits {

        public static readonly WaitForSeconds Wait1_0S  = new WaitForSeconds(1f);
        public static readonly WaitForSeconds Wait0_1S  = new WaitForSeconds(0.1f);
        public static readonly WaitForSeconds Wait0_5S  = new WaitForSeconds(0.5f);
    }
}
