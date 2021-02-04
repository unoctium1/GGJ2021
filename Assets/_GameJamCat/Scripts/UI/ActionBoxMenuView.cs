using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameJamCat
{
    public class ActionBoxMenuView : MenuView
    {        

        [Title("CatName")]
        [SerializeField]
        private TMP_Text _catName = null;

        //private CatBehaviour _targetCat = null;

        public event Action OnClaimCat;
        public event Action OnTalkToCat;

        public void SetNewCat(string catName)
        {
            //_targetCat = cat;
            SetName(catName);
        }

        private void SetName(string catName)
        {
            if (_catName != null)
            {
                _catName.text = catName;
            }
        }

        // Called by the UI claim button
        public void ClaimCat()
        {
            if (OnClaimCat != null)
            {
                OnClaimCat();
            }
        }

        // Called by the UI chat button
        public void ChatToCat()
        {
            if (OnTalkToCat != null)
            {
                OnTalkToCat();
            }
        }

        
    }
}
