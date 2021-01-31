using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace GameJamCat
{
    public class LivesViewBehaviour : MonoBehaviour
    {
        [SerializeField] private Sprite _twoLivesLeft = null;
        [SerializeField] private Sprite _oneLifeLeft = null;
        [SerializeField] private Sprite _noLivesLeft = null;
        [SerializeField] private Image _liveImage = null;
        
        /// <summary>
        /// Sets the Lives UI
        /// </summary>
        /// <param name="lives">number of lives</param>
        [Button]
        public void SetLiveImage(int lives)
        {
            Sprite lifeSprite = null;

            switch (lives)
            {
                case 2:
                    lifeSprite = _twoLivesLeft;
                    break;
                case 1:
                    lifeSprite = _oneLifeLeft;
                    break;
                case 0:
                    lifeSprite = _noLivesLeft;
                    break;
            }

            if (_liveImage != null)
            {
                if (lifeSprite != null)
                {
                    _liveImage.transform.localScale = Vector3.one;
                    _liveImage.transform.DOPunchScale(Vector3.one, 0.5f, 1);
                    _liveImage.sprite = lifeSprite;
                }
                else
                {
                    _liveImage.transform.localScale = Vector3.zero;
                }
            }
        }
    }
}
