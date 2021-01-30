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
        
        public void Initialize()
        {
            _dossierMenuView.Initialize(_dossierButton, "Mittens", "Fish", "Reading");
            SetPressToOpenCloseText();
            _dossierMenuView.SetDossierOpen(_isDossierOpen);
        }

        public void CleanUp()
        {
            
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(_dossierButton))
            {
                _isDossierOpen = !_isDossierOpen;
                _dossierMenuView.SetDossierOpen(_isDossierOpen);
                SetPressToOpenCloseText();
            }
        }

        private void SetPressToOpenCloseText()
        {
            var openOrClose = _isDossierOpen ? CloseLabel : OpenLabel;
            _pressToOpenClose.text = string.Format(InstructionalLabel, _dossierButton.ToString(), openOrClose);
        }

    }
}
