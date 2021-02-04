using System;
using System.Collections;
using TMPro;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GameJamCat
{
    public class DialogueBoxText : TextMeshProUGUI
    {
        public event Action OnReadCompleted;
        
        private float _speed = 50f;

        private RectTransform _dialougeBoxTransform;
        

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _dialougeBoxTransform.DOScale(0, 0).SetEase(Ease.OutBack);
        }

        protected override void Awake()
        {
            base.Awake();
            _dialougeBoxTransform = transform.parent.GetComponent<RectTransform>();
            _dialougeBoxTransform.DOScale(0, 0);
        }

        public void Show()
        {
            _dialougeBoxTransform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        }

        public void Hide()
        {
            _dialougeBoxTransform.DOScale(0, 0.5f).SetEase(Ease.OutBack);
        }

        public void ReadText(string newtext)
        {
            text = string.Empty;

            string[] subTexts = newtext.Split('<', '>');

            string displayText = "";
            for (int i = 0; i < subTexts.Length; i++)
            {
                if (i % 2 == 0)
                {
                    displayText += subTexts[i];
                }
                else if (!isCustomTag(subTexts[i].Replace(" ", " ")))
                {
                    displayText += $"<{subTexts[i]}>";
                }
            }

            bool isCustomTag(string v)
            {
                return tag.StartsWith("speed=") || tag.StartsWith("pause=");
            }

            text = displayText;
            maxVisibleCharacters = 0;
            StartCoroutine(Read());

            IEnumerator Read()
            {
                int subCounter = 0;
                int visibleCounter = 0;
                while (subCounter < subTexts.Length)
                {
                    // if 
                    if (subCounter % 2 == 1)
                    {
                        yield return EvaluateTag(subTexts[subCounter].Replace(" ", ""));
                    }
                    else
                    {
                        while (visibleCounter < subTexts[subCounter].Length)
                        {

                            visibleCounter++;
                            maxVisibleCharacters++;
                            yield return new WaitForSeconds(1f / _speed);
                        }
                        visibleCounter = 0;
                    }
                    subCounter++;
                }

                if (OnReadCompleted != null)
                {
                    yield return new WaitForSeconds(1);
                    OnReadCompleted();
                }
                
                yield return null;

                WaitForSeconds EvaluateTag(string tag)
                {
                    if (tag.Length > 0)
                    {
                        if (tag.StartsWith("speed="))
                        {
                            _speed = float.Parse(tag.Split('=')[1]);
                        }
                        else if (tag.StartsWith("pause="))
                        {
                            return new WaitForSeconds(float.Parse(tag.Split('=')[1]));
                        }
                    }
                    return null;
                }
            }
        }
    }
}
