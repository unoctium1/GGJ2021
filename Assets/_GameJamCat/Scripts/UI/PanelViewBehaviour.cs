using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GameJamCat
{
    public class PanelViewBehaviour : MonoBehaviour
    {
        private const float AnimationPawDistance = 1000;

        [Title("Components")]
        [SerializeField] private GameObject _certificate = null;
        [SerializeField] private Button _winReplayButton = null;
        
        [Title("Properties")]
        [SerializeField] private float _animationDuration = 1.5f;
        [SerializeField] private float _animationCertificateDuration = 1.0f;
        
        public void Initialize()
        {
            var rectTransform = GetComponent<RectTransform>();
            //Top
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -AnimationPawDistance);
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, -AnimationPawDistance);
        }

        /// <summary>
        /// Starts the Win Game Scenario
        /// </summary>
        public void StartWinAnimation()
        {
            if (_certificate != null)
            {
                _certificate.transform.localScale = Vector3.zero;
            }
            
            transform.DOBlendableLocalMoveBy(Vector3.up * AnimationPawDistance, _animationDuration, true)
                .SetEase(Ease.OutQuint).OnComplete( () => 
            {
                if (_certificate != null)
                {
                    _certificate.transform.DOScale(Vector3.one, _animationCertificateDuration).SetEase(Ease.OutBack);
                }
            });
        }

        private void OnEnable()
        {
            _winReplayButton.onClick.RemoveAllListeners();
            _winReplayButton.onClick.AddListener(ResetGame);
        }
        
        private void ResetGame()
        {
            //Add Reset Logic Here
        }
    }
}
