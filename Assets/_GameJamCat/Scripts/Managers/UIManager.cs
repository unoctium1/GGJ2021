using UnityEngine;

namespace GameJamCat
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private DossierViewBehaviour _dossierView = null;
        
        /// <summary>
        /// Initialize UIManager, setup values here
        /// </summary>
        public void Initialize()
        {
            _dossierView.Initialize();
        }

        /// <summary>
        /// Reset UI Values here, unsubscribe or reset values here
        /// </summary>
        public void CleanUp()
        {
            
        }
    }
}
