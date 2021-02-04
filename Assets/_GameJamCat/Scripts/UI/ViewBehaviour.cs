using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJamCat
{
    public abstract class ViewBehaviour : MonoBehaviour
    {
        protected MenuView _menuView = null;

        private bool _isOpen = false;

        public event Action<bool> OnStateChange;

        // override this to change default open/close state
        public virtual bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                if (OnStateChange != null)
                {
                    OnStateChange(_isOpen);
                }
            }
        }

        public virtual void Initialize()
        {
            if(_menuView != null)
            {
                _menuView.Initialize();
                _menuView.SetOpen(IsOpen);
            }
        }

        public void UpdateView()
        {
            if (_menuView != null)
            {
                _menuView.SetOpen(IsOpen);
            }
        }

        public abstract void CleanUp();
    }
}
