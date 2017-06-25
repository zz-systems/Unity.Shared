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

namespace ZzSystems.Unity.Shared.GameServices
{
    /// <summary>
    /// Generic achievement/score entity
    /// </summary>
    public class Achievement
    {
        /// <summary>
        /// Game service ID
        /// </summary>
        public Enum GameServiceKey { get; set; }

        /// <summary>
        /// Achievement types
        /// </summary>
        public enum Types
        {
            None,
            Score,
            IncrementalAchievement,
            UnlockableAchievement,
            Local
        }

        /// <summary>
        /// Achievement kind
        /// </summary>
        public Types Type { get; set; }

        /// <summary>
        /// Is this particular achievement unlocked?
        /// </summary>
        public bool IsUnlocked { get; set; }

        /// <summary>
        /// Reserved for future rule based engine.
        /// </summary>
        public long Limit { get; set; }

        /// <summary>
        /// Raw value, be it boolean or integer
        /// </summary>
        public long RawValue { get; set; }

        /// <summary>
        /// Score/Achievement value
        /// </summary>
        public long Value { get; set; }
    }

    /// <summary>
    /// Strongly typed enum mapped achievement entity
    /// </summary>
    /// <typeparam name="TGameServiceKeys"></typeparam>
    public class Achievement<TGameServiceKeys> : Achievement
        where TGameServiceKeys : struct, IConvertible
    {
        /// <summary>
        /// Derived strongly typed achievement key
        /// </summary>
        public new TGameServiceKeys GameServiceKey
        {
            get { return (TGameServiceKeys)(object)base.GameServiceKey; }
            set { base.GameServiceKey = (Enum)(object)value; }
        }
    }
}
