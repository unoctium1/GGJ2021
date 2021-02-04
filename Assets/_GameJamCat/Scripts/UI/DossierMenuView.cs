using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameJamCat
{
    public class DossierMenuView : MenuView
    {
        private const string DossierText = "likes {0}";

        [Title("Placeholder Values")]
        [SerializeField] private Texture2D _defaultTex;

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

        [Title("Sprites")]
        [SerializeField] Sprite _tunaIcon;
        [SerializeField] Sprite _salmonIcon, _chickenIcon, _pumpkinIcon, _dryKibbleIcon, _wetKibbleIcon, _kibbleIcon, _boneIcon, _beefIcon = null;
        [SerializeField] Sprite _yarnBallsIcon, _cardboardBoxIcon, _fishingRodIcon, _catnipSackIcon, _hijinksIcon, _scratchingPostIcon, _laserIcon, _tennisBall = null;

        //Tentative Use Case from UI Manager
        public void SetNewCat(CatCustomisation catDescription, Texture2D catimage = null)
        {
            var food = GetFoodSprite(catDescription._food);
            var cativities = GetToySprite(catDescription._toy);
            SetName(catDescription._catName);
            SetUIElement(_catLikes, food.Item2);
            SetUIElement(_catLikesImage, food.Item1);
            SetUIElement(_cativities, cativities.Item2);
            SetUIElement(_catActivitiesImage, cativities.Item1);
            SetUIElement(_catImage, catimage);
        }

        public void SetTexture(Texture catimage)
        {
            SetUIElement(_catImage, catimage);
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

        private void SetUIElement(RawImage element, Texture image, float duration = 0.5f)
        {
            if (element != null && image != null)
            {
                element.texture = image;
                element.DOFade(1f, duration);
            }
        }

        private void SetUIElement(Image element, Sprite image)
        {
            if (element != null && image != null)
            {
                element.sprite = image;
            }
        }

        public (Sprite, string) GetFoodSprite(Food food)
        {
            return food switch
            {
                Food.Tuna => (_tunaIcon, "TUNA"),
                Food.Salmon => (_salmonIcon, "SALMON"),
                Food.Chicken => (_chickenIcon, "CHICKEN"),
                Food.Pumpkin => (_pumpkinIcon, "PUMPKIN"),
                Food.DryKibble => (_dryKibbleIcon, "DRY KIBBLE"),
                Food.WetKibble => (_wetKibbleIcon, "WET KIBBLE"),
                Food.Kibble => (_kibbleIcon, "KIBBLE"),
                Food.Bones => (_boneIcon, "BONE"),
                Food.Beef => (_beefIcon, "BEEF"),
                _ => (null, null)
            };
        }

        public (Sprite, string) GetToySprite(Toy toy)
        {
            return toy switch
            {
                Toy.YarnBalls => (_yarnBallsIcon, "YARN"),
                Toy.Hijinks => (_hijinksIcon, "HIJINKS"),
                Toy.CardboardBox => (_cardboardBoxIcon, "A BOX"),
                Toy.FishingRod => (_fishingRodIcon, "FISHING"),
                Toy.CatnipSack => (_catnipSackIcon, "CATNIP"),
                Toy.ScratchingPost => (_scratchingPostIcon, "A POST"),
                Toy.TennisBall => (_tennisBall, "TENNIS"),
                Toy.Laser => (_laserIcon, "LASER"),
                _ => (null, null)
            };
        }

        protected override void SetDistance()
        {
            _animationDistance = Screen.height * 0.9f;
        }
    }
}
