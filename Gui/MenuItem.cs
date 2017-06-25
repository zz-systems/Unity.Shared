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
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace ZzSystems.Unity.Shared.Gui
{
    /// <summary>
    /// Menu entry usable by UI state machine
    /// </summary>
    public class MenuItem : MonoBehaviour
    {
        private Animator _ownAnimator;

        [FormerlySerializedAs("Name")]
        public string LocalizationKey;

        public readonly ReactiveProperty<string> DynamicName = new ReactiveProperty<string>();
    
        [Range(0, 10)]
        public int Layer;

        //[FormerlySerializedAs("Affected")]
        public List<MenuItem> AffectedTargets       = new List<MenuItem>();

        public List<MenuItem> ShowToo               = new List<MenuItem>(); 
        //[FormerlySerializedAs("IgnoreIf")]
        public List<MenuItem> IgnoredSources        = new List<MenuItem>();

        public bool CanGoBack = true;
        

        // Use this for initialization
        void Awake ()
        {
            _ownAnimator = GetComponent<Animator>();
        }
	
        public virtual void Show()
        {
            CancelInvoke("Disable");
            gameObject.SetActive(true);

            if (_ownAnimator != null)
            {
                //_ownAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                _ownAnimator.enabled = true;
                _ownAnimator.SetInteger("mode", (int) ActionModes.Show);
            }
            //Invoke("DisableAnimator", 1f);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(true);
            if (_ownAnimator != null)
            {
                _ownAnimator.enabled = true;
                _ownAnimator.SetInteger("mode", (int) ActionModes.Hide);
                Invoke("Disable", 1f);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public ActionModes CurrentMode
        {
            get
            {
                return isActiveAndEnabled ? (ActionModes) _ownAnimator.GetInteger("mode") : ActionModes.Hide;
            }
        }

        public bool IsVisible
        {
            get { return CurrentMode == ActionModes.Show; }
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            //_ownAnimator.cullingMode = AnimatorCullingMode.CullCompletely;
        }
    }
}
