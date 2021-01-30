using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameJamCat
{
    public class DossierMenuView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _catName = null;
        [SerializeField] private TextMeshProUGUI _catLikes = null;
        [SerializeField] private TextMeshProUGUI _cativities = null;
        [SerializeField] private RawImage _catImage = null;

        //Tentative Use Case from UI Manager
        public void Initialize(KeyCode keycode, string name, string likes, string cativities, Texture2D catimage = null)
        {
            SetUIElement(_catName, name);
            SetUIElement(_catLikes, likes);
            SetUIElement(_cativities, cativities);
            SetUIElement(_catImage, catimage);
        }

        public void SetDossierOpen(bool isCurrentlyOpen)
        {
            transform.localScale = isCurrentlyOpen ? Vector3.one : Vector3.zero;
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
