using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace GameJamCat
{
    public class PregameDialogueBoxBehaviour : MonoBehaviour
    {
        private const float DelayBeforeShowingDialogueBox = 1f;
        private const float ScaleAnimationDuration = 1f;
        private const float DurationOfReadTimeAfterAnimationCompletion = 3f;
        private const string PregameMessage =
            "... Thanks again detective for helping us. We couldnt find the cat all day, and theres only two minutes left until the adoption... Please help us find it!";
        
        public event Action OnDialogueCompleted;
        
        [SerializeField] private DialogueBoxBehaviour _dialogueBoxBehaviour = null;

        private Transform _dialogueBehaviourContainer = null;

        public void StartAnimation()
        {
            // Technically not needed, but OnEnable on DialogueBoxBehaviour already does a tween
            _dialogueBehaviourContainer.DOScale(Vector3.one, ScaleAnimationDuration).SetEase(Ease.OutBack).SetDelay(DelayBeforeShowingDialogueBox).OnComplete(() =>
            {
                _dialogueBoxBehaviour.ReadText(PregameMessage);
            });
        }

        public void Initialize()
        {
            if (_dialogueBoxBehaviour != null)
            {

                _dialogueBehaviourContainer = _dialogueBoxBehaviour.transform.parent;
#if UNITY_EDITOR
                _dialogueBehaviourContainer.gameObject.SetActive(false);
#endif
                _dialogueBehaviourContainer.localScale = Vector3.zero;
            }
            
            _dialogueBoxBehaviour.text = string.Empty;
        }

        private void HandleOnReadComplete()
        {
            StartCoroutine(WaitForSecondsBeforeFadingIn(DurationOfReadTimeAfterAnimationCompletion));
        }

        private void Awake()
        {
            if (_dialogueBoxBehaviour != null)
            {
                _dialogueBoxBehaviour.OnReadCompleted += HandleOnReadComplete;
            }
        }

        private void OnDestroy()
        {
            if (_dialogueBoxBehaviour != null)
            {
                _dialogueBoxBehaviour.OnReadCompleted -= HandleOnReadComplete;
            }
        }

        private IEnumerator WaitForSecondsBeforeFadingIn(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            _dialogueBehaviourContainer.DOScale(Vector3.zero, ScaleAnimationDuration).SetEase(Ease.OutQuint).OnComplete(() =>
            {
                if (OnDialogueCompleted != null)
                {
                    OnDialogueCompleted();
                }
            });
        }
    }
}
