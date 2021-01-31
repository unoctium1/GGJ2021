using System;
using UnityEngine;

namespace GameJamCat
{
    public class UIManager : MonoBehaviour
    {
        private readonly IStateManager _stateManager = StateManager.Instance;
        
        [SerializeField] private DossierViewBehaviour _dossierView = null;
        [SerializeField] private ScreenTransitionViewBehaviour _transitionViewBehaviour = null;
        [SerializeField] private EndGameMenu _endgameViewBehaviour = null;
        [SerializeField] private TimerUI _timer = null;
        [SerializeField] private GameObject _crossHair = null;
        [SerializeField] private LivesViewBehaviour _livesView = null;
        
        /// <summary>
        /// Initialize UIManager, setup values here
        /// </summary>
        public void Initialize(int lives)
        {
            if (_dossierView != null)
            {
                _dossierView.Initialize();
                _dossierView.OnDossierStateChange += HandleOnDossierStateChange;
            }
            
            _stateManager.OnStateChanged += HandleOnStateChange;
            
            if (_transitionViewBehaviour != null)
            {
                _transitionViewBehaviour.OnCompleteFade += HandleOnFadeComplete;
            }

            if (_endgameViewBehaviour != null)
            {
                _endgameViewBehaviour.Initialize();
            }

            if (_crossHair != null)
            {
                _crossHair.gameObject.SetActive(false);
            }

            if (_timer != null)
            {
                _timer.Initialize();
            }

            SetLives(lives);
        }

        public void UpdateTimer(float time)
        {
            if (_timer != null)
            {
                _timer.UpdateTime(time);
            }
        }

        /// <summary>
        /// Reset UI Values here, unsubscribe or reset values here
        /// </summary>
        public void CleanUp()
        {
            _stateManager.OnStateChanged -= HandleOnStateChange;
            if (_transitionViewBehaviour != null)
            {
                _transitionViewBehaviour.OnCompleteFade -= HandleOnFadeComplete;
            }
        }

        public void SetLives(int lives)
        {
            if (_livesView != null)
            {
                _livesView.SetLiveImage(lives);
            }
        }       
        
        public void SetCrossHairState(bool state)
        {
            if (_crossHair != null)
            {
                _crossHair.SetActive(state);
            }
        }

        public void SetUpDossier(CatBehaviour targetCat)
        {
            if(_dossierView != null)
            {
                _dossierView.SetTargetCat(targetCat.CatDialogue);
            }
        }

        public void SetUpDossierTexture(Texture catTex)
        {
            if(_dossierView != null)
            {
                _dossierView.SetCatImage(catTex);
            }
        }

        private void OnDestroy()
        {
            CleanUp();
        }        
        
        #region StateChanges
        private void OnPregameSet()
        {
            if (_transitionViewBehaviour != null) 
            { 
                _transitionViewBehaviour.SwitchBlackScreen(true); 
            }

            SetCrossHairState(false);
        }

        private void OnPlaySet()
        {
            if (_dossierView != null)
            {
                _dossierView.SetInstructionLabel(true);
                _dossierView.SetPressToOpenCloseText();
            }
        }
        
        private void OnDialogueSet()
        {
            if (_dossierView != null)
            {
                if (_dossierView.IsDossierOpen)
                {
                    _dossierView.IsDossierOpen = false;
                    _dossierView.UpdateDossierView();
                    SetCrossHairState(false);
                }
            
                _dossierView.SetInstructionLabel(false);
                _dossierView.SetPressToOpenCloseText();
            } 
        }
        
        private void OnEndGameSet()
        {
            SetCrossHairState(false);

            if (_endgameViewBehaviour != null)
            {
                _endgameViewBehaviour.DisplayEndPanel(true); //placeholder boolean
            }
        }
        #endregion
        
        #region Delegate

        private void HandleOnStateChange(State state)
        {
            switch (state)
            {
                case State.Pregame:
                    OnPregameSet();
                    break;
                case State.Play:
                    OnPlaySet();
                    break;
                case State.Dialogue:
                    OnDialogueSet();
                    break;
                case State.EndGame:
                    OnEndGameSet();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void HandleOnFadeComplete()
        {
            _stateManager.SetState(State.Play);
        }
        
        private void HandleOnDossierStateChange(bool isOpen)
        {
            SetCrossHairState(!isOpen);
        }
        #endregion 
    }
}
