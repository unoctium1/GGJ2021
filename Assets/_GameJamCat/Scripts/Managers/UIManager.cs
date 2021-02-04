using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameJamCat
{
    public class UIManager : MonoBehaviour
    {
        private readonly IStateManager _stateManager = StateManager.Instance;

        [Title("Action Box")]
        [SerializeField] private ActionBoxViewBehaviour _actionBoxView = null;

        [Title("Dialogue")]
        [SerializeField] private DialogueBoxViewBehaviour _dialogueBoxView = null;

        [Title("Screen Transition")]

        [SerializeField] private ScreenTransitionViewBehaviour _transitionViewBehaviour = null;
        [SerializeField] private PregameDialogueBoxBehaviour _pregameDialogueBox = null;

        [Title("Dossier")]
        [SerializeField] private DossierViewBehaviour _dossierView = null;

        [Title("End Game")]
        [SerializeField] private EndGameMenu _endgameViewBehaviour = null;

        [Title("HUD View")]
        [SerializeField] private TimerUI _timer = null;
        [SerializeField] private GameObject _crossHair = null;
        [SerializeField] private LivesViewBehaviour _livesView = null;

        [SerializeField] private GameObject _crossHairText = null;

        private CatBehaviour _targetCat = null;
        public bool ClaimedCat { get; set; } = false;

        // A little hacky, but this ensures when the claim button is passed the world manager is notified
        public event Action<CatBehaviour> OnClaimedCat;

        /// <summary>
        /// Initialize UIManager, setup values here
        /// </summary>
        public void Initialize(int lives)
        {
            _stateManager.OnStateChanged += HandleOnStateChange;

            if (_dossierView != null)
            {
                _dossierView.Initialize();
                _dossierView.OnDossierStateChange += HandleOnDossierStateChange;
            }
            if (_actionBoxView != null)
            {
                _actionBoxView.Initialize();
                _actionBoxView.OnTalkToCat += HandleOnTalkToCat;
                _actionBoxView.OnClaimCat += HandleOnClaimCat;
            }

            if(_dialogueBoxView != null)
            {
                _dialogueBoxView.Initialize();
            }

            if (_pregameDialogueBox != null)
            {
                _pregameDialogueBox.Initialize();
                _pregameDialogueBox.OnDialogueCompleted += HandleOnDialogueCompleted;
            }

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
            _crossHairText.SetActive(false);
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
            if (_dossierView != null)
            {
                _dossierView.OnDossierStateChange -= HandleOnDossierStateChange;
            }

            if (_pregameDialogueBox != null)
            {
                _pregameDialogueBox.OnDialogueCompleted -= HandleOnDialogueCompleted;
            }
            if (_actionBoxView != null)
            {
                _actionBoxView.CleanUp();
                _actionBoxView.OnTalkToCat -= HandleOnTalkToCat;
                _actionBoxView.OnClaimCat -= HandleOnClaimCat;
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
            if (_dossierView != null)
            {
                _dossierView.SetTargetCat(targetCat.CatDialogue);
            }
        }

        public void SetUpDossierTexture(Texture catTex)
        {
            if (_dossierView != null)
            {
                _dossierView.SetCatImage(catTex);
            }
        }

        public void SetCrossHairTextState(bool state)
        {
            _crossHairText.SetActive(state);
        }

        public void SetFocusedCat(CatBehaviour cat)
        {
            _targetCat = cat;
            if (_actionBoxView != null)
            {
                if (cat.IsNameKnown)
                {
                    _actionBoxView.SetTargetCat(cat.CatDialogue._catName);
                }
                else 
                {
                    _actionBoxView.SetTargetCat("???");
                }
            }
            if (_dialogueBoxView != null)
            {
                _dialogueBoxView.SetCat(cat);
            }
        }

        private void OnDestroy()
        {
            CleanUp();
        }

        #region StateChanges
        private void OnPregameSet()
        {
#if UNITY_EDITOR
            HandleOnDialogueCompleted();
#endif

#if !UNITY_EDITOR
            _pregameDialogueBox.StartAnimation();
#endif
            SetCrossHairState(false);
            if (_dialogueBoxView != null && _dialogueBoxView.IsOpen)
            {
                _dialogueBoxView.IsOpen = false;
                _dialogueBoxView.UpdateView();
            }
        }

        private void OnPlaySet()
        {
            if (_dossierView != null)
            {
                _dossierView.SetInstructionLabel(true);
                _dossierView.SetPressToOpenCloseText();
            }

            if (_actionBoxView != null && _actionBoxView.IsOpen)
            {
                _actionBoxView.IsOpen = false;
                _actionBoxView.UpdateView();
            }

            if(_dialogueBoxView != null && _dialogueBoxView.IsOpen)
            {
                _dialogueBoxView.IsOpen = false;
                _dialogueBoxView.UpdateView();
            }



        }

        private void OnDialogueSet()
        {
            SetCrossHairState(false);

            if (_dossierView != null)
            {
                if (_dossierView.IsOpen)
                {
                    _dossierView.IsOpen = false;
                    _dossierView.UpdateView();
                }

                _dossierView.SetInstructionLabel(false);
                _dossierView.SetPressToOpenCloseText();
            }
            if (_actionBoxView != null)
            {
                if (!_actionBoxView.IsOpen)
                {
                    _actionBoxView.IsOpen = true;
                    _actionBoxView.UpdateView();
                }
            }
            if (_dialogueBoxView != null && _dialogueBoxView.IsOpen)
            {
                _dialogueBoxView.IsOpen = false;
                _dialogueBoxView.UpdateView();
            }


        }

        private void OnEndGameSet()
        {
            SetCrossHairState(false);

            if (_actionBoxView != null && _actionBoxView.IsOpen)
            {
                _actionBoxView.IsOpen = false;
                _actionBoxView.UpdateView();
            }

            if (_endgameViewBehaviour != null)
            {
                _endgameViewBehaviour.DisplayEndPanel(ClaimedCat);
            }
            if (_dialogueBoxView != null && _dialogueBoxView.IsOpen)
            {
                _dialogueBoxView.IsOpen = false;
                _dialogueBoxView.UpdateView();
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


        private void HandleOnDialogueCompleted()
        {
            if (_transitionViewBehaviour != null)
            {
                _transitionViewBehaviour.SwitchBlackScreen(true);
            }
        }

        private void HandleOnClaimCat()
        {
            OnClaimedCat(_targetCat);
        }

        private void HandleOnTalkToCat()
        {
            if (_actionBoxView != null)
            {
                _actionBoxView.IsOpen = false;
                _actionBoxView.UpdateView();
            }
            if (_dialogueBoxView != null && !_dialogueBoxView.IsOpen)
            {
                _dialogueBoxView.IsOpen = true;
                _dialogueBoxView.UpdateView();
            }
        }
        #endregion
    }
}
