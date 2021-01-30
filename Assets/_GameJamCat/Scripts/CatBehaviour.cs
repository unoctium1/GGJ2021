using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJamCat
{
    public class CatBehaviour : MonoBehaviour
    {
        [SerializeField] private Renderer _catRenderer;

        private ICatFactory _factory = null;

        public ICatFactory Factory
        {
            get => _factory;
            set
            {
                if (_factory == null) _factory = value;
                else
                {
                    Debug.LogError("Shouldn't reassign cat factory");
                }
            }
        }


        public Renderer CatRenderer { get => _catRenderer; }

        /// <summary>
        /// Recycle Cat
        /// </summary>
        public void DestroyCat()
        {
            Factory.Reclaim(this);
        }

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
