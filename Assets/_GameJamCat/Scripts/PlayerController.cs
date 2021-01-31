using UnityEngine;
using Gameplay;

namespace GameJamCat {

    [SelectionBase]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private bool _activateControllerDebug = false;

        private readonly IStateManager _stateManager = StateManager.Instance;
        
        public CharacterController characterController { get; private set; }
        public PlayerCharacter playerCharacter { get; private set; }

        public FirstPersonCamera playerCamera;
        private Camera _mainCamera;
        private CatBehaviour _currentCatInFocus = null;
        private const float _maxReticleDistance = 5f;

        private bool _cameraAnimationInProgress = false;

        protected void Awake()
        {
            characterController = GetComponent<CharacterController>();
            playerCharacter = GetComponent<PlayerCharacter>();
            _mainCamera = this.transform.parent.GetComponentInChildren<Camera>();
        }

        protected void Update()
        {
            // THIS IS TEMPORARY: EXITS FROM FOCUS MODE
            // end conversation should be called from a UI button once we have the text options
            if (_stateManager.GetState() == State.Dialogue && _cameraAnimationInProgress == true)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (_currentCatInFocus == null) return;
                    _currentCatInFocus.EndConversation();
                    _stateManager.SetState(State.Play);
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
            Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _maxReticleDistance))
            {
                if (hit.collider.gameObject.CompareTag("Cat"))
                {
                    // Fire other event here that could highlite the cross hair 
                    // Thas UX babey 
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        _currentCatInFocus = hit.collider.gameObject.GetComponent<CatBehaviour>();
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


