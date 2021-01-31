using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

namespace GameJamCat
{
    public class CatManager : MonoBehaviour
    {
        
        [Title("Managers")]
        [SerializeField] private CatFactory _catGenerator = null;
        [SerializeField] private SpawnArea[] _spawnArea = null;

        [Title("Properties")] 
        [SerializeField] private int _catsToSpawn = 1;
        [SerializeField, MinMaxSlider(0.5f, 3.0f)]
        private Vector2 _catScaleRange = new Vector2(0.5f, 0.5f);

        private List<CatBehaviour> _activeCats;
        private CatBehaviour _chosenCatToFind = null;

        public event Action<CatBehaviour> OnGeneratedSelectedCatToFind;
        
        /// <summary>
        /// Initializes the CatManager
        /// </summary>
        public void Initialize()
        {
            _activeCats = new List<CatBehaviour>();
            SpawnCats();
            StoreRandomCatToFind();
        }

        /// <summary>
        /// Clean Up CatManager, unsubscribe or remove values 
        /// </summary>
        public void CleanUp()
        {
            foreach(var cat in _activeCats)
            {
                _catGenerator.DestroyCat(cat);
            }
            _activeCats.Clear();
            _chosenCatToFind = null;
        }

        private void StoreRandomCatToFind()
        {
            if (_activeCats.IsNullOrEmpty())
            {
                return;
            }

            _chosenCatToFind = Utilities.GetRandom(_activeCats);
            
            if (OnGeneratedSelectedCatToFind != null)
            {
                OnGeneratedSelectedCatToFind(_chosenCatToFind);
            }
        }

        private CatBehaviour GetRandomCat()
        {
            CatBehaviour cat = _catGenerator.GetRandomCat();
            var spawnArea = Utilities.GetRandom(_spawnArea);
            var position = spawnArea.GetRandomUnitWithSphere();
            var scale = Utilities.GetRandom(_catScaleRange);
            cat.Initialize(position, new Vector3(scale, scale, scale));
            _activeCats.Add(cat);
            return cat;
        }

        private void SpawnCats()
        {
            for (int i = 0; i < _catsToSpawn; i++)
            {
                GetRandomCat();
            }
        }


    }

    public interface ICatFactory
    {
        CatBehaviour GetRandomCat();

        void DestroyCat(CatBehaviour cat);
    }
}
