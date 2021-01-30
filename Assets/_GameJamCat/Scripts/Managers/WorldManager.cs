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
        
        private void OnEnable()
        {
            _catManager.Initialize();
            _uiManager.Initialize();
            _timeManager.Initialize();
        }

        private void Start()
        {
            _stateManager.SetState(State.Pregame);
        }

        private void OnDisable()
        {
            _catManager.CleanUp();
            _uiManager.CleanUp();
            _timeManager.CleanUp();
        }
    }
}
