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
        [SerializeField] private UIManager _uiManager = null;
        
        private float _currentTime = 120f;

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
                _catManager.OnGeneratedSelectedCatToFind += HandleOnGeneratedSelectedCatToFind;
                _catManager.Initialize();
            }

            if (_uiManager != null)
            {
                _uiManager.Initialize(Lives);
            }
        }

        private void Start()
        {
            _stateManager.SetState(State.Pregame);
        }

        private void Update()
        {
            State currentState = _stateManager.GetState();
            if (currentState != State.Dialogue && currentState != State.Play)
            {
                return;
            }

            if (HasTimeRunOut())
            {
                _stateManager.SetState(State.EndGame);
                return;
            }

            _currentTime -= Time.deltaTime;
            if (_uiManager != null)
            {
                _uiManager.UpdateTimer(_currentTime);
            }
        }

        private bool HasTimeRunOut()
        {
            return _currentTime <= 0;
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
        }

        #region delegate
        private void HandleOnGeneratedSelectedCatToFind(CatBehaviour cat)
        {
            _uiManager.SetUpDossier(cat);
        }
        #endregion
    }
}
