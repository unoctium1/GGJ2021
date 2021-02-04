using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace GameJamCat
{
    public class DossierViewBehaviour : ViewBehaviour
    {
        private const string OpenLabel = "open";
        private const string CloseLabel = "close";
        private const string InstructionalLabel = "Press {0} to {1} Dossier";
        
        [SerializeField] private DossierMenuView _dossierMenuView = null;
        [SerializeField] private TMP_Text _pressToOpenClose = null;

        [Title("Properties")]
        [SerializeField] private KeyCode _dossierButton = KeyCode.Tab;

        //Set open by default
        private bool _isDossierOpen = true;

        public event Action<bool> OnDossierStateChange;

        public override bool IsOpen
        {
            get => _isDossierOpen;
            set
            {
                _isDossierOpen = value; //test
                if(OnDossierStateChange != null)
                {
                    OnDossierStateChange(_isDossierOpen);
                }
            }
        }

        public void SetTargetCat(CatCustomisation catCustomisation, Texture2D catImage = null)
        {
            if (_dossierMenuView != null)
            {
                _dossierMenuView.SetNewCat(catCustomisation, catImage);
            }
        }

        public void SetCatImage(Texture catimage)
        {
            if(_dossierMenuView != null)
            {
                _dossierMenuView.SetTexture(catimage);
            }
        }

        public override void Initialize()
        {
            _menuView = _dossierMenuView;
            base.Initialize();
            SetPressToOpenCloseText();
        }
        
        public void SetPressToOpenCloseText()
        {
            var openOrClose = IsOpen ? CloseLabel : OpenLabel;
            if (_pressToOpenClose != null)
            {
                _pressToOpenClose.text = string.Format(InstructionalLabel, _dossierButton.ToString(), openOrClose);
            }
        }

        /// <summary>
        /// Set Instruction Label
        /// </summary>
        /// <param name="turnOn">turn on off label</param>
        public void SetInstructionLabel(bool turnOn)
        {
            if (_pressToOpenClose != null)
            {
                _pressToOpenClose.gameObject.SetActive(turnOn);
            }
        }

        public override void CleanUp()
        {
            
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(_dossierButton) && StateManager.Instance.GetState() != State.Dialogue)
            {
                IsOpen = !IsOpen;
                UpdateView();
                SetPressToOpenCloseText();
            }
        }



    }
}
