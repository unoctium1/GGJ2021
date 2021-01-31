using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace GameJamCat
{
    public class DossierViewBehaviour : MonoBehaviour
    {
        private const string OpenLabel = "open";
        private const string CloseLabel = "close";
        private const string InstructionalLabel = "Press {0} to {1} Dossier";
        
        [SerializeField] private DossierMenuView _dossierMenuView = null;
        [SerializeField] private TMP_Text _pressToOpenClose = null;

        [Title("Properties")]
        [SerializeField] private KeyCode _dossierButton = KeyCode.Tab;

        private bool _isDossierOpen = true;

        public event Action<bool> OnDossierStateChange;

        public bool IsDossierOpen
        {
            get => _isDossierOpen;
            set
            {
                _isDossierOpen = value;
                if (OnDossierStateChange != null)
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

        public void Initialize()
        {
            if (_dossierMenuView != null)
            {
                _dossierMenuView.Initialize();
                _dossierMenuView.SetDossierOpen(_isDossierOpen);
            }

            SetPressToOpenCloseText();
        }
        
        public void SetPressToOpenCloseText()
        {
            var openOrClose = IsDossierOpen ? CloseLabel : OpenLabel;
            if (_pressToOpenClose != null)
            {
                _pressToOpenClose.text = string.Format(InstructionalLabel, _dossierButton.ToString(), openOrClose);
            }
        }

        public void UpdateDossierView()
        {
            if (_dossierMenuView != null)
            {
                _dossierMenuView.SetDossierOpen(IsDossierOpen);
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

        public void CleanUp()
        {
            
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(_dossierButton) && StateManager.Instance.GetState() != State.Dialogue)
            {
                IsDossierOpen = !IsDossierOpen;
                UpdateDossierView();
                SetPressToOpenCloseText();
            }
        }



    }
}
