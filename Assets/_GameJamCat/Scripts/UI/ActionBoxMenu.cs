using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameJamCat
{
    public class ActionBoxMenu : MonoBehaviour
    {
        private const float AnimationDuration = 0.5f;
        private float _animationActionBoxDistance = 0;

        [Title("CatName")]
        [SerializeField]
        private TMP_Text _catName = null;

        //private float _animationDossierDistance = 0;

        //Tentative Use Case from UI Manager
        public void SetNewCat(CatCustomisation catCustomisation)
        {        
            SetName(catCustomisation._catName);
        }

        public void Initialize()
        {
            var rectTransform = GetComponent<RectTransform>();
            // Top
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -_animationActionBoxDistance);
            // Bottom
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, -_animationActionBoxDistance);
        }

        public void SetActionBoxOpen(bool isCurrentlyOpen)
        {
            var moveDirection = isCurrentlyOpen ? Vector3.up : Vector3.down;
            transform.DOBlendableLocalMoveBy(moveDirection * _animationActionBoxDistance, AnimationDuration, true);
        }

        private void SetName(string catName)
        {
            if (_catName != null)
            {
                _catName.text = catName;
            }
        }

        private void Awake()
        {
            _animationActionBoxDistance = Screen.height * 0.9f;
        }
    }
}
