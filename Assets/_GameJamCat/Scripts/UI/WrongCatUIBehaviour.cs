using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace GameJamCat
{
    public class WrongCatUIBehaviour : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.DOScale(Vector3.zero,0.5f).SetEase(Ease.OutBack);
        }

        private void OnEnable()
        {
            _rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            StartCoroutine(WaitAndThenHide());
        }

        private void OnDisable()
        {
            HideBox();
        }

        IEnumerator WaitAndThenHide()
        {
            yield return new WaitForSeconds(1f);
            HideBox();
        }

        private void HideBox()
        {
            _rectTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBack);
        }
    }
}
