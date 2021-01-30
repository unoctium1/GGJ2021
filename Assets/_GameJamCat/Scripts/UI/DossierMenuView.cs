using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameJamCat
{
    public class DossierMenuView : MonoBehaviour
    {
        [Title("Poster Cat Image")]
        [SerializeField] private RawImage _catImage = null;
        [Title("Name")]
        [SerializeField] private TextMeshProUGUI _catName = null;
        [Title("Likes")] 
        [SerializeField] private Image _catLikesImage = null;
        [SerializeField] private TextMeshProUGUI _catLikes = null;
        [Title("Cativities")]
        [SerializeField] private Image _catActivitiesImage = null;
        [SerializeField] private TextMeshProUGUI _cativities = null;

        //Tentative Use Case from UI Manager
        public void Initialize(string name, string likes, string cativities, Texture2D catimage = null)
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
