using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameJamCat
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton = null;
        [SerializeField] private Button _quitGameButton = null;

        private void Awake()
        {
            if (_startGameButton != null)
            {
                _startGameButton.onClick.RemoveAllListeners();
                _startGameButton.onClick.AddListener(() => SceneManager.LoadScene("MainScene"));
            }

            if (_quitGameButton != null)
            {
                _quitGameButton.onClick.RemoveAllListeners();
                _quitGameButton.onClick.AddListener(() => Application.Quit());
            }
        }
    }
}
