using System;
using UnityEngine;

namespace GameJamCat
{
    public class UIManager : MonoBehaviour
    {
        private readonly IStateManager _stateManager = StateManager.Instance;
        
        [SerializeField] private DossierViewBehaviour _dossierView = null;
        [SerializeField] private ScreenTransitionViewBehaviour _transitionViewBehaviour = null;
        
        /// <summary>
        /// Initialize UIManager, setup values here
        /// </summary>
        public void Initialize()
        {
            if (_dossierView != null)
            {
                _dossierView.Initialize();
            }
            
            _stateManager.OnStateChanged += HandleOnStateChange;
            _transitionViewBehaviour.OnCompleteFade += HandleOnFadeComplete;
        }

        /// <summary>
        /// Reset UI Values here, unsubscribe or reset values here
        /// </summary>
        public void CleanUp()
        {
            
        }
        
        #region Delegate

        private void HandleOnStateChange(State state)
        {
            switch (state)
            {
                case State.Pregame:
                    _transitionViewBehaviour.SwitchBlackScreen(true);
                    break;
                case State.Play:
                    break;
                case State.Dialogue:
                    break;
                case State.EndGame:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void HandleOnFadeComplete()
        {
            _stateManager.SetState(State.Play);
        }
        #endregion
    }
}
