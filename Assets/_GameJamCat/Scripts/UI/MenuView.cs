using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJamCat
{

    /// <summary>
    /// Base case for core UI objects. This handles animating screen wipes.
    /// Classes should override them if they have custom UI elements that need to be updated
    /// </summary>
    public abstract class MenuView : MonoBehaviour
    {
        protected const float AnimationDuration = 0.5f;
        protected float _animationDistance = 0;

        private bool _isOpen = true; 

        public virtual void Initialize()
        {
            SetDistance();
        }

        public virtual void SetOpen(bool isCurrentlyOpen)
        {
            if(_isOpen != isCurrentlyOpen){ // don't do anything if the panel is already open
                _isOpen = isCurrentlyOpen;
                var moveDirection = isCurrentlyOpen ? Vector3.up : Vector3.down;
                transform.DOBlendableLocalMoveBy(moveDirection * _animationDistance, AnimationDuration, true);
            }
        }

        protected virtual void SetDistance()
        {
            _animationDistance = Screen.height * 1f;
        }
    }
}
