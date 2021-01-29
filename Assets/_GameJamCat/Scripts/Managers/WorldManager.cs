using Sirenix.OdinInspector;
using UnityEngine;

namespace GameJamCat
{
    public class WorldManager : MonoBehaviour
    {
        [Title("Properties")]
        [SerializeField]
        private int _lives = 3;

        [Title("Managers")]
        [SerializeField] private CatManager _catManager = null;
        [SerializeField] private TimeManager _timeManager = null;
        [SerializeField] private UIManager _uiManager = null;
        
        private void OnEnable()
        {
            _catManager.Initialize();
            _uiManager.Initialize();
            _timeManager.Initialize();
        }

        private void OnDisable()
        {
            _catManager.CleanUp();
            _uiManager.CleanUp();
            _timeManager.CleanUp();
        }
    }
}
