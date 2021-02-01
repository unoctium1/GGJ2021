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
        
        private float _currentTime = 300f;

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
                _catManager.OnGeneratedCatTexture += HandleOnGeneratedCatTexture;
                _catManager.Initialize();
            }

            if (_uiManager != null)
            {
                _uiManager.Initialize(Lives);
            }

            if (_playerController != null)
            {
                _playerController.OnEndConversation += HandleOnEndConversation;
                _playerController.LookingAtCat += HandleShowCrossHairText;
                _playerController.NotLookingAtCat += HandleHideCrossHairText;
                _playerController.OnTalkToCat += HandleOnTalkToCat;
                _playerController.OnClaimCat += HandleOnClaimCat;
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
                _catManager.OnGeneratedCatTexture -= HandleOnGeneratedCatTexture;
            }

            if (_uiManager != null)
            {
                _uiManager.CleanUp();
            }
            if (_playerController != null)
            {
                _playerController.OnEndConversation -= HandleOnEndConversation;
                _playerController.LookingAtCat -= HandleShowCrossHairText;
                _playerController.NotLookingAtCat -= HandleHideCrossHairText;
                _playerController.OnTalkToCat -= HandleOnTalkToCat;
                _playerController.OnClaimCat -= HandleOnClaimCat;
            }
        }

        #region delegate
        private void HandleOnClaimCat(CatBehaviour cat)
        {
            if (_catManager.ClaimCat(cat))
            {
                _uiManager.ClaimedCat = true;
                StateManager.Instance.SetState(State.EndGame);
            }
            else
            {
                Lives--;
                if(StateManager.Instance.GetState() == State.Dialogue)
                {
                    StateManager.Instance.SetState(State.Play);
                }
            }
        }

        private void HandleOnTalkToCat(CatBehaviour cat)
        {
            _uiManager.SetTalkingToCat(cat);
        }

        private void HandleOnGeneratedSelectedCatToFind(CatBehaviour cat)
        {
            _uiManager.SetUpDossier(cat);
        }

        private void HandleOnGeneratedCatTexture(Texture tex)
        {
            _uiManager.SetUpDossierTexture(tex);
        }

        private void HandleOnEndConversation()
        {
            _uiManager.SetCrossHairState(true);
        }

        private void HandleShowCrossHairText()
        {
            _uiManager.SetCrossHairTextState(true);
        }

        private void HandleHideCrossHairText()
        {
            _uiManager.SetCrossHairTextState(false);
        } 
        #endregion
    }
}
