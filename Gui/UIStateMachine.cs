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

using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using ZzSystems.Unity.Shared.Resources;

namespace ZzSystems.Unity.Shared.Gui
{
    public enum ActionModes
    {
        None = -1,
    
        Hide = 0,
        Show = 1
    }

    /// <summary>
    /// UI state machine. Allows nested actions and interactions.
    /// Responsible for dialog and component states.
    /// </summary>
    public class UIStateMachine : MonoBehaviour
    {
        private Stack<MenuItem> _states;

        public bool HasState { get { return _states.Any(); } }
        private MenuItem Current { get { return _states.Any() ? _states.Peek() : MainMenu; } }

        public MenuItem MainMenu;
        public MenuItem GameOverMenu;
        public UnityEvent OnMenuEnterEvent;
        public UnityEvent OnMenuExitEvent;

        [FormerlySerializedAs("Paragraph")]
        public readonly ReactiveProperty<string> CurrentStateName = new ReactiveProperty<string>();

        private Localization _i18n;


        void Awake()
        {
            _states = new Stack<MenuItem>();

            _i18n = FindObjectOfType<Localization>();
        }

        /// <summary>
        /// Open specific menu item
        /// </summary>
        /// <param name="root"></param>
        public void Open(MenuItem root)
        {
            _states.Clear();
        
            GoTo(root);
        }

        /// <summary>
        /// Close specific menu item
        /// </summary>
        /// <param name="root"></param>
        public void Close(MenuItem root)
        {
            _states.Clear();

            root.gameObject.SetActive(false);
        }
    
        /// <summary>
        /// Go to specific menu item
        /// </summary>
        /// <param name="next"></param>
        public void GoTo(MenuItem next)
        {
            var current = Current;

            if (current == MainMenu)
            {
                Set(next);
            }
            else if(next.Layer > current.Layer)
            {
                Set(next);
            }
            else if (next.Layer <= current.Layer)
            {
                Previous();

                if(!next.IgnoredSources.Contains(current))
                
                    Set(next);
            } 
            else 
            {
                Debug.Log("Wrong menu state");
            }
        }

        /// <summary>
        /// Set current state.
        /// Hide previous items
        /// </summary>
        /// <param name="next"></param>
        private void Set(MenuItem next)
        {
            next.gameObject.SetActive(true);

            for (var i = 0; i < next.AffectedTargets.Count; i++)
                next.AffectedTargets[i].Hide();

            for (int i = 0; i < next.ShowToo.Count; i++)
                next.ShowToo[i].Show();

            _states.Push(next);
            next.Show();

            if (next.LocalizationKey != null)
                CurrentStateName.Value = _i18n.Get(next.LocalizationKey);
        }

        /// <summary>
        /// Go to last state
        /// </summary>
        public void Previous()
        {
            if (Current == null)
                return;

            for (int i = 0; i < Current.AffectedTargets.Count; i++)
                Current.AffectedTargets[i].Show();

            for (int i = 0; i < Current.ShowToo.Count; i++)
                Current.ShowToo[i].Hide();

            Current.Hide();

            if(_states.Count > 0)
                _states.Pop();
            
            if(Current != null)
                CurrentStateName.Value = _i18n.Get(Current.LocalizationKey);
        }

        /// <summary>
        /// Handle 'escape' event.
        /// Sometimes you cannot go back further, and in other cases you quit the application
        /// </summary>
        public void HandleEscape()
        {
            // Still in menu -> go back
            if (_states.Any())
            {
                if (Current.CanGoBack)
                {
                    Previous();
                }
                else if (Current == MainMenu && OnMenuExitEvent != null)
                {
                    // Left menu -> trigger exit event
                    OnMenuExitEvent.Invoke();
                }
            }
            // Not in menu -> enter
            else if(MainMenu != null)
            {
                GoTo(MainMenu);

                if (OnMenuEnterEvent != null)
                    OnMenuEnterEvent.Invoke();
            }
        }
    }
}