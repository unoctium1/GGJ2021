using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static GameJamCat.Utilities;

namespace GameJamCat
{
    /// <summary>
    /// Gets a camera set up for generating render textures
    /// </summary>
    public class RTCameraManager : MonoBehaviour
    {
        const int CameraWidth = 256;
        const int CameraHeight = 256;
        const int CameraDepth = 8;
        [SerializeField] private Camera cam = null;

        public event Action<Texture> OnTextureGenerated;

        // I know this isn't great for singletons lol, but should be okay for this purpose
        public static RTCameraManager Instance {
            get
            {
                _instance ??= FindObjectOfType<RTCameraManager>();
                return _instance;
            }
        }

        private static RTCameraManager _instance;
        private bool isCapturing = false;

        public void SetupCameraLocation(Transform t)
        {
            cam.transform.parent = t;
            cam.transform.localPosition = Vector3.zero;
            cam.transform.localRotation = Quaternion.identity;
        }

        public void TakeCapture()
        {
            cam.targetTexture = RenderTexture.GetTemporary(CameraWidth, CameraHeight, CameraDepth);
            isCapturing = true;
        }

        private void OnEnable()
        {
            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
        }

        private void OnDisable()
        {
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        }

        #region delegates
        private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            if (isCapturing)
            {
                isCapturing = false;
                var tex = cam.targetTexture;
                var output = tex.ToTexture2D();

                if (OnTextureGenerated != null)
                {
                    OnTextureGenerated(output);
                }
            }
        }
        #endregion
    }
}
