using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJamCat
{
    public class CatBehaviour : MonoBehaviour
    {
        [SerializeField] private Renderer _catRenderer;


        public Renderer CatRenderer => _catRenderer;


        /// <summary>
        /// Called by the CatManager when a cat is grabbed from the pool
        /// </summary>
        public void Initialize()
        {
            gameObject.SetActive(true);
        }

        private void Start()
        {
            
        }

        private void Update()
        {

        }
    }
}
