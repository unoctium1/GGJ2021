using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GameJamCat
{
    public class ScreenTransitionViewBehaviour : MonoBehaviour
    {
        [Title("Properties")]
        [SerializeField] private Image _blackScreen = null;
        [SerializeField] private float _fadeInDuration = 3;

        public event Action OnCompleteFade;
        
        /// <summary>
        /// Fades In or Out to black with duration
        /// </summary>
        /// <param name="isFadingIn">should fade in</param>
        public void SwitchBlackScreen(bool isFadingIn)
        {
            var alphaValue = isFadingIn ? 0 : 1;
            if (_blackScreen != null)
            {
                _blackScreen.CrossFadeAlpha(alphaValue, _fadeInDuration, true);
                StartCoroutine(FadeCountDown());
            }
        }

        private IEnumerator FadeCountDown()
        {
            yield return new WaitForSeconds(_fadeInDuration);
            if (OnCompleteFade != null)
            {
                OnCompleteFade();
            }
        }

        private void OnEnable()
        {
            _blackScreen.color = Color.black;
        }
    }
}
