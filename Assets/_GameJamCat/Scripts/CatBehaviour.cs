using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace GameJamCat
{
    public class CatBehaviour : MonoBehaviour
    {
        [SerializeField] private Renderer _catRenderer;
        private PlayableDirector playableDirector;
        private CinemachineVirtualCamera playerVirtualCamera;

        public Renderer CatRenderer { get; private set; }

        /// <summary>
        /// Called by the CatManager when a cat is grabbed from the pool
        /// </summary>
        public void Initialize(Vector3 spawnPosition)
        {
            playableDirector = GetComponent<PlayableDirector>();
            transform.position = spawnPosition;
            SetupVirtualCams();
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
        }

        private void Update()
        {

        }

        /// <summary>
        /// Fetches main cam to get cinemachine brain and player cam so we can transition from player cam to cat cam
        /// </summary>
        private void SetupVirtualCams()
        {
            foreach (var output in playableDirector.playableAsset.outputs)
            {
                if (output.streamName == "Dialouge")
                {
                    CinemachineBrain cinemachine = Camera.main.GetComponent<CinemachineBrain>();
                    CinemachineVirtualCamera playerVirtualCamera = cinemachine.transform.parent.GetComponentInChildren<CinemachineVirtualCamera>();
                    playableDirector.SetGenericBinding(output.sourceObject, Camera.main.GetComponent<CinemachineBrain>());
                    var cinemachineTrack = output.sourceObject as CinemachineTrack;
                    foreach (var clip in cinemachineTrack.GetClips())
                    {
                        if (clip.displayName == "Player")
                        {
                            var cinemachineShot = clip.asset as CinemachineShot;
                            playableDirector.SetReferenceValue(cinemachineShot.VirtualCamera.exposedName, playerVirtualCamera);
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
            playableDirector.Play();
        }

        /// <summary>
        /// Ends conversation with the cat
        /// </summary>
        public void EndConversation()
        {
            playableDirector.Resume();
        }

        /// <summary>
        /// Stops camera animation so we stay on the cat
        /// </summary>
        public void StopTimeline()
        {
            Debug.Log("=========Pause+======");
            playableDirector.Pause();
        }
    }
}
