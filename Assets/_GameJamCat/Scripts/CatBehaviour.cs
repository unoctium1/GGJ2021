using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace GameJamCat
{
    public class CatBehaviour : MonoBehaviour
    {
        private const string DialogueConstant = "Dialogue";
        private const string PlayerConstant = "Player";
        [SerializeField] private Renderer _catRenderer;
        private PlayableDirector _playableDirector;
        private CinemachineVirtualCamera _playerVirtualCamera;

        public Renderer CatRenderer { get; private set; }

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
    }
}
