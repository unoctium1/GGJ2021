using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJamCat
{
    public class CatBehaviour : MonoBehaviour
    {

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
        }

        private void Update()
        {

        }
    }
}
