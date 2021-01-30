using UnityEngine;
using System.Collections.Generic;

namespace GameJamCat
{
    public class CatManager : MonoBehaviour
    {
        [SerializeField]
        private CatFactory catGenerator;

        private List<CatBehaviour> _activeCats;

        /// <summary>
        /// Initializes the CatManager
        /// </summary>
        public void Initialize()
        {
            _activeCats = new List<CatBehaviour>();
        }

        /// <summary>
        /// Clean Up CatManager, unsubscribe or remove values 
        /// </summary>
        public void CleanUp()
        {
            foreach(var cat in _activeCats)
            {
                catGenerator.DestroyCat(cat);
            }
            _activeCats.Clear();
        }

        public CatBehaviour GetRandomCat()
        {
            CatBehaviour cat = catGenerator.GetRandomCat();
            _activeCats.Add(cat);
            cat.Initialize();
            return cat;
        }
    }

    public interface ICatFactory
    {
        CatBehaviour GetRandomCat();

        void DestroyCat(CatBehaviour cat);
    }
}
