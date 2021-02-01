using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;


namespace GameJamCat
{
    public class CatBehaviour : MonoBehaviour
    {

        [SerializeField]
        private CatCustomisation _catDialogue;
        private const string DialogueConstant = "Dialogue";
        private const string PlayerConstant = "Player";
        private PlayableDirector _playableDirector;
        private ParticleSystem _particles;
        private CinemachineVirtualCamera _playerVirtualCamera;
        [SerializeField] private Transform[] _renderTextureCamLocations;
        [SerializeField] private float _petDuration = 0.5f;
        [SerializeField] private AudioSource _source;

        public Renderer CatRenderer { get; private set; }

        public CatCustomisation CatDialogue { get => _catDialogue; set => _catDialogue = value; }

        public Texture2D CatScreenshot { get; private set; } = null;

        /// <summary>
        /// Called by the CatManager when a cat is grabbed from the pool
        /// </summary>
        public void Initialize(Vector3 spawnPosition)
        {
            transform.position = spawnPosition;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Called by the CatManager when a cat is grabbed from the pool
        /// </summary>
        public void Initialize(Vector3 spawnPosition, Vector3 scale)
        {
            transform.position = spawnPosition;
            transform.localScale = scale;
            gameObject.SetActive(true);
        }

        private void Awake()
        {
            CatRenderer = GetComponentInChildren<Renderer>();
            _playableDirector = GetComponent<PlayableDirector>();
            _particles = GetComponentInChildren<ParticleSystem>();
            if (_source != null)
            {
                _source.pitch = Random.Range(-3f, 3f);
                _source.volume = Random.Range(0.5f, 1f);
            }
        }

        private void Start()
        {
            SetupVirtualCams();
        }

        /// <summary>
        /// Fetches main cam to get cinemachine brain and player cam so we can transition from player cam to cat cam
        /// </summary>
        private void SetupVirtualCams()
        {
            foreach (var output in _playableDirector.playableAsset.outputs)
            {
                if (output.streamName == DialogueConstant)
                {
                    CinemachineBrain cinemachine = Camera.main.GetComponent<CinemachineBrain>();
                    CinemachineVirtualCamera playerVirtualCamera = cinemachine.transform.parent.GetComponentInChildren<CinemachineVirtualCamera>();
                    _playableDirector.SetGenericBinding(output.sourceObject, Camera.main.GetComponent<CinemachineBrain>());
                    var cinemachineTrack = output.sourceObject as CinemachineTrack;
                    foreach (var clip in cinemachineTrack.GetClips())
                    {
                        if (clip.displayName == PlayerConstant)
                        {
                            var cinemachineShot = clip.asset as CinemachineShot;
                            _playableDirector.SetReferenceValue(cinemachineShot.VirtualCamera.exposedName, playerVirtualCamera);
                        }
                    }
                }
            }
        }

        public void ActivatePet()
        {
            StartCoroutine(StartParticles());
        }

        private IEnumerator StartParticles()
        {
            _particles.Play();
            yield return new WaitForSeconds(_petDuration);
            _particles.Stop();
        }

        /// <summary>
        /// Begins player Conversation With Cat
        /// </summary>
        public void BeginConversation()
        {
            _playableDirector.Play();
        }

        /// <summary>
        /// Ends conversation with the cat
        /// </summary>
        public void EndConversation()
        {
            _playableDirector.Resume();
        }

        /// <summary>
        /// Stops camera animation so we stay focused on the cat
        /// </summary>
        public void StopTimeline()
        {
            _playableDirector.Pause();
        }

        public Transform GetCameraLocation()
        {
            return Utilities.GetRandom(_renderTextureCamLocations);
        }

    }
}
