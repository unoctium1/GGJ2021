using UnityEngine;

namespace GameJamCat
{
    public class CatManager : MonoBehaviour
    {
        private ICatFactory _catFactory = null;
        
        /// <summary>
        /// Initializes the CatManager
        /// </summary>
        public void Initialize()
        {
            
        }

        /// <summary>
        /// Clean Up CatManager, unsubscribe or remove values 
        /// </summary>
        public void CleanUp()
        {
            
        }
    }

    public interface ICatFactory
    {
        CatBehaviour GetRandomCat();
    }
}
