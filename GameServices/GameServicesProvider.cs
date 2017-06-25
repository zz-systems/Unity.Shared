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
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using ZzSystems.Unity.Shared.Settings;
using ZzSystems.TheVoid.Infrastructure.Configuration;

namespace ZzSystems.Unity.Shared.GameServices
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TGameServicesKeys"></typeparam>
    public class GameServicesProvider<TGameServicesKeys>
        where TGameServicesKeys : struct, IConvertible
    {
        private readonly ISettingsProvider _settings;

        private readonly IGameServicesMetadataProvider<TGameServicesKeys> _gameServicesMetadata;

        private readonly Dictionary<TGameServicesKeys, Achievement<TGameServicesKeys>> _cache = new Dictionary<TGameServicesKeys, Achievement<TGameServicesKeys>>();

        public GameServicesProvider(IGameServicesMetadataProvider<TGameServicesKeys> gameServicesMetadataProvider, ISettingsProvider settingsProvider)
        {
            _gameServicesMetadata = gameServicesMetadataProvider;
            _settings = settingsProvider;

            var values = Enum.GetValues(typeof(TGameServicesKeys));
            foreach (object value in values)
            {
                var key = (TGameServicesKeys) value;
                var metadata = _gameServicesMetadata.Resolve(key);
                
                _cache[key] = new Achievement<TGameServicesKeys>
                {
                    GameServiceKey = key,
                    Type = metadata.Type
                };
            }
        }

        public Achievement<TGameServicesKeys> this[TGameServicesKeys key]
        {
            get { return _cache[key]; }
            set { _cache[key] = value; }
        }
        /// <summary>
        /// Submit score / scoreValue value to game service
        /// </summary>
        /// <param name="achievement"></param>
        public void Submit(Achievement<TGameServicesKeys> achievement)
        {
#if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
            if (!PlayGamesPlatform.Instance.IsAuthenticated())
                return;

            var metadata = _gameServicesMetadata.Resolve(achievement.GameServiceKey);
            var value = achievement.Value;

            switch (metadata.Type)
            {
                case Achievement.Types.UnlockableAchievement:
                    PlayGamesPlatform.Instance.ReportProgress(metadata.Id, value != 0 ? 100f : 0f, success => value = success ? 0 : 1);
                    break;
                case Achievement.Types.IncrementalAchievement:
                    PlayGamesPlatform.Instance.IncrementAchievement(metadata.Id, (int)value, success => value = success ? 0 : value);
                    break;
                case Achievement.Types.Score:
                    PlayGamesPlatform.Instance.ReportScore(value, metadata.Id, success => { });
                    break;
            }
#endif
        }

        /// <summary>
        /// Load score / scoreValue from local storage
        /// </summary>
        /// <param name="key">Achievement / scoreValue identifier</param>
        public Achievement<TGameServicesKeys> LoadLocal(TGameServicesKeys key)
        {
            var result = _settings.Get<Achievement<TGameServicesKeys>>(_gameServicesMetadata.Resolve(key).Id);
            result.GameServiceKey = key;

            return result;
        }

        /// <summary>
        /// Save score / scoreValue to local storage
        /// </summary>
        /// <param name="achievement">Achievement / scoreValue to save</param>
        public void SaveLocal(Achievement<TGameServicesKeys> achievement)
        {
            _settings.Set(_gameServicesMetadata.Resolve(achievement.GameServiceKey).Id, achievement);
        }

        /// <summary>
        /// Submit scoreValue to game service
        /// </summary>
        /// <param name="score">Score to submit to</param>
        /// <param name="scoreValue">Value to submit</param>
        /// <returns></returns>
        public bool SubmitScore(Achievement<TGameServicesKeys> score, long scoreValue)
        {
            bool result = scoreValue > score.Value;

            var oldValue = score.Value;
            score.Value = scoreValue;

            Submit(score);

            if (!result)
                score.Value = oldValue;

            return result;
        }

        /// <summary>
        /// Unlock achievement
        /// </summary>
        /// <param name="achievement">Achievement to unlock</param>
        public void UnlockAchievement(Achievement<TGameServicesKeys> achievement)
        {
            achievement.Value = 1;
            achievement.IsUnlocked = true;

            Submit(achievement);
        }

        /// <summary>
        /// Increment achievement
        /// </summary>
        /// <param name="achievement">Achievement to increment</param>
        public void IncrementAchievement(Achievement<TGameServicesKeys> achievement)
        {
            achievement.Value++;
            achievement.RawValue++;

            Submit(achievement);
        }

        /// <summary>
        /// Display the system's leaderbord UI
        /// </summary>
        /// <param name="key">Game service key</param>
        public void ShowLeaderboardUi(TGameServicesKeys key)
        {
#if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
            PlayGamesPlatform.Instance.ShowLeaderboardUI(_gameServicesMetadata.Resolve(key).Id);
#endif
        }

        /// <summary>
        /// Submit scoreValue to game service
        /// </summary>
        /// <param name="key">Score identifier to submit to</param>
        /// <param name="scoreValue">Value to submit</param>
        /// <returns></returns>
        public bool SubmitScore(TGameServicesKeys key, long scoreValue)
        {
            return SubmitScore(this[key], scoreValue);
        }

        /// <summary>
        /// Unlock achievement
        /// </summary>
        /// <param name="key">Achievement identifier to submit to</param>
        public void UnlockAchievement(TGameServicesKeys key)
        {
            UnlockAchievement(this[key]);
        }

        /// <summary>
        /// Increment achievement
        /// </summary>
        /// <param name="key">Achievement identifier to submit to</param>
        public void IncrementAchievement(TGameServicesKeys key)
        {
            IncrementAchievement(this[key]);
        }

        /// <summary>
        /// Submit offline data from local storage / memory as an internet connection may not persist
        /// </summary>
        /// <returns>Enumerator (Coroutine)</returns>
        public IEnumerator SubmitOfflineData()
        {
            var values = Enum.GetValues(typeof(TGameServicesKeys));
            foreach (object value in values)
            {
                var candidate = (TGameServicesKeys)value;
                var metadata = _gameServicesMetadata.Resolve(candidate);

                if (metadata.Type != Achievement.Types.Score)
                {
                    var achievement = LoadLocal(candidate);
                    Submit(achievement);
                    SaveLocal(achievement);
                }

                yield return null;
            }
        }

        /// <summary>
        /// Save all cache values to local storage
        /// </summary>
        public void SaveLocal()
        {
            foreach(var value in _cache.Values)
            {
                SaveLocal(value);
            }
        }
    }
}
