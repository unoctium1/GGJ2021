using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace GameJamCat
{
    public class EndGameMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _loseScreen = null;
        [SerializeField] private GameObject _winScreen = null;
        [SerializeField] private Button _loseReplayButton = null;
        [SerializeField] private Button _winReplayButton = null;

        private bool _foundCat = false; // TempCondition until manager has information
        
        [Button]
        private void WinGame()
        {
            _foundCat = true;
            StateManager.Instance.SetState(State.EndGame);
        }
        
        [Button]
        private void LoseGame()
        {
            _foundCat = false;
            StateManager.Instance.SetState(State.EndGame);
        }

        private void Awake()
        {
            if (_loseScreen != null)
            {
                _loseScreen.SetActive(false);
            }

            if (_winScreen != null)
            {
                _winScreen.SetActive(false);
            }

            StateManager.Instance.OnStateChanged += DisplayEndPanel;
            _loseReplayButton.onClick.AddListener(() => ResetGame());
            _winReplayButton.onClick.AddListener(() => ResetGame());
        }

        private void OnDestroy()
        {
            StateManager.Instance.OnStateChanged -= DisplayEndPanel;
        }

        private void DisplayEndPanel(State state)
        {
            if (state == State.EndGame)
            {
                if (_foundCat)
                {
                    if (_winScreen != null)
                    {
                        _winScreen.SetActive(true);
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
        }

        private void ResetGame()
        {
            //Add Reset Logic Here
        }
    }
}
