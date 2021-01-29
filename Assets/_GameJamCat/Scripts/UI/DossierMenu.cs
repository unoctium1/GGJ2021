using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameJamCat
{
    public class DossierMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _catName = null;
        [SerializeField] private TextMeshProUGUI _catLikes = null;
        [SerializeField] private TextMeshProUGUI _cativities = null;
        [SerializeField] private RawImage _catImage = null;

        [SerializeField] private KeyCode _dossierButton = KeyCode.Tab;

        private bool _isMenuOpen = false;
        
        //Tentative Use Case from UI Manager
        public void SetupDossier(string name, string likes, string cativities, Texture2D catimage)
        {
            SetUIElement(_catName, name);
            SetUIElement(_catLikes, likes);
            SetUIElement(_cativities, cativities);
            SetUIElement(_catImage, catimage);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_dossierButton))
            {
                OpenCloseMenu();
            }
        }

        private void OpenCloseMenu()
        {
            if (_isMenuOpen)
            {
                transform.localScale = Vector3.zero;
            }
            else
            {
                transform.localScale = Vector3.one;
            }

            _isMenuOpen = !_isMenuOpen;
        }

        private void SetUIElement(TextMeshProUGUI element, string label)
        {
            if (element != null && !string.IsNullOrEmpty(label))
            {
                element.text = label;
            }
        }

        private void SetUIElement(RawImage element, Texture2D image)
        {
            if (element != null && image != null)
            {
                element.texture = image;
            }
        }
    }
}
