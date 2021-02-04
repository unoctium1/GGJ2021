using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace GameJamCat
{
    public class ActionBoxViewBehaviour : ViewBehaviour
    {

        [SerializeField] private ActionBoxMenuView _actionBoxMenuView = null;

        /* /TODO Add this back in so we have an exit button
        [Title("Properties")]
        [SerializeField] private KeyCode _dossierButton = KeyCode.Tab;
        */

        public event Action OnClaimCat;
        public event Action OnTalkToCat;

        public void SetTargetCat(string catName)
        {
            _actionBoxMenuView?.SetNewCat(catName);
        }

        public override void Initialize()
        {
            _menuView = _actionBoxMenuView;
            base.Initialize();
            if (_actionBoxMenuView != null)
            {
                _actionBoxMenuView.OnClaimCat += HandleClaimCat;
                _actionBoxMenuView.OnTalkToCat += HandleTalkToCat;
            }

        }


        public override void CleanUp()
        {
            if (_actionBoxMenuView != null)
            {
                _actionBoxMenuView.OnClaimCat -= HandleClaimCat;
                _actionBoxMenuView.OnTalkToCat -= HandleTalkToCat;
            }
        }

        private void HandleClaimCat()
        {
            OnClaimCat();
        }
        private void HandleTalkToCat()
        {
            OnTalkToCat();
        }


    }

}

