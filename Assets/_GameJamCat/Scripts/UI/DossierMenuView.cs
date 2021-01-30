using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameJamCat
{
    public class DossierMenuView : MonoBehaviour
    {
        private const string DossierText = "likes {0}";

        [Title("Poster Cat Image")]
        [SerializeField] private RawImage _catImage = null;
        [Title("Name")]
        [SerializeField] private TMP_Text _catName = null;
        [Title("Likes")] 
        [SerializeField] private Image _catLikesImage = null;
        [SerializeField] private TMP_Text _catLikes = null;
        [Title("Cativities")]
        [SerializeField] private Image _catActivitiesImage = null;
        [SerializeField] private TMP_Text _cativities = null;

        //Tentative Use Case from UI Manager
        public void Initialize(string catName, string likes, string cativities, Texture2D catimage = null)
        {
            SetName(catName);
            SetUIElement(_catLikes, likes);
            SetUIElement(_cativities, cativities);
            SetUIElement(_catImage, catimage);
        }

        public void SetDossierOpen(bool isCurrentlyOpen)
        {
            transform.localScale = isCurrentlyOpen ? Vector3.one : Vector3.zero;
        }

        private void SetName(string catName)
        {
            if (_catName != null)
            {
                _catName.text = catName;
            }
        }

        private void SetUIElement(TMP_Text element, string label)
        {
            if (element != null && !string.IsNullOrEmpty(label))
            {
                element.text = string.Format(DossierText, label);
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
