using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace GameJamCat
{
    public class EndGameMenu : MonoBehaviour
    {
        [SerializeField] private PanelViewBehaviour _loseScreen = null;
        [SerializeField] private PanelViewBehaviour _winScreen = null;

        public void Initialize()
        {
            if (_loseScreen != null)
            {
                _loseScreen.gameObject.SetActive(false);
            }

            if (_winScreen != null)
            {
                _winScreen.gameObject.SetActive(false);
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
            PanelViewBehaviour panel = foundCat ? _winScreen : _loseScreen;
            if (panel != null)
            {
                panel.gameObject.SetActive(true);
                panel.Initialize();
                panel.StartWinAnimation();
            }
        }
    }
}
