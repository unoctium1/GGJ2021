using Sirenix.OdinInspector;
using UnityEngine;

namespace GameJamCat
{
    public class WorldManager : MonoBehaviour
    {
        private readonly IStateManager _stateManager = StateManager.Instance;
        
        [Title("Properties")]
        [SerializeField]
        private int _lives = 3;

        [Title("Managers")] 
        [SerializeField] private PlayerController _playerController = null;
        [SerializeField] private CatManager _catManager = null;
        [SerializeField] private TimeManager _timeManager = null;
        [SerializeField] private UIManager _uiManager = null;

        public int Lives
        {
            get => _lives;
            set
            {
                _lives = value;
                _uiManager.SetLives(value);
            }
        }
        
        private void OnEnable()
        {
            if (_catManager != null)
            {
                _catManager.Initialize();
                _catManager.OnGeneratedSelectedCatToFind += HandleOnGeneratedSelectedCatToFind;
            }

            if (_uiManager != null)
            {
                _uiManager.Initialize(Lives);
            }

            if (_timeManager != null)
            {
                _timeManager.Initialize();
            }
        }

        private void Start()
        {
            _stateManager.SetState(State.Pregame);
        }

        private void OnDisable()
        {
            if (_catManager != null)
            {
                _catManager.CleanUp();
                _catManager.OnGeneratedSelectedCatToFind -= HandleOnGeneratedSelectedCatToFind;
            }

            if (_uiManager != null)
            {
                _uiManager.CleanUp();
            }

            if (_timeManager != null)
            {
                _timeManager.CleanUp();
            }
        }

        #region delegate
        private void HandleOnGeneratedSelectedCatToFind(CatBehaviour cat)
        {
            // TODO - update dossier
        }
        #endregion
    }
}
