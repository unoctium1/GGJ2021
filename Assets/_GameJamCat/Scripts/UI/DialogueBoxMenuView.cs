using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJamCat
{
    public class DialogueBoxMenuView : MenuView
    {
        private const string PlaceholderName = "???";
        private const float TextFontScale = 0.8f;

        [Title("Panels")]
        [SerializeField] private GameObject _dialogueBoxPanel;
        [SerializeField] private GameObject _optionBoxPanel;

        [Title("Dialogue Box Components")]
        [SerializeField] private TMPro.TextMeshProUGUI _nameField;
        [SerializeField] private DialogueBoxText _dialogueBoxText;


        private CatBehaviour _cat;

        public override void Initialize()
        {
            base.Initialize();
            if (_dialogueBoxText != null)
            {
                _dialogueBoxText.OnReadCompleted += ShrinkDialogueAndReturnToOptions;
            }

            if(_nameField != null && _dialogueBoxText != null)
            {
                _dialogueBoxText.fontSize = _nameField.fontSize * TextFontScale; //hacky way to make sure dialogue box scales without weird resizing
            }
        }

        private void OnDisable()
        {
            if (_dialogueBoxText != null)
            {
                _dialogueBoxText.OnReadCompleted -= ShrinkDialogueAndReturnToOptions;
            }
        }

        public void SetupDialogueAnswers(CatBehaviour cat)
        {
            _cat = cat;
            SetName();
        }

        private void SetName()
        {
            if (_cat.IsNameKnown)
            {
                _nameField.text = _cat.CatDialogue._catName;
            }
            else
            {
                _nameField.text = PlaceholderName;
            }
        }

        private void ShrinkDialogueAndReturnToOptions()
        {
            _dialogueBoxText?.Hide();
            _optionBoxPanel?.SetActive(true);
        }

        private void SwitchToDialoguePanel()
        {
            _optionBoxPanel?.SetActive(false);
            _dialogueBoxPanel?.SetActive(true);
            _dialogueBoxText?.Show();
        }

        #region BUTTON_EVENTS
        public void LeaveButton()
        {
            StateManager.Instance.SetState(State.Play); //switch states and let managers handle closing boxes
        }

        public override void SetOpen(bool isCurrentlyOpen)
        {
            base.SetOpen(isCurrentlyOpen);
            _dialogueBoxPanel?.SetActive(false);
            _optionBoxPanel?.SetActive(true);
        }

        public void AskNameButton()
        {
            if (_cat != null)
            {
                SwitchToDialoguePanel();
                _dialogueBoxText.ReadText(_cat.CatDialogue._catNameAnswer);
                if (!_cat.IsNameKnown)
                {
                    _cat.IsNameKnown = true;
                    SetName();
                }
            }
        }

        public void AskFoodButton()
        {
            if(_cat != null)
            {
                SwitchToDialoguePanel();
                _dialogueBoxText.ReadText(_cat.CatDialogue._catFoodAnswer);
            }
        }

        public void AskToyButton()
        {
            if(_cat != null)
            {
                SwitchToDialoguePanel();
                _dialogueBoxText.ReadText(_cat.CatDialogue._catActivityAnswer);
            }
        }
        #endregion //BUTTON_EVENTS


    }
}
