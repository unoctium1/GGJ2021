using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GameJamCat
{
    public class LivesViewBehaviour : MonoBehaviour
    {
        [SerializeField] private Sprite _twoLivesLeft = null;
        [SerializeField] private Sprite _oneLifeLeft = null;
        [SerializeField] private Sprite _noLivesLeft = null;
        private Image _liveImage = null;
        
        /// <summary>
        /// Sets the Lives UI
        /// </summary>
        /// <param name="lives">number of lives</param>
        [Button]
        public void SetLiveImage(int lives)
        {
            Sprite lifeSprite = null;
            if (lives == 2)
            {
                lifeSprite = _twoLivesLeft;
            }

            if (lives == 1)
            {
                lifeSprite = _oneLifeLeft;
            }

            if (lives == 0)
            {
                lifeSprite = _noLivesLeft;
            }

            if (_liveImage != null)
            {
                _liveImage.transform.DOPunchScale(Vector3.one, 0.5f, 1, 1);
                _liveImage.sprite = lifeSprite;
            }
        }

        private void Awake()
        {
            _liveImage = GetComponentInChildren<Image>();
        }
    }
}
