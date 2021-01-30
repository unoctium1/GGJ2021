using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace GameJamCat
{
    public class EndGameMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _loseScreen = null;
        [SerializeField] private WinPanelViewBehaviour _winScreen = null;
        [SerializeField] private Button _loseReplayButton = null;

        public void Initialize()
        {
            if (_loseScreen != null)
            {
                _loseScreen.SetActive(false);
            }

            if (_winScreen != null)
            {
                _winScreen.gameObject.SetActive(false);
            }

            if (_loseReplayButton != null)
            {
                _loseReplayButton.onClick.AddListener(() => ResetGame());
            }
        }
        
        [Button]
        private void WinGame()
        {
            DisplayEndPanel(true);
        }
        
        [Button]
        private void LoseGame()
        {
            DisplayEndPanel(false);
        }
        
        public void DisplayEndPanel(bool foundCat)
        { 
            if (foundCat)
            {
                if (_winScreen != null)
                {
                    _winScreen.gameObject.SetActive(true);
                    _winScreen.Initialize();
                    _winScreen.StartWinAnimation();
                }
            }
            else
            {
                if (_loseScreen != null)
                {
                    _loseScreen.SetActive(true);
                }
            }
        }



        private void ResetGame()
        {
            //Add Reset Logic Here
        }
    }
}
