using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJamCat
{
    /// <summary>
    /// This serves as a runtime replacement for the CSV
    /// </summary>
    [CreateAssetMenu]
    public class DialogueOptions : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField, Tooltip("Note that this doesn't do anything on its own - you have to load the csv to make changes to the list")] private string _pathToCSV;
#endif
        public List<CatCustomisation> _catCustomizationOptions;

        [System.NonSerialized]
        private List<CatCustomisation> _pool;

        public bool GetRandomCat(out CatCustomisation cat)
        {
            _pool ??= new List<CatCustomisation>(_catCustomizationOptions);
            if (_pool.Count == 0)
            {
                // Deal with this properly later
                cat = CatCustomisation.EmptyCat();
                return false;
            }
            int index = Random.Range(0, _pool.Count);
            cat = _pool[index];
            _pool.RemoveAt(index);
            return true;
        }

        public void RecycleCat(CatCustomisation cat)
        {
            _pool ??= new List<CatCustomisation>(_catCustomizationOptions);
            _pool.Add(cat);
        }

    }

    [System.Serializable]
    public struct CatCustomisation
    {
        public string _catName;
        public string _catNameAnswer;
        public string _catFoodAnswer;
        public string _catActivityAnswer;
        public Food _food;
        public Toy _toy;
        public string _flavourText;

        public static CatCustomisation EmptyCat() {
            return new CatCustomisation
            {
                _catName = "???",
                _catNameAnswer = "Meow meow meow, meow meow. Meow.",
                _catActivityAnswer = "Moew meow. Meow meow MEOW!! Meow meow meow.",
                _catFoodAnswer = "Meow",
                _food = Food.none,
                _toy = Toy.none,
            };
        }
    }

    public enum Food
    {
        Tuna,
        Salmon,
        Chicken,
        Pumpkin,
        DryKibble,
        WetKibble,
        Kibble, //Use this for both?
        Bones,
        Beef,
        none
    } 

    public enum Toy
    {
        YarnBalls,
        Hijinks,
        CardboardBox,
        FishingRod,
        CatnipSack,
        ScratchingPost,
        TennisBall,
        Laser,
        none
    }
}
