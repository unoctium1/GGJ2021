using System;
using UnityEngine;
using Gameplay;

namespace GameJamCat {

    [SelectionBase]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        private const float _maxReticleDistance = 8f;
        private const string CatConstant = "Cat";
        
        private readonly IStateManager _stateManager = StateManager.Instance;

        [SerializeField]
        private bool _activateControllerDebug = false;
        public FirstPersonCamera playerCamera;
        private Camera _mainCamera = null;
        private CatBehaviour _currentCatInFocus = null;
        private bool _cameraAnimationInProgress = false;
        private bool _lookingAtCat = false;
        private Vector3 _viewportCenter = new Vector3(0.5f, 0.5f, 0);

        public event Action OnEndConversation;
        public event Action LookingAtCat;
        public event Action NotLookingAtCat;
        public event Action<CatBehaviour> OnSelectCat;

        private CharacterController characterController { get; set; }
        private PlayerCharacter playerCharacter { get; set; }
        
        protected void Awake()
        {
            characterController = GetComponent<CharacterController>();
            playerCharacter = GetComponent<PlayerCharacter>();
            _mainCamera = transform.parent.GetComponentInChildren<Camera>();
            _stateManager.OnStateChanged += HandleStateChange;
        }


        protected void Update()
        {

            if (_stateManager.GetState() != State.Play && _activateControllerDebug == false)
            {
                return;
            }
            
            PlayerInput input;
            PlayerInput.Update(out input);

            if (input.move.y < 0f)
                input.look.y = -input.look.y;

            if (playerCharacter)
                playerCharacter.Simulate(characterController, input);
            if (_cameraAnimationInProgress == false)
                FocusObjectUpdate();
            if (_lookingAtCat == true && _currentCatInFocus != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _currentCatInFocus.ActivatePet();
                }
            }
        }

        private void OnDisable()
        {
            _stateManager.OnStateChanged -= HandleStateChange;
        }

        protected void LateUpdate()
        {
            var firstPersonCamera = playerCamera as FirstPersonCamera;

            if (firstPersonCamera && playerCharacter)
            {
                float pitch, yaw;
                playerCharacter.GetLookPitchAndYaw(out pitch, out yaw);
                firstPersonCamera.pitch = pitch;
                firstPersonCamera.yaw = yaw;
            }

            var transform = this.transform;
            var position = transform.localPosition;
            var rotation = transform.localEulerAngles;
            float deltaTime = Time.deltaTime;

            playerCamera.Simulate(position, rotation, deltaTime);
        }

        private void FocusObjectUpdate()
        {
            
            Ray ray = _mainCamera.ViewportPointToRay(_viewportCenter);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _maxReticleDistance))
            {
                if (hit.collider.CompareTag(CatConstant))
                {
                    if (_lookingAtCat == false)
                    {
                        LookingAtCat();
                        _lookingAtCat = true;
                        _currentCatInFocus = hit.collider.GetComponent<CatBehaviour>();
                    }
                    // Fire other event here that could highlite the cross hair 
                    // Thas UX babey 
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        SelectCat();
                       
                    }
                }
                else
                {
                    NotLookingAtCat();
                    _lookingAtCat = false;
                    _currentCatInFocus = null;
                }
            }
        }

        private void ReturnCameraToFocus()
        {
            if (_currentCatInFocus == null && !_cameraAnimationInProgress)
            {
                return;
            }
            _currentCatInFocus.EndConversation();

            if (OnEndConversation != null)
            {
                OnEndConversation();
            }
        }

        private void UpdateCursor(State state)
        {
            bool isInPlayState = state == State.Play;
            Cursor.lockState = isInPlayState ? CursorLockMode.Locked : CursorLockMode.Confined;
            Cursor.visible = !isInPlayState;
        }

        /// <summary>
        /// Event calls this from the timeline. Ensures we stay focused on the 'current' cat
        /// </summary>
        public void PauseTimelineForCat()
        {
            _currentCatInFocus.StopTimeline();
        }

        /// <summary>
        /// Stops spam of the camera 
        /// </summary>
        public void CameraAnimationEnded()
        {
            _cameraAnimationInProgress = false;
        }

        public void SelectCat()
        {
            if (_currentCatInFocus != null)
            {
                OnSelectCat(_currentCatInFocus);
                _currentCatInFocus.BeginConversation();
                _cameraAnimationInProgress = true;
            }
        }

        /*
        public void FavFoodDialogue()
        {
            if (_currentCatInFocus != null)
            {
                if (_currentCatInFocus.CatDialogue._catFoodAnswer != null)
                {
                    _dialogueBoxBehaviour.Show();
                    _dialogueBoxBehaviour.ReadText(_currentCatInFocus.CatDialogue._catFoodAnswer);
                }
            }
        }
        */

        /*
        private void DisableOptionsBox()
        {
            _dialogueOptions.SetActive(false);
        }
        */

        /*
        public void CatNameDialogue()
        {
            if (_currentCatInFocus != null)
            {
                if (_currentCatInFocus.CatDialogue._catNameAnswer != null)
                {
                    _dialogueBoxBehaviour.Show();
                    _dialogueBoxBehaviour.ReadText(_currentCatInFocus.CatDialogue._catNameAnswer);
                }
            }
        }
        */

        /*
        public void CatToyDialogue()
        {
            if (_currentCatInFocus != null)
            {
                if (_currentCatInFocus.CatDialogue._catActivityAnswer != null)
                {
                    _dialogueBoxBehaviour.Show();
                    _dialogueBoxBehaviour.ReadText(_currentCatInFocus.CatDialogue._catActivityAnswer);
                }
            }
        }
        */

        /*
        public void SetCatNameInDialogue()
        {
            if (_currentCatInFocus != null)
            {
                if (_currentCatInFocus.CatDialogue._catName != null)
                {
                    SetDialogueBoxName boxname = _dialogueBoxBehaviour.GetComponent<SetDialogueBoxName>();
                    boxname.SetCatNameInDialogueBox(_currentCatInFocus.CatDialogue._catName);
                }
                
            }
        }
        */

        private void HandleStateChange(State state)
        {
            UpdateCursor(state);
            switch (state)
            {
                case State.Pregame:
                    break;
                case State.Play:
                    ReturnCameraToFocus();
                    break;
                case State.Dialogue:
                    break;
                case State.EndGame:
                    break;
            }
        }

    }
}


