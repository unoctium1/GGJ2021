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
        private Vector3 _viewportCenter = new Vector3(0.5f, 0.5f, 0);

        public event Action OnEndConversation;
        
        private CharacterController characterController { get; set; }
        private PlayerCharacter playerCharacter { get; set; }
        
        protected void Awake()
        {
            characterController = GetComponent<CharacterController>();
            playerCharacter = GetComponent<PlayerCharacter>();
            _mainCamera = transform.parent.GetComponentInChildren<Camera>();
        }

        protected void Update()
        {
            // THIS IS TEMPORARY: EXITS FROM FOCUS MODE
            // end conversation should be called from a UI button once we have the text options
            if (_stateManager.GetState() == State.Dialogue && _cameraAnimationInProgress == true)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (_currentCatInFocus == null)
                    {
                        return;
                    }
                    _currentCatInFocus.EndConversation();
                    _stateManager.SetState(State.Play);

                    if (OnEndConversation != null)
                    {
                        OnEndConversation();
                    }
                }
            }

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
                    // Fire other event here that could highlite the cross hair 
                    // Thas UX babey 
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        _currentCatInFocus = hit.collider.GetComponent<CatBehaviour>();
                        _currentCatInFocus.BeginConversation();
                        _cameraAnimationInProgress = true;
                    }
                }
            }
        }

        /// <summary>
        /// Event calls this from the timeline. Ensures we stay focused on the 'current' cat
        /// </summary>
        public void PauseTimelineForCat()
        {
            _currentCatInFocus.StopTimeline();
            _stateManager.SetState(State.Dialogue);
        }
        
        /// <summary>
        /// Stops spam of the camera 
        /// </summary>
        public void CameraAnimationEnded()
        {
            _cameraAnimationInProgress = false;
        }
    }
}


